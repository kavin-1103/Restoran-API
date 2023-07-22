namespace Restaurant_Reservation_Management_System_Api.Model
{
    public class CartStore
    {
        public int CartStoreId { get; set; }
       // public AddToCart AddToCart { get; set; }    

        public int AddToCartId { get; set; }

        public int FoodItemId { get; set; }

        public int CategoryId { get; set; }

         public string CategoryName { get; set; }   

        public string ItemName { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
