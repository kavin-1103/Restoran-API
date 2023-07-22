using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Reservation_Management_System_Api.Data;
using Restaurant_Reservation_Management_System_Api.Dto.User.Reservation;
using Restaurant_Reservation_Management_System_Api.Dto.User.Table;
using Restaurant_Reservation_Management_System_Api.Model;
using Restaurant_Reservation_Management_System_Api.Services.User.ReservationService;

namespace Restaurant_Reservation_Management_System_Api.Controllers.UserController
{
    [Route("api/customer/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        private readonly IReservationServicesUser _reservationServices;

        public ReservationController(RestaurantDbContext context , IReservationServicesUser reservationServices)
        {
            _context = context;
            _reservationServices = reservationServices;
        }

		//[HttpPost]
		//[Route("Tables")]

		//public async Task<ActionResult<ServiceResponse<List<GetTableDtoUser>>>> GetAvailableTables(GetReservationDetailsForTableDtoUser getReservationDetailsForTableDtoUser)
		//{
		//	var response = await _reservationServices.GetAvailableTables(getReservationDetailsForTableDtoUser);

		//	if (response.Success == false)
		//	{
		//		return BadRequest(response);
		//	}
		//	return Ok(response);
		//}

		[HttpPost]
		[Route("Tables")]

		public async Task<ActionResult<ServiceResponse<List<TableAvailability>>>> GetAvailableTables(GetAvailableTablesDto getReservationDetailsForTableDtoUser)
		{
			var response = await _reservationServices.GetAvailableTables(getReservationDetailsForTableDtoUser);

			if (response.Success == false)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}
		[HttpPost("ReserveTable")]
		public async Task<ActionResult<ServiceResponse<Reservation>>> ReserveTable(CreateReservationDtoUser createReservationDtoUser)
		{

			var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
			if (customerIdClaim == null)
			{
				return BadRequest();
			}
			var customerId = customerIdClaim.Value;

			var response = await _reservationServices.ReserveTable(customerId,createReservationDtoUser);
			//if (!response.Success)
			//{
			//    return BadRequest(response);
			//}

			return Ok(response);
		}

		// GET: api/Reservations
		//[HttpGet]
		//public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
		//{
		//  if (_context.Reservations == null)
		//  {
		//      return NotFound();
		//  }
		//    return await _context.Reservations.ToListAsync();
		//}


		// GET: api/Reservations/5
		//[HttpGet("{id}")]
		//public async Task<ActionResult<Reservation>> GetReservation(int id)
		//{
		//  if (_context.Reservations == null)
		//  {
		//      return NotFound();
		//  }
		//    var reservation = await _context.Reservations.FindAsync(id);

		//    if (reservation == null)
		//    {
		//        return NotFound();
		//    }

		//    return reservation;
		//}

		// PUT: api/Reservations/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754


		// POST: api/Reservations
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
        public async Task<ActionResult<GetReservationDtoUser>> PostReservation(CreateReservationDtoUser createReservationDtoUser)
        {

            var response = await _reservationServices.PostReservation(createReservationDtoUser);

            if(response.Success==false)
            {
                return BadRequest(response);
            }
            return Ok(response);


        }

        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteReservation(int id)
        {
            var response = await _reservationServices.DeleteReservation(id);

            if(response.Success == false)
            {
                return BadRequest(response);

            }
            return Ok(response);
                
        }

        private bool ReservationExists(int id)
        {
            return (_context.Reservations?.Any(e => e.ReservationId == id)).GetValueOrDefault();
        }
    }
}
