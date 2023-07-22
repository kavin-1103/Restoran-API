using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Reservation_Management_System_Api.Data;
using Restaurant_Reservation_Management_System_Api.Model;

namespace Restaurant_Reservation_Management_System_Api.Controllers.AdminController
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisteredCustomerController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public RegisteredCustomerController(RestaurantDbContext context)
        {
            _context = context;
        }

        // GET: api/RegisteredCustomers
        [HttpGet]
        //[Authorize(Roles = "Admin")]
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
       // [Authorize(Roles = "Admin")]
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



        private bool RegisteredCustomerExists(string id)
        {
            return (_context.RegisteredCustomers?.Any(e => e.RegisteredCustomerId == id)).GetValueOrDefault();
        }
    }
}
