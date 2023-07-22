namespace Restaurant_Reservation_Management_System_Api.Model
{
    public class Order
    {
        public int OrderId { get; set; }

        public int TableId { get; set; }

        public Table Table { get; set; }    



        public DateTime OrderDate { get; set; } 

        public ICollection<OrderItem> OrderItems { get; set; }

        public string ApplicationUserId { get; set; }

        // Navigation property for ApplicationUser
        public ApplicationUser ApplicationUser { get; set; }




    }
}
