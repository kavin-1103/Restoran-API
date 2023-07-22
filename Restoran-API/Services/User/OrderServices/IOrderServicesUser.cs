using Restaurant_Reservation_Management_System_Api.Dto.User.Order;
using Restaurant_Reservation_Management_System_Api.Model;

namespace Restaurant_Reservation_Management_System_Api.Services.User.OrderServices
{
    public interface IOrderServicesUser
    {

        Task<ServiceResponse<GetOrderDtoUser>> AddOrder(string customerIdClaim , AddOrderDtoUser addOrderDtoUser);

        Task<ServiceResponse<IEnumerable<GetAllOrderDto>>> OrderDetails(string customerId);

        Task<ServiceResponse<IEnumerable<GetAllOrderDto>>> GetAllOrders();

		Task<ServiceResponse<(List<string> Dates, List<int> Counts)>> GetOrderCountForLast7Days();

		Task<ServiceResponse<int>> GetTotalOrderCount();
        Task<ServiceResponse<IEnumerable<OrderDto>>> GetOrdersForCustomer(string customerId);
	}
}
