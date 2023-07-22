using Restaurant_Reservation_Management_System_Api.Model;

namespace Restaurant_Reservation_Management_System_Api.Dto.User.Order
{
    public class AddOrderDtoUser
    {
       
 
        public int TableId { get; set; }    
        public DateTime OrderDate { get; set; }

        public ICollection<GetOrderItemDtoUser> OrderItems { get; set; }
    }
}
