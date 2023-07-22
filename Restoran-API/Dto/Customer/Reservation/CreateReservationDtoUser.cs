namespace Restaurant_Reservation_Management_System_Api.Dto.User.Reservation
{
    public class CreateReservationDtoUser
    {
        
        public int TableId{ get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NumberOfGuests { get; set; }
    }
}
