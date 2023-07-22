namespace Restaurant_Reservation_Management_System_Api.Model
{
    public class FoodItem
    {
        public int FoodItemId { get; set; } 

        public int CategoryId { get; set; } 

        public MenuCategory Category { get; set; } 
        
        public string ItemName { get; set; }    

        public string Description { get; set; } 

        public decimal Price { get; set; }  

        public ICollection<OrderItem> OrderItems { get; set; }



    }
}
