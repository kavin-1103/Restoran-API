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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetMenuCategoryDtoAdmin>>>> AddMenuCategory(AddMenuCategoryDtoAdmin addMenuCategoryDtoAdmin)
        {

            var response = await _menuCategoryServices.AddMenuCategory(addMenuCategoryDtoAdmin);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
            //if (_context.MenuCategories == null)
            //{
            //    return Problem("Entity set 'RestaurantDbContext.MenuCategories'  is null.");
            //}
            //  _context.MenuCategories.Add(menuCategory);
            //  await _context.SaveChangesAsync();

            //  return CreatedAtAction("GetMenuCategory", new { id = menuCategory.MenuCategoryId }, menuCategory);
        }

        // DELETE: api/MenuCategoriesControllerAdmin/5
        [HttpDelete("{id}")]
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
