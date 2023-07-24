using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Reservation_Management_System_Api.Data;
using Restaurant_Reservation_Management_System_Api.Dto.Admin.MenuCategory;
using Restaurant_Reservation_Management_System_Api.Model;
using Restaurant_Reservation_Management_System_Api.Services.Admin.MenuCategoryService;

namespace Restaurant_Reservation_Management_System_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuCategoryController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        private readonly IMenuCategoryServicesAdmin _menuCategoryServices;

        public MenuCategoryController(RestaurantDbContext context, IMenuCategoryServicesAdmin menuCategoryServices)
        {
            _context = context;
            _menuCategoryServices = menuCategoryServices;
        }

        // GET: api/MenuCategoriesControllerAdmin
        [HttpGet]
		[Authorize(Roles = "Admin, Customer")]

        //endpoint to get all menu category
		public async Task<ActionResult<IEnumerable<GetMenuCategoryDtoAdmin>>> GetMenuCategories()
        {
            var response = await _menuCategoryServices.GetMenuCategory();
            if (response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }

        // GET: api/MenuCategoriesControllerAdmin/5
        [HttpGet("{id}")]

		//endpoint to get menu category by id
		public async Task<ActionResult<MenuCategory>> GetMenuCategory(int id)
        {
            if (_context.MenuCategories == null)
            {
                return NotFound();
            }
            var menuCategory = await _context.MenuCategories.FindAsync(id);

            if (menuCategory == null)
            {
                return NotFound();
            }

            return menuCategory;
        }

        // PUT: api/MenuCategoriesControllerAdmin/5
        
        [HttpPut("{id}")]
		[Authorize(Roles = "Admin")]

		//endpoint to update the  menu category
		public async Task<ActionResult<ServiceResponse<GetMenuCategoryDtoAdmin>>> UpdateMenuCategory(int id, AddMenuCategoryDtoAdmin addMenuCategoryDtoAdmin)
        {
            var response = await _menuCategoryServices.UpdateMenuCategory(id, addMenuCategoryDtoAdmin);

            if (response.Success == false)
            {
                return NotFound(response);

            }
            return Ok(response);

        }

        // POST: api/MenuCategoriesControllerAdmin
    
        [HttpPost]
		[Authorize(Roles = "Admin")]

		//endpoint to  add new menu category
		public async Task<ActionResult<ServiceResponse<IEnumerable<GetMenuCategoryDtoAdmin>>>> AddMenuCategory(AddMenuCategoryDtoAdmin addMenuCategoryDtoAdmin)
        {

            var response = await _menuCategoryServices.AddMenuCategory(addMenuCategoryDtoAdmin);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
            
        }

        // DELETE: api/MenuCategoriesControllerAdmin/5
        [HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]

		//endpoint to Delete the menu category by id
		public async Task<ActionResult<ServiceResponse<string>>> DeleteMenuCategory(int id)
        {
            var response = await _menuCategoryServices.DeleteMenuCategory(id);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(response);
        }



        private bool MenuCategoryExists(int id)
        {
            return (_context.MenuCategories?.Any(e => e.MenuCategoryId == id)).GetValueOrDefault();
        }
    }
}
