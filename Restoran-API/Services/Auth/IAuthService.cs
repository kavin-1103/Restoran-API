using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Restaurant_Reservation_Management_System_Api.Dto.Auth;
using Restaurant_Reservation_Management_System_Api.Model;

namespace Restaurant_Reservation_Management_System_Api.Services.Auth
{
    public interface IAuthService
    {
		Task<ServiceResponse<string>> RegisterUserAsync(RegisterRequestDto registerRequestDto);

		//Task<IdentityResult> LoginUserAsync(LoginRequestDto loginRequestDto);

		Task<ServiceResponse<string>> ForgotPassword(string email);
		Task<ServiceResponse<string>> Verify(VerifyOtpDto verifyOtpDto);

		

		Task<ServiceResponse<string>> EmailVerification(EmailVerificationDto emailVerificationDto);
		Task<ServiceResponse<string>> ResetPassword(PasswordChangeDto passswordChangeDto);
	}
}
