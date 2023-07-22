using Restaurant_Reservation_Management_System_Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_Reservation_Management_System_Api.Model
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        public int TableId { get; set; }
        public Table Table { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        public int NumberOfGuests { get; set; }

        public string ApplicationUserId { get; set; }   

        public ApplicationUser ApplicationUser { get; set; }    
    }


}