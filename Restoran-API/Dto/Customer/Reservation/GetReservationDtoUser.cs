using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Restaurant_Reservation_Management_System_Api.Dto.User.Reservation
{
    public class GetReservationDtoUser
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }    

        public int TableId { get; set; }    
        public int TableNumber { get; set; }
        public DateTime ReservationDate { get; set; }

        public DateTime StartTime { get; set; }
      
        public DateTime EndTime { get; set; }
        public int NumberOfGuests { get; set; }
    }
}
