namespace Restaurant_Reservation_Management_System_Api.Model
{
	public class TableAvailability
	{
		public int TableId { get; set; }
		public int TableNumber { get; set; }
		public int Capacity { get; set; }
		public bool IsAvailable { get; set; }
	}
}