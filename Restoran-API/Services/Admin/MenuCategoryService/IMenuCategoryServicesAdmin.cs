using Restaurant_Reservation_Management_System_Api.Dto.Admin.MenuCategory;
using Restaurant_Reservation_Management_System_Api.Dto.Admin.Table;
using Restaurant_Reservation_Management_System_Api.Model;

namespace Restaurant_Reservation_Management_System_Api.Services.Admin.MenuCategoryService
{
    public interface IMenuCategoryServicesAdmin
    {
        Task<ServiceResponse<IEnumerable<GetMenuCategoryDtoAdmin>>> AddMenuCategory(AddMenuCategoryDtoAdmin menuCategory);

        Task<ServiceResponse<IEnumerable<GetMenuCategoryDtoAdmin>>> GetMenuCategory();

        Task<ServiceResponse<string>> DeleteMenuCategory(int id);

        Task<ServiceResponse<GetMenuCategoryDtoAdmin>> UpdateMenuCategory(int id, AddMenuCategoryDtoAdmin addMenuCategoryDtoAdmin);
    }
}
