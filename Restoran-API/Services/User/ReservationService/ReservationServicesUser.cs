using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant_Reservation_Management_System_Api.Data;
using Restaurant_Reservation_Management_System_Api.Dto.User.Reservation;
using Restaurant_Reservation_Management_System_Api.Dto.User.Table;
using Restaurant_Reservation_Management_System_Api.Model;

namespace Restaurant_Reservation_Management_System_Api.Services.User.ReservationService
{
    public class ReservationServicesUser : IReservationServicesUser
    {
        private readonly RestaurantDbContext _context;

        private readonly IMapper _mapper;

        public ReservationServicesUser(RestaurantDbContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;   

        }

        public async Task<ServiceResponse<GetReservationDtoUser>> PostReservation(CreateReservationDtoUser createReservationDtoUser)
        {
            var serviceResponse =  new ServiceResponse<GetReservationDtoUser>();

            try
            {

                var findTable = await _context.Tables.FirstOrDefaultAsync(t => t.TableId == createReservationDtoUser.TableId);

                var tableId = findTable.TableId;

               // var reservationDate = createReservationDtoUser.ReservationDate;
               // DateTime dateOnly = reservationDate.Date;

                if(findTable.IsOccupied == true)
                {
                    serviceResponse.Message = "Table is Already Booked!";
                    serviceResponse.Success = false;
                    return serviceResponse;
                }

                if(createReservationDtoUser.NumberOfGuests > findTable.Capacity)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "The Number Of Guest are higher than the Table Capacity";
                    return serviceResponse;
                }
            
                var newReservation = new Reservation()
                {
                    //CustomerId = 2,
                    TableId = tableId,
                    ReservationDate = createReservationDtoUser.ReservationDate.Date,
                    StartTime = createReservationDtoUser.StartTime,
                    EndTime = createReservationDtoUser.EndTime,
                    NumberOfGuests = createReservationDtoUser.NumberOfGuests,
                    

                };

                findTable.IsOccupied = true; 
              

                _context.Reservations.Add(newReservation);

                await _context.SaveChangesAsync();

              

                //var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == newReservation.CustomerId);

               // var customerName = customer?.Name;

                var reservationDto = new GetReservationDtoUser()
                {
                  //  CustomerId = newReservation.CustomerId,
                    //CustomerName = newReservation.Customer.Name,
                    TableId = newReservation.TableId,
                    TableNumber = newReservation.Table.TableNumber,
                    ReservationDate = newReservation.ReservationDate,
                    StartTime = newReservation.StartTime,
                    EndTime = newReservation.EndTime,
                    NumberOfGuests = newReservation.NumberOfGuests
                };

                serviceResponse.Data = reservationDto;
                serviceResponse.Message = "Table Reserved Successfully";
                serviceResponse.Success = true;
            }
            catch(Exception ex)
            {
                serviceResponse.Message += ex.ToString();
                serviceResponse.Success = false;
            }

            return serviceResponse;

        }

        public async Task<ServiceResponse<string>> DeleteReservation(int id)
        {
            var serviceResponse = new ServiceResponse<string>();
            try
            {
                var reservation = await _context.Reservations.FindAsync(id);

                if(reservation == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Reservation Not Found With Id {id}";
                    return serviceResponse;
                }

                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
                serviceResponse.Success = true;
                serviceResponse.Message = "Deleted the Reservation Successfully";

            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                
            }
            return serviceResponse;

        }

		public async Task<ServiceResponse<Reservation>> ReserveTable(string customerIdClaim , CreateReservationDtoUser createReservationDtoUser)
		{
			var response = new ServiceResponse<Reservation>();


			var customerId = customerIdClaim;
			try
			{
				var table = await _context.Tables
					.FirstOrDefaultAsync(t => t.TableId == createReservationDtoUser.TableId);

				if (table == null)
				{
					response.Success = false;
					response.Message = "Table not found.";
					return response;
				}

				if (IsTableOccupied(table.TableId, createReservationDtoUser.ReservationDate, createReservationDtoUser.StartTime, createReservationDtoUser.EndTime))
				{
					response.Success = false;
					response.Message = "The selected table is already occupied for the specified reservation period.";
					return response;
				}

				if (createReservationDtoUser.NumberOfGuests > table.Capacity)
				{
					response.Success = false;
					response.Message = "The number of guests exceeds the table's capacity.";
					return response;
				}

				var newReservation = new Reservation
				{
					TableId = table.TableId,
					ReservationDate = createReservationDtoUser.ReservationDate.Date,
					StartTime = createReservationDtoUser.StartTime,
					EndTime = createReservationDtoUser.EndTime,
					NumberOfGuests = createReservationDtoUser.NumberOfGuests,
					ApplicationUserId = customerId, // Set the application user ID here (assuming a fixed value for this example)
				};

				_context.Reservations.Add(newReservation);

				var reservationsForSlot = _context.Reservations
					.Where(r => r.TableId == table.TableId &&
								r.ReservationDate.Date == createReservationDtoUser.ReservationDate.Date &&
								r.StartTime < createReservationDtoUser.EndTime &&
								r.EndTime > createReservationDtoUser.StartTime)
					.ToList();

				table.IsOccupied = reservationsForSlot.Count > 0;

				await _context.SaveChangesAsync();

				if(newReservation == null)
				{
					response.Success = true;
					response.Message = "No Tables Available For this Slot";
					return response;
				}

				response.Data = newReservation;
				response.Success = true;
				response.Message = "Table reserved successfully.";
				return response;
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = "Error reserving the table: " + ex.Message;
				return response;
			}
		}
		public async Task<ServiceResponse<List<TableAvailability>>> GetAvailableTables(GetAvailableTablesDto getAvailableTablesDto)
		{
			var response = new ServiceResponse<List<TableAvailability>>();

			try
			{
				// Retrieve all tables from the database with capacity greater than or equal to the requested number of guests
				var allTables = await _context.Tables
					.Where(table => table.Capacity >= getAvailableTablesDto.NumberOfGuests)
					.ToListAsync();

				// Generate the table availability for the specified reservation period
				var availableTables = new List<TableAvailability>();

				foreach (var table in allTables)
				{
					// Check if the table is not occupied during the specified reservation period
					if (!IsTableOccupied(table.TableId, getAvailableTablesDto.ReservationDate, getAvailableTablesDto.StartTime, getAvailableTablesDto.EndTime))
					{
						availableTables.Add(new TableAvailability
						{
							TableId = table.TableId,
							TableNumber = table.TableNumber,
							Capacity = table.Capacity,
							IsAvailable = true
						});
					}
				}

				response.Data = availableTables;
				response.Message = "Available tables retrieved successfully";
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = "Error retrieving available tables: " + ex.Message;
			}

			return response;
		}

		private bool IsTableOccupied(int tableId, DateTime reservationDate, DateTime startTime, DateTime endTime)
		{
			// Check if the start time is after the end time
			if (startTime >= endTime)
			{
				throw new ArgumentException("Start time should be before the end time.");
			}

			// Retrieve reservations for the specified table and date from the database
			var reservationsForTable = _context.Reservations
				.Where(r => r.TableId == tableId && r.ReservationDate.Date == reservationDate.Date &&
							!(r.StartTime >= endTime || r.EndTime <= startTime)) // Check for overlapping reservations
				.ToList();

			return reservationsForTable.Count > 0; // Table is occupied if there are any matching reservations
		}

	}
}
