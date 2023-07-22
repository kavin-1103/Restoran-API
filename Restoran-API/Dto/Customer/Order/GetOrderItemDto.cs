namespace Restaurant_Reservation_Management_System_Api.Dto.User.Order
{
    public class GetOrderItemDto
    {
        public int FoodItemId { get; set; }

        public string ItemName { get; set; }    
        public int Quantity { get; set; }
    }
}
