namespace Restaurant_Reservation_Management_System_Api.Dto.Admin.Table
{
    public class GetAllTableDtoAdmin
    {
        public int TableId { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public bool IsOccupied { get; set; }
    }
}
