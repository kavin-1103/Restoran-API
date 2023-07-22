using Microsoft.AspNetCore.Identity;

namespace Restaurant_Reservation_Management_System_Api.Model
{
    public class ApplicationUser : IdentityUser
    {

        public string ?Name {  get; set; }

		public bool IsVerified { get; set; }
		public string? Otp { get; set; }
		public DateTimeOffset? OtpExpiration { get; set; }
		public int OtpResendCount { get; set; } = 0;
		public ICollection<Reservation> ?Reservations { get; set; }
        public ICollection<Order> ?Orders { get; set; }
    }
}
