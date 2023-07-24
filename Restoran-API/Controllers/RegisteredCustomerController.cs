using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Reservation_Management_System_Api.Data;
using Restaurant_Reservation_Management_System_Api.Dto.Customer.Customer_Details;
using Restaurant_Reservation_Management_System_Api.Model;
using Restaurant_Reservation_Management_System_Api.Services.User.CustomerServices;

namespace Restaurant_Reservation_Management_System_Api.Controllers.AdminController
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisteredCustomerController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        private readonly ICustomerServicesUser _customerService;

        public RegisteredCustomerController(RestaurantDbContext context, ICustomerServicesUser customerService)
        {
            _context = context;
            _customerService = customerService; 
        }

        // GET: api/RegisteredCustomers
        [HttpGet]
        [Authorize(Roles = "Admin")]

        //endpoint to get the registered customers
        public async Task<ActionResult<ServiceResponse<IEnumerable<RegisteredCustomer>>>> GetRegisteredCustomers()
        {
            var response = new ServiceResponse<IEnumerable<RegisteredCustomer>>();
            if (_context.RegisteredCustomers == null)
             {

                    response.Success = false;
                    response.Message = "No Customers Found";

                  return NotFound(response);
             }

            response.Data = await _context.RegisteredCustomers.ToListAsync();
            response.Success = true;
            response.Message = "Fetched All Registered Customer Successfully!";

            return Ok(response);
        }

        // GET: api/RegisteredCustomers/5
        [HttpGet("{id}")]
		[Authorize(Roles = "Admin")]

        //endpoint to get the registered customer by id
		public async Task<ActionResult<RegisteredCustomer>> GetRegisteredCustomer(string id)
        {
          if (_context.RegisteredCustomers == null)
          {
              return NotFound();
          }
            var registeredCustomer = await _context.RegisteredCustomers.FindAsync(id);

            if (registeredCustomer == null)
            {
                return NotFound();
            }

            return registeredCustomer;
        }

        // GET: api/RegisteredCustomers
        [HttpGet]
        [Route("CustomerCount")]
        [Authorize(Roles = "Admin")]

		//endpoint to get the count of registered ciustomers
		public async Task<ActionResult<ServiceResponse<int>>> GetRegisteredCustomersCount()
        {
            var response = new ServiceResponse<int>();

            int customerCount = await _context.RegisteredCustomers.CountAsync();

            if (customerCount == 0)
            {
                response.Success = false;
                response.Message = "No Customers Found";
                return NotFound(response);
            }

            response.Data = customerCount;
            response.Success = true;
            response.Message = "Fetched Total Number of Registered Customers Successfully!";
            return Ok(response);
        }

		[HttpGet]
		[Route("GetCustomerDetails")]
		[Authorize(Roles = "Customer")]


		//endpoint to get the customer details
		public async Task<ActionResult<ServiceResponse<GetCustomerDetailsDto>>> GetCustomerDetails()
		{
			var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

			if (customerIdClaim == null)
			{
				return BadRequest();
			}
			var customerId = customerIdClaim.Value;

			var response = await _customerService.GetCustomerDetails(customerId);

			if (response.Success)
			{
				
				return Ok(response);
			}
			else
			{
				// Handle the case when customer details retrieval failed
				return BadRequest(response);
			}


		}



		private bool RegisteredCustomerExists(string id)
        {
            return (_context.RegisteredCustomers?.Any(e => e.RegisteredCustomerId == id)).GetValueOrDefault();
        }
    }
}
