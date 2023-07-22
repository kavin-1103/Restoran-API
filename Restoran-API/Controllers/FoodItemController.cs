using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Reservation_Management_System_Api.Data;
using Restaurant_Reservation_Management_System_Api.Dto.Admin.FoodItem;
using Restaurant_Reservation_Management_System_Api.Model;
using Restaurant_Reservation_Management_System_Api.Services.Admin.FoodItemService;
using Restaurant_Reservation_Management_System_Api.Services.Admin.MenuCategoryService;

namespace Restaurant_Reservation_Management_System_Api.Controllers.AdminController
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        private readonly IFoodItemServicesAdmin _foodItemServicesAdmin;

        public FoodItemController(RestaurantDbContext context , IFoodItemServicesAdmin foodItemServicesAdmin)
        {
            _context = context;
            _foodItemServicesAdmin = foodItemServicesAdmin;

        }

        // GET: api/FoodItemsControllerAdmin
        [HttpGet]
        
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetFoodItemDtoAdmin>>>> GetFoodItems()
        {
            var response = await _foodItemServicesAdmin.GetFoodItems();
            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);


        }

        // GET: api/FoodItemsControllerAdmin/5
        [HttpGet("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<FoodItem>> GetFoodItem(int id)
        {
          if (_context.FoodItems == null)
          {
              return NotFound();
          }
            var foodItem = await _context.FoodItems.FindAsync(id);

            if (foodItem == null)
            {
                return NotFound();
            }

            return foodItem;
        }

        // PUT: api/FoodItemsControllerAdmin/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetFoodItemDtoAdmin>>> UpdateFoodItem(int id, AddFoodItemDtoAdmin addFoodItemDtoAdmin)
        {
            var response = await _foodItemServicesAdmin.UpdateFoodItem(id, addFoodItemDtoAdmin);
            if(response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);    
        }

        // POST: api/FoodItemsControllerAdmin
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetFoodItemDtoAdmin>>>> AddFoodItem(AddFoodItemDtoAdmin addFoodItemDtoAdmin)
        {


            var response = await _foodItemServicesAdmin.AddFoodItem(addFoodItemDtoAdmin);
            if(response.Success == false)
            {
                return BadRequest(response);
           }
            return Ok(response);
        }

        // DELETE: api/FoodItemsControllerAdmin/5
        [HttpDelete("{id}")]
        
        public async Task<ActionResult<ServiceResponse<string>>> DeleteFoodItem(int id)
        {
            var response = await _foodItemServicesAdmin.DeleteFoodItem(id);
            if(response.Success==false)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }

        [HttpGet]
        [Route("CountFoodItems")]
       // [Authorize(Roles ="Admin")]

        public async Task<ActionResult<ServiceResponse<int>>> GetFoodItemsCount()
        {
            var response = await _foodItemServicesAdmin.GetFoodItemsCount();

            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);


        }

		[HttpGet]
		[Route("GetFoodItemByCategory")]
		public async Task<ActionResult<ServiceResponse<IEnumerable<GetFoodItemDtoAdmin>>>> GetFoodItemByCategory(int id)
		{
			var response = await _foodItemServicesAdmin.GetFoodItemByCategory(id);

			if (response.Success == false)
			{
				return BadRequest(response);

			}
			return Ok(response);

		}

		private bool FoodItemExists(int id)
        {
            return (_context.FoodItems?.Any(e => e.FoodItemId == id)).GetValueOrDefault();
        }
    }
}
