namespace Restaurant_Reservation_Management_System_Api.Dto.Auth
{
	public class ResetPasswordDto
	{
		public string Email { get; set; } = string.Empty;
		public string NewPassword { get; set; } = string.Empty;
	}
}
