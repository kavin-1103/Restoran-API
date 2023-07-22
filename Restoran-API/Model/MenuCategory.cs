namespace Restaurant_Reservation_Management_System_Api.Model
{
    public class MenuCategory
    {
        public int MenuCategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<FoodItem> FoodItems { get; set; }
    }
}
