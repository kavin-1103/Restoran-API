using Microsoft.AspNetCore.Mvc;
using Restaurant_Reservation_Management_System_Api.Dto.Admin.Table;
using Restaurant_Reservation_Management_System_Api.Model;

namespace Restaurant_Reservation_Management_System_Api.Services.Admin.TableService
{
    public interface ITableServicesAdmin
    {
        Task<ServiceResponse<IEnumerable<GetAllTableDtoAdmin>>> GetAllTables();

        Task<ServiceResponse<List<GetAllTableDtoAdmin>>> AddTable(AddTableDtoAdmin addTableDtoAdmin);

        Task<ServiceResponse<string>> DeleteTable(int id);

        Task<ServiceResponse<GetAllTableDtoAdmin>> UpdateTable(int id, AddTableDtoAdmin addTableDtoAdmin);

		Task<ServiceResponse<int>> GetTotalTableCount();
	}
}
