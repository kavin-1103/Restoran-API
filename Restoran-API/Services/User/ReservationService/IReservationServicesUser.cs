using Microsoft.AspNetCore.Mvc;
using Restaurant_Reservation_Management_System_Api.Dto.User.Reservation;
using Restaurant_Reservation_Management_System_Api.Dto.User.Table;
using Restaurant_Reservation_Management_System_Api.Model;

namespace Restaurant_Reservation_Management_System_Api.Services.User.ReservationService
{
    public interface IReservationServicesUser
    {
        Task<ServiceResponse<GetReservationDtoUser>> PostReservation(CreateReservationDtoUser createReservationDtoUser);

        Task<ServiceResponse<string>> DeleteReservation(int id);

        // Task<ServiceResponse<List<GetTableDtoUser>>> GetAvailableTables(GetReservationDetailsForTableDtoUser getReservationDetailsForTableDtoUser);

        Task<ServiceResponse<List<TableAvailability>>> GetAvailableTables(GetAvailableTablesDto getAvailableTablesDto);

        Task<ServiceResponse<Reservation>> ReserveTable(string customerID , CreateReservationDtoUser createReservationDtoUser);

	}

}
