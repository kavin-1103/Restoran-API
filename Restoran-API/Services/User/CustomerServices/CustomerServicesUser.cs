using AutoMapper;
using Restaurant_Reservation_Management_System_Api.Data;
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

        public async Task<ServiceResponse<GetCustomerDtoUser>> RegisterCustomer(RegisterCustomerDtoUser addCustomerDtoUser)
        {
            var serviceResponse = new ServiceResponse<GetCustomerDtoUser>();

            CreatePasswordHash(addCustomerDtoUser.password, out byte[] passwordHash, out byte[] passwordSalt);

          //  var passwordHash = passwordHash;
            //var passwordSalt = passswordSalt



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
