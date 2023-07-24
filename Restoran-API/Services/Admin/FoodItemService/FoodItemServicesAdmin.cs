using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Reservation_Management_System_Api.Data;
using Restaurant_Reservation_Management_System_Api.Dto.Admin.FoodItem;
using Restaurant_Reservation_Management_System_Api.Dto.Admin.MenuCategory;
using Restaurant_Reservation_Management_System_Api.Model;

namespace Restaurant_Reservation_Management_System_Api.Services.Admin.FoodItemService
{
    public class FoodItemServicesAdmin : IFoodItemServicesAdmin
    {
        private readonly RestaurantDbContext _context;

        private readonly IMapper _mapper;
        public FoodItemServicesAdmin(RestaurantDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


       

        //service to return all the food items
        public async Task<ServiceResponse<IEnumerable<GetFoodItemDtoAdmin>>> GetFoodItems()
        {
            var serviceResponse = new ServiceResponse<IEnumerable<GetFoodItemDtoAdmin>>();

            try
            {
                var getAllFoodItems = await _context.FoodItems.ToListAsync();

                if (getAllFoodItems == null)
                {
                    serviceResponse.Success = true;
                    serviceResponse.Message = "No Food Items Found!";
                    return serviceResponse;

                }
                var getFoodItemDtoList = new List<GetFoodItemDtoAdmin>();

                foreach (var foodItem in getAllFoodItems)
                {

                    
                    var category = await _context.MenuCategories.FindAsync(foodItem.CategoryId);
                    var categoryName = category.CategoryName;
                    var getFoodItemDto = new GetFoodItemDtoAdmin()
                    {
                        CategoryId = foodItem.CategoryId,
                        FoodItemId = foodItem.FoodItemId,
                       
                        CategoryName = categoryName ,
                        ItemName = foodItem.ItemName,
                        Price = foodItem.Price,
                        Description = foodItem.Description,
                    };
                    getFoodItemDtoList.Add(getFoodItemDto);
                }

              
               serviceResponse.Data = getFoodItemDtoList;
                serviceResponse.Message = "All Food Items are Fetched";
                serviceResponse.Success = true;

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        //ADding New Food Item to the Food Item Model
        public async Task<ServiceResponse<IEnumerable<GetFoodItemDtoAdmin>>> AddFoodItem(AddFoodItemDtoAdmin addFoodItemDtoAdmin)
        {

            var serviceResponse = new ServiceResponse<IEnumerable<GetFoodItemDtoAdmin>>();
            try
            {
                var categoryExist = await _context.MenuCategories.FindAsync(addFoodItemDtoAdmin.CategoryId);
                
                if(categoryExist == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "There is No Category For This Id ";
                    return serviceResponse;

                }

			    var existingFoodItems = await _context.FoodItems.ToListAsync();

			    // Perform case-insensitive comparison locally
			    var foodItemExist = existingFoodItems.FirstOrDefault(foodItem =>
				    string.Equals(foodItem.ItemName, addFoodItemDtoAdmin.ItemName, StringComparison.OrdinalIgnoreCase));

                //check for description and price 
			    if (addFoodItemDtoAdmin.Price <= 0 && string.IsNullOrEmpty(addFoodItemDtoAdmin.Description))
			    {
				    serviceResponse.Success = false;
				    serviceResponse.Message = "Both Description should not be empty and Price should be greater than 0";
				    return serviceResponse;
			    }
                //check whether the food item alreadyeist
			    if (foodItemExist != null)
			    {
				    serviceResponse.Success = false;
				    serviceResponse.Message = "There is Already a Food Item With This Name";
				    return serviceResponse;
			    }

                //check whether the description is empty or not
			    if (string.IsNullOrEmpty(addFoodItemDtoAdmin.Description))
			    {
				    serviceResponse.Success = false;
				    serviceResponse.Message = "Description should not be empty";
				    return serviceResponse;
			    }

			    if (addFoodItemDtoAdmin.Price <= 0)
			    {
				    serviceResponse.Success = false;
				    serviceResponse.Message = "Price should be greater than 0";
				    return serviceResponse;
			    }

                //mapping AddfoodItemDto to FoodItem model
			    var newFoodItem = _mapper.Map<FoodItem>(addFoodItemDtoAdmin);

                _context.FoodItems.Add(newFoodItem);

                await _context.SaveChangesAsync();

                //getting all the food items

                var allFoodItems = await _context.FoodItems.ToListAsync();

                
                var getFoodItemDtoList = _context.FoodItems.Select(m=>_mapper.Map<GetFoodItemDtoAdmin>(m)).ToList();


                serviceResponse.Data = getFoodItemDtoList;
                serviceResponse.Success = true;
                serviceResponse.Message = "Food Item Added Successfully";
            }


            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;

            }
            return serviceResponse;

        }

        //service to update the food item

        public async Task<ServiceResponse<GetFoodItemDtoAdmin>> UpdateFoodItem(int id, AddFoodItemDtoAdmin addFoodItemDtoAdmin)
        {
            var serviceResponse = new ServiceResponse<GetFoodItemDtoAdmin>();

            var existingFoodItem = await _context.FoodItems.FindAsync(id);

            //check if there is already a food item with the given id
            if (existingFoodItem == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Food Item Not Found With Id {id}";
                return serviceResponse;
            }
            existingFoodItem.CategoryId = addFoodItemDtoAdmin.CategoryId;
            existingFoodItem.ItemName = addFoodItemDtoAdmin.ItemName;
            existingFoodItem.Price = addFoodItemDtoAdmin.Price;
            existingFoodItem.Description = addFoodItemDtoAdmin.Description;
            


            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetFoodItemDtoAdmin>(existingFoodItem);
            serviceResponse.Success = true;
            serviceResponse.Message = "Updated the Food Item Successfully!";

            return serviceResponse;
        }

		//service to Delete the food item
		public async Task<ServiceResponse<string>> DeleteFoodItem(int id)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {



                if (_context.FoodItems == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "There is No Single Food Item Exist! ";
                    return serviceResponse;
                }

                var foodItemToDelete = await _context.FoodItems.FindAsync(id);
                if (foodItemToDelete == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"No Food Item Found With ID {id}";
                    return serviceResponse;


                }
                 //food item with the given id is removed
                _context.FoodItems.Remove(foodItemToDelete);
                await _context.SaveChangesAsync();

                serviceResponse.Message = "Food Item Deleted Successfully!";
                serviceResponse.Success = true;

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;

            }


            return serviceResponse;



        }

		//service to get the count of the food item

		public async Task<ServiceResponse<int>> GetFoodItemsCount()
        {

            var serviceResponse = new ServiceResponse<int>();
            int foodItemCount = await _context.FoodItems.CountAsync();
            

            if (foodItemCount == 0)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "No Food Items Found";
                return serviceResponse;
            }


            serviceResponse.Data = foodItemCount;
            serviceResponse.Success = true;
            serviceResponse.Message = "Fetched Total Number of Food Items Successfully!";
            return serviceResponse;


        }

		//service to get the food item by category

		public async Task<ServiceResponse<IEnumerable<GetFoodItemDtoAdmin>>> GetFoodItemByCategory(int id)
		{

			var serviceResponse = new ServiceResponse<IEnumerable<GetFoodItemDtoAdmin>>();
			try
			{



				var categoryExist = await _context.MenuCategories
					 .Include(c => c.FoodItems) // Include the FoodItems collection in the query
					 .FirstOrDefaultAsync(c => c.MenuCategoryId == id);


				if (categoryExist == null)
				{
					serviceResponse.Success = false;
					serviceResponse.Message = "No Food Item Found For the Category!";
					return serviceResponse;

				}
				var foodItemsDto = categoryExist.FoodItems
					.Select(foodItem => new GetFoodItemDtoAdmin
					{
						FoodItemId = foodItem.FoodItemId,
						CategoryId = foodItem.CategoryId,
						CategoryName = foodItem.Category.CategoryName,
						ItemName = foodItem.ItemName,
						Description = foodItem.Description,
						Price = foodItem.Price
						
					});

				serviceResponse.Data = foodItemsDto;
				serviceResponse.Success = true;



			}
			catch (Exception ex)
			{
				serviceResponse.Message = "Error in Retrieving Food Item For The given Category" + ex.Message;
				serviceResponse.Success = false;

			}
			return serviceResponse;
		}
	}
}
