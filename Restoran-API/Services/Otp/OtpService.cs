


//using System.Text.RegularExpressions;

//namespace restaurant_reservation_management_system_api.services.otp
//{
//	public class otpservice
//	{

//		public void sendotp(string email)
//		{
//			try
//			{
//				// Generate OTP
//				var otp = GenerateOtp();

//				// Compose email message
//				var fromEmail = "your-email@example.com";
//				var toEmail = email;
//				var subject = "OTP Verification";
//				var body = $"Your OTP: {otp}";

//				// Configure SMTP settings
//				var smtpClient = new SmtpClient("smtp.gmail.com")
//				{
//					Port = 587,
//					Credentials = new NetworkCredential("kavin.s2020cse@sece.ac.in", "skdd0143"),
//					EnableSsl = true,
//				};

//				// Send email
//				var mailMessage = new MailMessage(fromEmail, toEmail, subject, body);
//				smtpClient.Send(mailMessage);
//			}
//			catch (Exception ex)
//			{
//				// Handle exception
//				Console.WriteLine($"Error sending OTP: {ex.Message}");
//			}
//		}

//		private string GenerateOtp()
//		{
//			// Generate a random 6-digit OTP
//			var random = new Random();
//			var otp = random.Next(100000, 999999).ToString();

//			return otp;
//		}

//		private bool IsEmailValid(string email)
//		{
//			Regular expression pattern for email validation

//		   string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

//			Check if the email matches the pattern
//			bool isValid = Regex.IsMatch(email, pattern);

//			return isValid;
//		}
//	}
//}
