using Restaurant_Reservation_Management_System_Api.Dto.User.Customer;
using Restaurant_Reservation_Management_System_Api.Model;

namespace Restaurant_Reservation_Management_System_Api.Services.User.CustomerServices
{
    public interface ICustomerServicesUser
    {

        Task<ServiceResponse<GetCustomerDtoUser>> RegisterCustomer(RegisterCustomerDtoUser addCustomerDtoUser);
    }
}
