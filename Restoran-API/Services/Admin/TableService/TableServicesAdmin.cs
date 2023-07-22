using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Restaurant_Reservation_Management_System_Api.Data;
using Restaurant_Reservation_Management_System_Api.Dto.Admin.Table;
using Restaurant_Reservation_Management_System_Api.Model;
using System.Text.RegularExpressions;

namespace Restaurant_Reservation_Management_System_Api.Services.Admin.TableService
{
    public class TableServicesAdmin : ITableServicesAdmin
    {
        private readonly RestaurantDbContext _context;

        private readonly IMapper _mapper;


        public TableServicesAdmin(RestaurantDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<ServiceResponse<IEnumerable<GetAllTableDtoAdmin>>> GetAllTables()
        {
            var serviceResponse = new ServiceResponse<IEnumerable<GetAllTableDtoAdmin>>();

            try
            {


                var tables = await _context.Tables.ToListAsync();

                serviceResponse.Data = tables.Select(m => _mapper.Map<GetAllTableDtoAdmin>(m)).ToList();

                serviceResponse.Success = true;

                serviceResponse.Message = "All Tables are Fetched and Displayed";



            }

            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;



        }

        public async Task<ServiceResponse<List<GetAllTableDtoAdmin>>> AddTable(AddTableDtoAdmin addTableDtoAdmin)
        {

            var serviceResponse = new ServiceResponse<List<GetAllTableDtoAdmin>>();


            //var tableToAdd = _mapper.Map<Table>(addTableDtoAdmin);


            try
            {

                var tableToAdd = new Table()
                {
                    TableNumber = addTableDtoAdmin.TableNumber,
                    Capacity = addTableDtoAdmin.Capacity,
                    IsOccupied = addTableDtoAdmin.IsOccupied,

                };

                var tableNumberExist = await _context.Tables.AnyAsync(t => t.TableNumber == tableToAdd.TableNumber);

                if(tableNumberExist)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Table Already Exists!";

                    return serviceResponse;
                }

                _context.Tables.Add(tableToAdd);

                await _context.SaveChangesAsync();

                //var tables = await  _context.Tables.ToListAsync();

                serviceResponse.Data = await _context.Tables.Select(m => _mapper.Map<GetAllTableDtoAdmin>(m)).ToListAsync();

                serviceResponse.Success = true;

                serviceResponse.Message = "New Table Added Successfully!";


            }
            catch(Exception ex)
            {
                serviceResponse.Success = false; 

                serviceResponse.Message = ex.Message;
            }

            return serviceResponse; 


        }



        public async Task<ServiceResponse<GetAllTableDtoAdmin>> UpdateTable(int id,AddTableDtoAdmin addTableDtoAdmin)
        {
            var serviceResponse = new ServiceResponse<GetAllTableDtoAdmin>();

            var existingTable = await _context.Tables.FindAsync(id);

            if (existingTable == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Table Not Found With Id {id}";
                return serviceResponse;
            }
            existingTable.TableNumber = addTableDtoAdmin.TableNumber;
            existingTable.Capacity = addTableDtoAdmin.Capacity;
            existingTable.IsOccupied = addTableDtoAdmin.IsOccupied;
            
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetAllTableDtoAdmin>(existingTable);
            serviceResponse.Success = true;
            serviceResponse.Message = "Updated the Table Successfully!";

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> DeleteTable(int id)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {



                if (_context.Tables == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "There is No Single Tables Exist! ";
                    return serviceResponse;
                }

                var tableToDelete = await _context.Tables.FindAsync(id);
                if (tableToDelete == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"No Table Found With ID {id}";
                    return serviceResponse;


                }

                _context.Tables.Remove(tableToDelete);
                await _context.SaveChangesAsync();

                serviceResponse.Message = "Table Deleted Successfully!";
                serviceResponse.Success = true;

            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;    
                serviceResponse.Message = ex.Message;   

            }


            return serviceResponse;



        }


		public async Task<ServiceResponse<int>> GetTotalTableCount()
		{
			var serviceResponse = new ServiceResponse<int>();

			// Get the total count of orders from the database
			var totalCount = await _context.Tables.CountAsync();

			// Set the total order count in the Data property of the ServiceResponse
			serviceResponse.Data = totalCount;

			return serviceResponse;
		}


	}
}
