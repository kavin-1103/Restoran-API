﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Reservation_Management_System_Api.Data;
using Restaurant_Reservation_Management_System_Api.Dto.User.Order;
using Restaurant_Reservation_Management_System_Api.Model;
using Restaurant_Reservation_Management_System_Api.Services.User.OrderServices;

namespace Restaurant_Reservation_Management_System_Api.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodOrderController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
      


        private readonly IOrderServicesUser _orderServiceUser;

        public FoodOrderController(RestaurantDbContext context , IOrderServicesUser orderServicesUser )
        {
            _context = context;
            _orderServiceUser = orderServicesUser;
           
        }

       
        // GET: api/OrdersControllerUser
        [HttpGet]
        [Authorize(Roles ="Customer")]

        //endpoint for getting the order of a particular customer
        public async Task<ActionResult<IEnumerable<GetOrderDtoUser>>> GetOrders()
        {
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (customerIdClaim == null)
            {
                return BadRequest();
            }
            var customerId = customerIdClaim.Value;

            var response = await _orderServiceUser.OrderDetails(customerId);
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);    
              
        }

        [HttpGet]
        [Route("GetAllOrders")]
		[Authorize(Roles = "Admin")]

        //endpoint to get all the orders
		public async Task<ActionResult<ServiceResponse<IEnumerable<GetAllOrderDto>>>> GetAllOrders()
        {
            var response = await _orderServiceUser.GetAllOrders();
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        // GET: api/OrdersControllerUser/5
        [HttpGet("{id}")]

        //endpoint to get the order by id
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/OrdersControllerUser/5

        [HttpPut("{id}")]


        //endpoint to change the order
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/OrdersControllerUser

        //endpoint to add a new order
       
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<ServiceResponse<GetOrderDtoUser>>> AddOrder(AddOrderDtoUser addOrderDtoUser)
        {
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if(customerIdClaim==null)
            {
                return BadRequest();
            }
            var customerId = customerIdClaim.Value;
            var response = await _orderServiceUser.AddOrder(customerId ,addOrderDtoUser);
            
            if(response.Success==false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        
        }

        // DELETE: api/OrdersControllerUser/5
        [HttpDelete("{id}")]

        //endpoint to delete the order
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }


		[HttpGet]
		[Route("GetOrderCountForLast7Days")]
		[Authorize(Roles = "Admin")]

        //endpoint to get the Order for last seven days
		public async Task<IActionResult> GetOrderCountForLast7Days()
		{
			var response = await _orderServiceUser.GetOrderCountForLast7Days();
			if (!response.Success)
			{
				return BadRequest(response);
			}

			return Ok(new
			{
				Dates = response.Data.Dates,
				Counts = response.Data.Counts
			});


		}

		private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }

		[HttpGet("total-order-count")]
		[Authorize(Roles = "Admin")]

        //endpoint to get the total order count
		public async Task<IActionResult> GetTotalOrderCount()
		{
			var response = await _orderServiceUser.GetTotalOrderCount();

			if (!response.Success)
			{
				return BadRequest(response);
			}

			return Ok(response);
		}


		[HttpGet]
		[Route("GetOrdersOfCustomer")]
		[Authorize(Roles = "Customer")]

        //endpoint to get the all the orders of a particular customer
		public async Task<IActionResult> GetOrdersForCustomer()
		{
			// Get the currently logged-in user's Id from the ClaimsPrincipal
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var response = await _orderServiceUser.GetOrdersForCustomer(userId);
			if (!response.Success)
			{
				return BadRequest(response);

			}
			return Ok(response);

		}

	}
}
