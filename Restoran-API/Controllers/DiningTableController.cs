using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetAllTableDtoAdmin>>>> GetTables()
        {

            var response = await _tableServices.GetAllTables();

            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);


            //if (_context.Tables == null)
            //{
            //    return NotFound();
            //}
            //  return await _context.Tables.ToListAsync();
        }

        // GET: api/Tables/5
        [HttpGet("{id}")]
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetAllTableDtoAdmin>>>> AddTable(AddTableDtoAdmin addTableDtoAdmin)
        {

            var response = await _tableServices.AddTable(addTableDtoAdmin);

            if(response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);

            //if (_context.Tables == null)
            //{
            //    return Problem("Entity set 'RestaurantDbContext.Tables'  is null.");
            //}
            //_context.Tables.Add(table);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTable", new { id = table.TableId }, table);
        }

        // DELETE: api/Tables/5
        [HttpDelete("{id}")]
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
