namespace Restaurant_Reservation_Management_System_Api.Dto.User.Table
{
    public class GetReservationDetailsForTableDtoUser
    {
        public DateTime ReservationDate { get; set; }

        public DateTime StartTime { get; set; } 

        public DateTime EndTime { get; set; }   

        public int NumberOfGuests { get; set; } 


    }
}
