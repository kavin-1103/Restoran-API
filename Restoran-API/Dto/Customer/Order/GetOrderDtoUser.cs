using Restaurant_Reservation_Management_System_Api.Model;

namespace Restaurant_Reservation_Management_System_Api.Dto.User.Order
{
    public class GetOrderDtoUser
    {
        public int OrderId { get; set; }

        public string CustomerId { get; set; }

        public string ?CustomerName { get; set; }
        public int TableId { get; set; }

        public int ?TableNumber { get; set; } 

        public DateTime OrderDate { get; set; }

        public ICollection<GetOrderItemDtoUser> ?OrderItems { get; set; }
    }
}
