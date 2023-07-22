namespace Restaurant_Reservation_Management_System_Api.Dto.User.Customer
{
    public class GetCustomerDtoUser
    {
        public int CustomerId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ReservationId { get; set; }

        public string OrderId { get; set; } 

    }
}
