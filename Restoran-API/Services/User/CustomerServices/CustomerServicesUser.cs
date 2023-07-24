using AutoMapper;
using Restaurant_Reservation_Management_System_Api.Data;
using Restaurant_Reservation_Management_System_Api.Dto.Customer.Customer_Details;
using Restaurant_Reservation_Management_System_Api.Dto.User.Customer;
using Restaurant_Reservation_Management_System_Api.Model;

namespace Restaurant_Reservation_Management_System_Api.Services.User.CustomerServices
{
    public class CustomerServicesUser : ICustomerServicesUser
    {

        private readonly RestaurantDbContext _context;

        private readonly IMapper _mapper;

        public CustomerServicesUser(RestaurantDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        //service to store the new customer in Registered customer model
        public async Task<ServiceResponse<GetCustomerDtoUser>> RegisterCustomer(RegisterCustomerDtoUser addCustomerDtoUser)
        {
            var serviceResponse = new ServiceResponse<GetCustomerDtoUser>();

            //hashing the password 

            CreatePasswordHash(addCustomerDtoUser.password, out byte[] passwordHash, out byte[] passwordSalt);

          

            var newCustomer = new Customer()
            {
                 
                 Name = addCustomerDtoUser.Name,
                 Email = addCustomerDtoUser.Email,
                 Phone = addCustomerDtoUser.Phone,
                 
                 
                 
            };


            newCustomer.PasswordHash = passwordHash;
            newCustomer.PasswordSalt = passwordSalt;
            _context.Customers.Add(newCustomer);

            await _context.SaveChangesAsync();

            var newGetCustomerDtoUser = new GetCustomerDtoUser()
            {
                CustomerId = newCustomer.CustomerId,
                Name = newCustomer.Name ,
                Email = newCustomer.Email,
                Phone = newCustomer.Phone,
            };

            serviceResponse.Data = newGetCustomerDtoUser;
            serviceResponse.Success = true;
            serviceResponse.Message ="new Customer Created Successfully";
            return serviceResponse;
        }

        //service to get the details of particular customer

		public async Task<ServiceResponse<GetCustomerDetailsDto>> GetCustomerDetails(string customerId)
		{
			var response = new ServiceResponse<GetCustomerDetailsDto>();

			try
			{
				

				if (string.IsNullOrEmpty(customerId))
				{
					response.Success = false;
					response.Message = "Customer ID not found in claims.";
					return response;
				}

				// Get the ApplicationUser by the customer ID
				var customer = await _context.Users.FindAsync(customerId);

				if (customer == null)
				{
					response.Success = false;
					response.Message = "Customer not found.";
					return response;
				}

				// Get the number of orders for the customer
				var numberOfOrders = _context.Orders.Count(o => o.ApplicationUserId == customerId);

				// Create the GetCustomerDetailsDto object
				var customerDetails = new GetCustomerDetailsDto
				{
					Name = customer.Name,
					Email = customer.Email,
					Phone = customer.PhoneNumber,
					NumberOfOrders = numberOfOrders,
				};

				response.Data = customerDetails;
				response.Success = true;
				response.Message = "Fetched Your Details";
			}
			catch (Exception ex)
			{
				
				response.Success = false;
				response.Message = "An error occurred while fetching customer details.";
				
			}

			return response;


		}

		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }

    }
}
