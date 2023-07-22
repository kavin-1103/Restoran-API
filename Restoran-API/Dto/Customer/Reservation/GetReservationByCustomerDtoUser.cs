namespace Restaurant_Reservation_Management_System_Api.Dto.User.Reservation
{
    public class GetReservationByCustomerDtoUser
    {
        public int TableId { get; set; }
        public int TableNumber { get; set; }


        public DateTime ReservationDate { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        public int NumberOfGuests { get; set; }

        public int OrderId { get; set; }


    }
}
