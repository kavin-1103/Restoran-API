using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Reservation_Management_System_Api.Data;
using Restaurant_Reservation_Management_System_Api.Dto.Admin.Table;
using Restaurant_Reservation_Management_System_Api.Model;
using Restaurant_Reservation_Management_System_Api.Services.Admin.TableService;

namespace Restaurant_Reservation_Management_System_Api.Controllers.AdminController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiningTableController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        private readonly ITableServicesAdmin _tableServices;

        public DiningTableController(RestaurantDbContext context, ITableServicesAdmin tableServices)
        {
            _context = context;
            _tableServices = tableServices;
        }

        // GET: api/Tables
        [HttpGet]


        //endpoint to get all the dining tables

        public async Task<ActionResult<ServiceResponse<IEnumerable<GetAllTableDtoAdmin>>>> GetTables()
        {

            var response = await _tableServices.GetAllTables();

            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);


          
        }

        // GET: api/Tables/5
        [HttpGet("{id}")]

		//endpoint to get all the dining tables by id
		public async Task<ActionResult<Table>> GetTable(int id)
        {
            if (_context.Tables == null)
            {
                return NotFound();
            }
            var table = await _context.Tables.FindAsync(id);

            if (table == null)
            {
                return NotFound();
            }

            return table;
        }

      
        // PUT: api/Tables/5
      
        [HttpPut("{id}")]

        [Authorize(Roles ="Admin")]

		//endpoint to get update the dining table
		public async Task<ActionResult<ServiceResponse<GetAllTableDtoAdmin>>> UpdateTable(int id , AddTableDtoAdmin addTableDtoAdmin)
        {

            var response = await _tableServices.UpdateTable(id,addTableDtoAdmin);

            if(response.Success == false)
            {
                return NotFound(response);

            }
            return Ok(response);    
          
        }

        // POST: api/Tables
       
        [HttpPost]

		[Authorize(Roles = "Admin")]

		//endpoint to add  the dining tables

		public async Task<ActionResult<ServiceResponse<List<GetAllTableDtoAdmin>>>> AddTable(AddTableDtoAdmin addTableDtoAdmin)
        {

            var response = await _tableServices.AddTable(addTableDtoAdmin);

            if(response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);

          
        }

        // DELETE: api/Tables/5
        [HttpDelete("{id}")]

		[Authorize(Roles = "Admin")]

		//endpoint to delete the dining tables
		public async Task<ActionResult<ServiceResponse<string>>> DeleteTable(int id)
        {

            var response = await _tableServices.DeleteTable(id);

            if(response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(response);

        }



		[HttpGet]
		[Route("total-table-count")]
		[Authorize(Roles = "Admin")]

		//endpoint to get the  count
		public async Task<ActionResult<ServiceResponse<int>>> GetTotalTableCount()
		{
			var response = await _tableServices.GetTotalTableCount();

			if (!response.Success)
			{
				return BadRequest(response);
			}

			return Ok(response);

		}

		private bool TableExists(int id)
        {
            return (_context.Tables?.Any(e => e.TableId == id)).GetValueOrDefault();
        }
    }
}
