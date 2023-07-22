namespace Restaurant_Reservation_Management_System_Api.Model
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

      //  public ICollection<Reservation> Reservations { get; set; }
      
    }
}
