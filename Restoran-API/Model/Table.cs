using Restaurant_Reservation_Management_System_Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Serialization;

namespace Restaurant_Reservation_Management_System_Api.Model
{
    public class Table
    {
        public int TableId { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public bool IsOccupied { get; set; }

        [JsonIgnore]
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

}
