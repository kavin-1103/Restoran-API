using Microsoft.AspNetCore.Mvc;
using Restaurant_Reservation_Management_System_Api.Dto.Admin.FoodItem;
using Restaurant_Reservation_Management_System_Api.Model;

namespace Restaurant_Reservation_Management_System_Api.Services.Admin.FoodItemService
{
    public interface IFoodItemServicesAdmin
    {

        Task<ServiceResponse<IEnumerable<GetFoodItemDtoAdmin>>> GetFoodItems();
        Task<ServiceResponse<IEnumerable<GetFoodItemDtoAdmin>>> AddFoodItem(AddFoodItemDtoAdmin addFoodItemDtoAdmin);

        Task<ServiceResponse<GetFoodItemDtoAdmin>> UpdateFoodItem(int id, AddFoodItemDtoAdmin addFoodItemDtoAdmin);
        Task<ServiceResponse<string>> DeleteFoodItem(int id);

        Task<ServiceResponse<int>> GetFoodItemsCount();

		Task<ServiceResponse<IEnumerable<GetFoodItemDtoAdmin>>> GetFoodItemByCategory(int id);
	}
}
