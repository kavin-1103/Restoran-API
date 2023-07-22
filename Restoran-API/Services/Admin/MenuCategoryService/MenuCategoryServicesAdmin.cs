using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurant_Reservation_Management_System_Api.Data;
using Restaurant_Reservation_Management_System_Api.Dto.Admin.MenuCategory;
using Restaurant_Reservation_Management_System_Api.Dto.Admin.Table;
using Restaurant_Reservation_Management_System_Api.Model;

namespace Restaurant_Reservation_Management_System_Api.Services.Admin.MenuCategoryService
{
    public class MenuCategoryServicesAdmin : IMenuCategoryServicesAdmin
    {
        private readonly RestaurantDbContext _context;

        private readonly IMapper _mapper;
        public MenuCategoryServicesAdmin(RestaurantDbContext context, IMapper mapper) 
        { 
            _context = context;
            _mapper = mapper;
        }  


        public async Task<ServiceResponse<IEnumerable<GetMenuCategoryDtoAdmin>>> GetMenuCategory ()
        {
            var serviceResponse = new ServiceResponse<IEnumerable<GetMenuCategoryDtoAdmin>>();

            
            
                var getAllMenuCategory = await _context.MenuCategories.ToListAsync();

                if (getAllMenuCategory == null)
                {
                    serviceResponse.Success = true;
                    serviceResponse.Message = "No Menu Catrgories Found!";
                    return serviceResponse;

                }
                serviceResponse.Data = getAllMenuCategory.Select(m => _mapper.Map<GetMenuCategoryDtoAdmin>(m)).ToList();
                serviceResponse.Message = "All Menu Categories are Fetched";
                serviceResponse.Success = true;

           
            return serviceResponse;
        }

        public async Task<ServiceResponse<IEnumerable<GetMenuCategoryDtoAdmin>>> AddMenuCategory(AddMenuCategoryDtoAdmin addMenuCategoryDtoAdmin)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<GetMenuCategoryDtoAdmin>>();

            try
            {
                if(int.TryParse(addMenuCategoryDtoAdmin.CategoryName ,out _))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Only the Strings are accepted for MenuCategory!";
                    return serviceResponse;

                }



                var newMenuCategory = new MenuCategory()
                {
                    CategoryName = addMenuCategoryDtoAdmin.CategoryName,

                };
                _context.MenuCategories.Add(newMenuCategory);

                await _context.SaveChangesAsync();

                var getAllMenuCategory = await _context.MenuCategories.ToListAsync();

                var getMenuCategoryDto = getAllMenuCategory.Select(m => _mapper.Map<GetMenuCategoryDtoAdmin>(m)).ToList();


                serviceResponse.Data = getMenuCategoryDto;
                serviceResponse.Success = true;
                serviceResponse.Message = "New Menu Category Added Successfully";
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Error In Adding New Menu Category : " + ex.Message;
            }


            return serviceResponse;


        }

        public async Task<ServiceResponse<GetMenuCategoryDtoAdmin>> UpdateMenuCategory(int id, AddMenuCategoryDtoAdmin addMenuCategoryDtoAdmin)
        {
            var serviceResponse = new ServiceResponse<GetMenuCategoryDtoAdmin>();

            var existingMenuCategory = await _context.MenuCategories.FindAsync(id);

            if (existingMenuCategory == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Menu Category Not Found With Id {id}";
                return serviceResponse;
            }
            existingMenuCategory.CategoryName = addMenuCategoryDtoAdmin.CategoryName;
      

            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetMenuCategoryDtoAdmin>(existingMenuCategory);
            serviceResponse.Success = true;
            serviceResponse.Message = "Updated the Menu Category Successfully!";

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> DeleteMenuCategory(int id)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {



                if (_context.MenuCategories== null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "There is No Single Menu Category Exist! ";
                    return serviceResponse;
                }

                var menuCategoryToDelete = await _context.MenuCategories.FindAsync(id);
                if (menuCategoryToDelete == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"No Table Found With ID {id}";
                    return serviceResponse;


                }

                _context.MenuCategories.Remove(menuCategoryToDelete);
                await _context.SaveChangesAsync();

                serviceResponse.Message = "Menu Category Deleted Successfully!";
                serviceResponse.Success = true;

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;

            }


            return serviceResponse;



        }




    }
}
