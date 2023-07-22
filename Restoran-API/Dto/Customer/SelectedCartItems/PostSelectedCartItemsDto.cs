namespace Restaurant_Reservation_Management_System_Api.Dto.SelectedCartItems
{
    public class PostSelectedCartItemsDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public int FoodItemId { get; set; } 

        public string ItemName { get; set; }    

        public int Price { get; set; }  
        public int Quantity { get; set; }   
    }
}
