using System.ComponentModel.DataAnnotations;

namespace Restaurant_Reservation_Management_System_Api.Dto.Auth
{
	public class ForgotPasswordDto
	{
		[Required]
		[EmailAddress]
		public string? Email { get; set; }
		[Required]
		public string? ClientURI { get; set; }
	}
}
