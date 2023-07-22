namespace Restaurant_Reservation_Management_System_Api.Model
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }    

        public int OrderId { get; set; }    

        public Order Order { get; set; }    

        public int FoodItemId { get; set; } 

        public FoodItem FoodItem { get; set; }  

        public int Quantity { get; set; }  
        

    }
}
