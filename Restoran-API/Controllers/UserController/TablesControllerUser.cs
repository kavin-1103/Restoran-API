//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Restaurant_Reservation_Management_System_Api.Data;
//using Restaurant_Reservation_Management_System_Api.Dto.User.Table;
//using Restaurant_Reservation_Management_System_Api.Model;

//namespace Restaurant_Reservation_Management_System_Api.Controllers.UserController
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TablesControllerUser : ControllerBase
//    {
//        private readonly RestaurantDbContext _context;

//        public TablesControllerUser(RestaurantDbContext context)
//        {
//            _context = context;
//        }

//        // GET: api/TablesControllerUser
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Table>>> GetTables()
//        {
//          if (_context.Tables == null)
//          {
//              return NotFound();
//          }
//            return await _context.Tables.ToListAsync();
//        }



//        // GET: api/TablesControllerUser/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Table>> GetTable(int id)
//        {
//          if (_context.Tables == null)
//          {
//              return NotFound();
//          }
//            var table = await _context.Tables.FindAsync(id);

//            if (table == null)
//            {
//                return NotFound();
//            }

//            return table;
//        }
       


//        // PUT: api/TablesControllerUser/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutTable(int id, Table table)
//        {
//            if (id != table.TableId)
//            {
//                return BadRequest();
//            }

//            _context.Entry(table).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!TableExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // POST: api/TablesControllerUser
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<Table>> PostTable(Table table)
//        {
//          if (_context.Tables == null)
//          {
//              return Problem("Entity set 'RestaurantDbContext.Tables'  is null.");
//          }
//            _context.Tables.Add(table);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetTable", new { id = table.TableId }, table);
//        }

//        // DELETE: api/TablesControllerUser/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteTable(int id)
//        {
//            if (_context.Tables == null)
//            {
//                return NotFound();
//            }
//            var table = await _context.Tables.FindAsync(id);
//            if (table == null)
//            {
//                return NotFound();
//            }

//            _context.Tables.Remove(table);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool TableExists(int id)
//        {
//            return (_context.Tables?.Any(e => e.TableId == id)).GetValueOrDefault();
//        }
//    }
//}
