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

		

		[HttpPost]
		[Route("Tables")]
		[Authorize(Roles = "Customer")]

		//endpoint to get the avilable tables

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
		[Authorize(Roles = "Customer")]

		//endpoint to reserve the table
		public async Task<ActionResult<ServiceResponse<Reservation>>> ReserveTable(CreateReservationDtoUser createReservationDtoUser)
		{

			var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
			if (customerIdClaim == null)
			{
				return BadRequest();
			}
			var customerId = customerIdClaim.Value;

			var response = await _reservationServices.ReserveTable(customerId,createReservationDtoUser);
			

			return Ok(response);
		}

		
		
	
		


		// POST: api/Reservations
		
		[HttpPost]
		[Authorize(Roles = "Customer")]

		//endpoint to add new reservation
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
		[Authorize(Roles = "Customer")]

		//endpoint to delete the reservation
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
