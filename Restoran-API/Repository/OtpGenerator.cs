using System.Security.Cryptography;

namespace Restaurant_Reservation_Management_System_Api.Repository
{
	public class OtpGenerator
	{
		private const int OtpLength = 6;

		public string GenerateOtp()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var buffer = new byte[OtpLength];
				rng.GetBytes(buffer);

				var otp = BitConverter.ToInt32(buffer, 0) % (int)Math.Pow(10, OtpLength);
				otp = Math.Abs(otp);

				return otp.ToString("D" + OtpLength);
			}
		}
	}
}
