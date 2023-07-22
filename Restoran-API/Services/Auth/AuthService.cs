using EmailService;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using Restaurant_Reservation_Management_System_Api.Data;
using Restaurant_Reservation_Management_System_Api.Dto.Auth;
using Restaurant_Reservation_Management_System_Api.Model;
using Restaurant_Reservation_Management_System_Api.Repository;
using System.Drawing.Text;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace Restaurant_Reservation_Management_System_Api.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly RestaurantDbContext _context;
        private readonly IEmailSender _emailSender;

        public AuthService(UserManager<ApplicationUser> userManager , ITokenRepository tokenRepository , RestaurantDbContext context, IEmailSender emailSender)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _context = context;
			_emailSender = emailSender;
        }

		public async Task<ServiceResponse<string>> RegisterUserAsync(RegisterRequestDto registerRequestDto)
		{
			ServiceResponse<string> response = new ServiceResponse<string>();

			if (registerRequestDto.Email == null)
			{
				response.Success = false;
				response.Message = "Email cannot be null";
				return response;
			}

			if (!IsEmailValid(registerRequestDto.Email))
			{
				response.Success = false;
				response.Message = "Invalid email address.";
				return response;
			}

			var ExistingEmail = await _userManager.FindByEmailAsync(registerRequestDto.Email);

			if (ExistingEmail != null)
			{
				response.Success = false;
				response.Message = "Email already exists";
				return response;
			}

			var user = new ApplicationUser
			{
				UserName = registerRequestDto.Name,
				Name = registerRequestDto.Name,
				Email = registerRequestDto.Email,
				PhoneNumber = registerRequestDto.PhoneNumber,
			};

			var passwordHashed = await _userManager.CreateAsync(user, registerRequestDto.Password);

			await _userManager.AddToRoleAsync(user, "Customer");

			OtpGenerator otpGenerator = new OtpGenerator();
			string otp = otpGenerator.GenerateOtp();

			DateTimeOffset indianTime = DateTimeOffset.UtcNow.ToOffset(TimeZoneInfo.FindSystemTimeZoneById("India Standard Time").BaseUtcOffset);
			DateTimeOffset otpExpiration = indianTime.AddDays(1);

			user.Otp = otp;
			user.OtpExpiration = otpExpiration;

			// Save the changes to the database
			await _context.SaveChangesAsync();

			var employeeMessage = new Message(new string[] { registerRequestDto.Email }, "Verification", "\n\n" +
					 "Thank you for registering with us. Your OTP is: " + otp + "\n\n" +
					 "Please use this OTP to complete the registration process." + "\n\n" +
					 "Best regards,\n" +
					 "Restoran");


			_emailSender.SendEmail(employeeMessage);

			response.Success = true;
			response.Message = "Email Sent Successfully. Please check Your Mail for OTP";
			return response;
	

		//}
		//catch (Exception ex)
		//{

		//}

		//         if(Verify())
		//         {
		//	try
		//	{
		//		var user = new ApplicationUser
		//		{
		//			UserName = registerRequestDto.Name,
		//			Name = registerRequestDto.Name,
		//			Email = registerRequestDto.Email,
		//			PhoneNumber = registerRequestDto.PhoneNumber,



		//		};
		//		var passHashed = await _userManager.CreateAsync(user, registerRequestDto.Password);

		//		if (passHashed.Succeeded)
		//		{
		//			await _userManager.AddToRoleAsync(user, "Customer");
		//		}

		//		//Store in Registered Customer Table 

		//		StoreInRegisteredCustomer(user);






		//		await _context.Users.AddAsync(user);



		//		await _context.SaveChangesAsync();



		//		response.Data = "User Registered Successfully";
		//		return response;
		//	}


		//	catch (Exception ex)
		//	{
		//		// Handle exception if needed

		//		response.Success = false;
		//		response.Message = "An error occurred during registration.";
		//		return response;
		//	}
		//}








		//-----
		//var user = new ApplicationUser
		//         {
		//             UserName = registerRequestDto.Name,
		//             Name = registerRequestDto.Name ,
		//             Email = registerRequestDto.Email ,
		//             PhoneNumber = registerRequestDto.PhoneNumber,



		//         };
		//         var result = await _userManager.CreateAsync(user,registerRequestDto.Password);

		//         if(result.Succeeded)
		//         {
		//             await _userManager.AddToRoleAsync(user, "Customer");
		//         }

		//         //Store in Registered Customer Table 

		//         StoreInRegisteredCustomer(user);

		//         return result;


	}

	//public async Task<LoginResponseDto> LoginUserAsync(LoginRequestDto loginRequestDto)
	//{


	//    //Check Email
	//    var identityUser = await _userManager.FindByEmailAsync(loginRequestDto.Email);

	//    if(identityUser is not null)
	//    {
	//        //Check Password

	//        var checkPasswordResult = await _userManager.CheckPasswordAsync(identityUser, loginRequestDto.Password);

	//        if(checkPasswordResult)
	//        {
	//            var roles = await _userManager.GetRolesAsync(identityUser);

	//            //Create a Toeken and Response

	//            var jwtToken = _tokenRepository.CreateJwtToken(identityUser, roles.ToList());
	//            var response = new LoginResponseDto()
	//            {
	//                Email = loginRequestDto.Email,
	//                Roles = roles.ToList(),
	//                Token = "TOKEN" ,

	//            };      



	//        }

	//    }

	private async Task StoreInRegisteredCustomer(ApplicationUser user)
        {
            var registeredCustomer = new RegisteredCustomer()
            {
                RegisteredCustomerId = user.Id ,
                CustomerName = user.Name,
                Email = user.Email , 
                PhoneNumber = user.PhoneNumber,

            };
            _context.RegisteredCustomers.Add(registeredCustomer);

            await _context.SaveChangesAsync();
            
        }


		public async Task<ServiceResponse<string>> EmailVerification(EmailVerificationDto emailVerificationDto)
		{
			var response = new ServiceResponse<string>();

			var userExist = await _userManager.FindByEmailAsync(emailVerificationDto.Email);
			if (userExist == null)
			{
				response.Success = false;
				response.Message = "User Already Exist";

			}
			OtpGenerator otpGenerator = new OtpGenerator();

			string otp = otpGenerator.GenerateOtp();

			// Get the current Indian time
			DateTimeOffset indianTime = DateTimeOffset.UtcNow.ToOffset(TimeZoneInfo.FindSystemTimeZoneById("India Standard Time").BaseUtcOffset);

			// Add the expiration time in minutes
			DateTimeOffset otpExpiration = indianTime.AddMinutes(10);

			var registerUser = new ApplicationUser()
			{
				UserName = emailVerificationDto.Name,
				Name = emailVerificationDto.Name , 
				Email = emailVerificationDto.Email ,
				PhoneNumber = emailVerificationDto.PhoneNumber ,
				Otp =  otp ,
				OtpExpiration = otpExpiration,
				IsVerified = false

			};
			var newUser = await _userManager.CreateAsync(registerUser, emailVerificationDto.Password);

			response.Success = true;
			response.Message = "Otp Has Sent to Your Email";
			return response;
		}

		public async Task<ServiceResponse<string>> ForgotPassword(string email)
        {
            var response = new ServiceResponse<string>();
            //var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }
            //if (!IsEmailValid(email))
            //{
            //	response.Success = false;
            //	response.Message = "Invalid email address.";
            //	return response;
            //}
            //if (!await EmailExists(email))
            //{
            //	response.Success = false;
            //	response.Message = "Email does not exists";
            //	return response;
            //}
            //if (user.IsVerified == false)
            //{
            //    response.Success = false;
            //    response.Message = "User Not Verified";
            //    return response;
            //}

		

			OtpGenerator otpGenerator = new OtpGenerator();

            string otp = otpGenerator.GenerateOtp();
			

            // Get the current Indian time
            DateTimeOffset indianTime = DateTimeOffset.UtcNow.ToOffset(TimeZoneInfo.FindSystemTimeZoneById("India Standard Time").BaseUtcOffset);

            // Add the expiration time in minutes
            DateTimeOffset otpExpiration = indianTime.AddMinutes(10);

            user.Otp = otp;
            user.OtpExpiration = otpExpiration;
            user.IsVerified = false;
            await _context.SaveChangesAsync();



			var employeeMessage = new Message(new string[] { email}, "Verification", $"Dear {user.Name},\n\nYou have requested to reset your password on our website. Your One-Time Password (OTP) for password reset is: "+otp+"\n\nPlease use this OTP to reset your password. This OTP is valid for a limited time and can be used only once.\n\nIf you did not request a password reset, please ignore this email.");
			_emailSender.SendEmail(employeeMessage);


			response.Data = "Please check your email for OTP.";
            return response;
        }




		public async Task<ServiceResponse<string>> Verify(VerifyOtpDto verifyOtpDto)
		{
			ServiceResponse<string> response = new ServiceResponse<string>();


			var user = await _userManager.FindByEmailAsync(verifyOtpDto.email);
			if (user != null)
			{
				if (user.Otp == verifyOtpDto.otp)
				{
					if (user.OtpExpiration > DateTimeOffset.UtcNow)
					{
						//await StoreInRegisteredCustomer(user);
						user.Otp = null;
						user.OtpExpiration = null;
						user.IsVerified = true;
						await _context.SaveChangesAsync();
						response.Success = true;
						response.Message = "OTP verification successful.";
						var employeeMessage = new Message(new string[] { verifyOtpDto.email }, "Registration Successfull", "\n\n" +
						 "Thank you for registering with 'Restoran'. We are delighted to welcome you to our platform!" + "\n\n" +
						 "If you have any questions or need assistance, please don't hesitate to reach out to us." + "\n\n" +
						 "Best regards,\n" +
						 "Restoran");


						_emailSender.SendEmail(employeeMessage);
						return response;
					}
					else
					{
						response.Success = false;
						response.Message = "Your Otp has been expired , Please try again";
					}
				}
				else
				{
					response.Success = false;
					response.Message = "Invalid OTP , Please try again";
				}

			}
			else
			{
				response.Success = false;
				response.Message = "Invalid Email , Please try again";
			}

			return response;
		}

		public async Task<ServiceResponse<string>> ResetPassword(PasswordChangeDto passswordChangeDto)
		{
			var serviceResponse = new ServiceResponse<string>();

			var userExist = await _userManager.FindByEmailAsync (passswordChangeDto.email);
			if (userExist == null)
			{
				serviceResponse.Success = false;
				serviceResponse.Message = "No User Exist With the Email";
				return serviceResponse;
			}
			var token = await _userManager.GeneratePasswordResetTokenAsync(userExist);

			// Reset the user's password using the token and new password
			var resetResult = await _userManager.ResetPasswordAsync(userExist, token, passswordChangeDto.password);

			if (!resetResult.Succeeded)
			{
				// Handle password reset failure
				serviceResponse.Success = false;
				serviceResponse.Message = "Failed to reset password.";
			}
			else
			{
				serviceResponse.Success = true;
				serviceResponse.Message = "Password reset successfully.";
			}

			return serviceResponse;






		}


		//public async Task<ServiceResponse<string>> Verify(string email, string otp)
		//{
		//	ServiceResponse<string> response = new ServiceResponse<string>();

		//	if (!IsEmailValid(email))
		//	{
		//		response.Success = false;
		//		response.Message = "Invalid email address.";
		//		return response;
		//	}
		//	var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());
		//	if (user != null)
		//	{
		//		if (user.OtpResendCount >= 3)
		//		{
		//			response.Success = false;
		//			response.Message = "Maximum OTP resend limit reached.";
		//			return response;
		//		}
		//		if (user.Otp == otp)
		//		{

		//			if (user.OtpExpiration > DateTimeOffset.UtcNow)
		//			{
		//				user.Otp = null;
		//				user.IsVerified = true;
		//				await _dbContext.SaveChangesAsync();
		//				response.Success = true;
		//				response.Message = "OTP verification successful.";
		//				return response;
		//			}
		//			else
		//			{
		//				response.Success = false;
		//				response.Message = "Your Otp has been expired , Please try again";
		//			}
		//		}
		//		else
		//		{
		//			response.Success = false;
		//			response.Message = "Invalid OTP , Please try again";

		//		}

		//	}
		//	else
		//	{

		//		response.Success = false;
		//		response.Message = "Invalid Email , Please try again";

		//	}

		//	return response;
		//}

		private bool IsEmailValid(string email)
		{
			// Regular expression pattern for email validation
			string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

			// Check if the email matches the pattern
			bool isValid = Regex.IsMatch(email, pattern);

			return isValid;
		}



	}
}

