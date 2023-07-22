namespace Restaurant_Reservation_Management_System_Api.Dto.User.Order
{
	public class OrderDto
	{
		public int OrderId { get; set; }
		public DateTime OrderDate { get; set; }
		public DateTime? ReservationDate { get; set; } // Assuming ReservationDate can be nullable if there's no reservation
		public int TableNumber { get; set; }
		public List<FoodItemDto> FoodItems { get; set; }

	}
}
