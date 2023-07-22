//using Azure;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Identity.Client;
//using Restaurant_Reservation_Management_System_Api.Dto.Auth;
//using Restaurant_Reservation_Management_System_Api.Model;
//using Restaurant_Reservation_Management_System_Api.Repository;
//using Restaurant_Reservation_Management_System_Api.Services.Auth;
//using System.Security.Claims;
//using System.Net;
//using System.Net.Mail;
//using System;
////using System.Net;
////using System.Net.Mail;
////using System.Web.Http;
//using System.ComponentModel.DataAnnotations;
//using Microsoft.DotNet.Scaffolding.Shared.Messaging;
//using AutoMapper;
//using EmailService;
//using Microsoft.AspNetCore.WebUtilities;
//using Microsoft.EntityFrameworkCore;
//namespace Restaurant_Reservation_Management_System_Api.Controllers.AuthController
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly IAuthService _authService;
//        private readonly ITokenRepository _tokenRepository;

//		private readonly IMapper _mapper;
//		//private readonly TokenRepository _jwtHandler;
//		//private readonly IEmailSender _emailSender;



//		public AuthController(IAuthService authService, ITokenRepository tokenRepository, UserManager<ApplicationUser> userManager, IMapper mapper, TokenRepository jwtHandler, IEmailSender emailSender)
//        {
//            _authService = authService;
//            _tokenRepository = tokenRepository;
//            _userManager = userManager;
//			_mapper = mapper;
//			//_jwtHandler = jwtHandler;
//			//_emailSender = emailSender;
//			//_authRepository = authRepository;
//		}

//        [HttpPost("register")]

//        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
//        {


//            if (ModelState.IsValid)
//            {

//                var result = await _authService.RegisterUserAsync(request);

//                if (result.Succeeded)
//                {
//                    return Ok();
//                }

//                foreach (var error in result.Errors)
//                {
//                    ModelState.AddModelError("", error.Description);
//                }
//            }

//            return ValidationProblem(ModelState);


//        }

//        [HttpPost]
//        [Route("login")]

//        public async Task<ActionResult<ServiceResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto loginRequestDto)
//        {
//            //var result = await _authService.LoginUserAsync(loginRequestDto);
//            //Check Email
//            var identityUser = await _userManager.FindByEmailAsync(loginRequestDto.Email);

//            var response = new ServiceResponse<LoginResponseDto>();


//            if (identityUser == null)
//            {
//                response.Success = false;
//                response.Message = "User not found with the provided email address.";

//                return NotFound(response);

//            }

//            //Check Password

//            var checkPasswordResult = await _userManager.CheckPasswordAsync(identityUser, loginRequestDto.Password);


//            if (!checkPasswordResult)
//            {
//                response.Success = false;
//                response.Message = "Incorrect Password";
//                return NotFound(response);
//            }

//            if (checkPasswordResult && identityUser is not null)

//            {

//                var jwtToken = await _tokenRepository.CreateJwtToken(identityUser);

//                var loginResponseDto = new LoginResponseDto()
//                {
//                    CustomerId = identityUser.Id,
//                    Email = loginRequestDto.Email,
//                    Token = jwtToken,
//                    Roles = await _userManager.GetRolesAsync(identityUser),

//                };
//                response.Data = loginResponseDto;
//                response.Success = true;
//                response.Message = "User Logged In Successfully!";

//                return Ok(response);


//            }
//            else
//            {
//                response.Success = false;
//                return Unauthorized();

//            }



//            ModelState.AddModelError("", "Email Or Password incorrect");

//            return ValidationProblem(ModelState);

//        }


//        //[HttpPost("EmailExists")]
//        //public async Task<ActionResult<ServiceResponse<string>>> CheckEmailExists(string email)
//        //{
//        //    // Perform the logic to check if the email exists in the database
//        //    var identityUser = await _userManager.FindByEmailAsync(email);
//        //    var response = new ServiceResponse<string>();

//        //    if (identityUser == null)
//        //    {
//        //        response.Success = false;
//        //        response.Message = "User not found with the provided email address.";

//        //        return NotFound(response);

//        //    }
//        //    response.Data = "Email exists";
//        //    response.Success = true;
//        //    response.Message = "Email exists.";
//        //    return Ok(response);

//        //}

//		//	[HttpPost]
//		//	[Route("send-verification-link")]
//		//	public Task<ActionResult<string>> SendVerificationLink([FromBody] EmailModel emailModel)
//		//	{
//		//		try
//		//		{
//		//			// Generate OTP
//		//			var otp = GenerateOTP();

//		//			// Compose email message
//		//			var fromEmail = "your-email@example.com";
//		//			var toEmail = emailModel.Email;
//		//			var subject = "Verification Link";
//		//			var body = $"Your verification OTP: {otp}";

//		//			// Configure SMTP settings
//		//			var smtpServer = "your-smtp-server";
//		//			var smtpPort = 587;
//		//			var smtpUsername = "your-email-username";
//		//			var smtpPassword = "your-email-password";

//		//			using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
//		//			{
//		//				smtpClient.EnableSsl = true;
//		//				smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

//		//				// Send email
//		//				using (var mailMessage = new MailMessage(fromEmail, toEmail, subject, body))
//		//				{
//		//					smtpClient.Send(mailMessage);
//		//				}
//		//			}

//		//			return Ok("Verification link sent successfully");
//		//		}
//		//		catch (Exception ex)
//		//		{
//		//			return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
//		//		
//		//	}

//		//	private string GenerateOTP()
//		//	{
//		//		// Implement your OTP generation logic here
//		//		// For example, you can use a random number generator or a library like "Google Authenticator" for OTP generation
//		//		// Return the generated OTP as a string
//		//		return "123456";
//		//	}
//		//}
//		//public class EmailModel
//		//{
//		//	public string Email { get; set; }
//		//}


//		//   public async Task<IActionResult> ForgotPassword([Required] string email)
//		//   {
//		//       var user = await _userManager.FindByEmailAsync(email);
//		//       if (user != null)
//		//       {
//		//           var token = await _userManager.GenerateOtp
//		//           var forgotPasswordLink = Url.Action(n, "Auth", new { token, email = user.Email }, Request.Scheme);

//		//           var message = new Message(new string[] { user.Email! }, "Forgot Password email Link", forgotPasswordLink!);

//		//           _emailService.SendEmail(message);

//		//           return StatusCode(StatusCodes.Status200OK,
//		//new Response { Status = "Success", Message = $"Password Change request is sent on Email {user.Email}. Please Open your email & click the link" });
//		//       }

//		//       return StatusCode(StatusCode.Status400BadRequest,
//		//            new Response { Status = "Error", Message = $"Could not send mail , Please try agian" });

//		//   }

//		//[HttpPost("Verify")]
//		//[AllowAnonymous]
//		//public async Task<ActionResult<ServiceResponse<string>>> Verify(string email, string otp)
//		//{
//		//	var response = await _authRepository.Verify(
//		//		email, otp
//		//		);
//		//	if (!response.Success)
//		//	{
//		//		return BadRequest(response);
//		//	}
//		//	return Ok(response);

//		//}

//		//[HttpPost("ForgotPassword")]
//		//[AllowAnonymous]

//		//public async Task<ActionResult<ServiceResponse<string>>> ForgotPassword(string email)
//		//{
//		//	var response = await _authRepository.ForgotPassword(email);
//		//	if (!response.Success)
//		//	{
//		//		return BadRequest(response);
//		//	}
//		//	return Ok(response);
//		//}


//		//[HttpPost("ResetPassword")]
//		//[AllowAnonymous]

//		//public async Task<ActionResult<ServiceResponse<string>>> ResetPassword(ResetPasswordDto request)
//		//{
//		//	var response = await _authRepository.ResetPassword(request);
//		//	if (!response.Success)
//		//	{
//		//		return BadRequest(response);
//		//	}
//		//	return Ok(response);
//		//}

//	}
//}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Restaurant_Reservation_Management_System_Api.Dto.Auth;
using Restaurant_Reservation_Management_System_Api.Model;
using Restaurant_Reservation_Management_System_Api.Repository;
using Restaurant_Reservation_Management_System_Api.Services.Auth;
using System.Security.Claims;

namespace Restaurant_Reservation_Management_System_Api.Controllers.AuthController
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;

		private readonly IAuthService _authService;
		private readonly ITokenRepository _tokenRepository;

		public AuthController(IAuthService authService, ITokenRepository tokenRepository, UserManager<ApplicationUser> userManager)
		{
			_authService = authService;
			_tokenRepository = tokenRepository;
			_userManager = userManager;
		}

		[HttpPost("register")]

		public async Task<ActionResult<ServiceResponse<string>>> Register([FromBody] RegisterRequestDto request)
		{
			ServiceResponse<string> serviceResponse;

			if (ModelState.IsValid)
			{

				var response = await _authService.RegisterUserAsync(request);

				if (response.Success)
				{
					return Ok(response);
				}

				response.Success = false;
				return BadRequest(response);
			}

			return ValidationProblem(ModelState);


		}

		[HttpPost]
		[Route("login")]

		public async Task<ActionResult<ServiceResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto loginRequestDto)
		{
			//var result = await _authService.LoginUserAsync(loginRequestDto);
			//Check Email
			var identityUser = await _userManager.FindByEmailAsync(loginRequestDto.Email);

			var response = new ServiceResponse<LoginResponseDto>();


			if (identityUser == null)
			{
				response.Success = false;
				response.Message = "User not found with the provided email address.";

				return NotFound(response);

			}

			//Check Password

			var checkPasswordResult = await _userManager.CheckPasswordAsync(identityUser, loginRequestDto.Password);


			if (!checkPasswordResult)
			{
				response.Success = false;
				response.Message = "Incorrect Password";
				return NotFound(response);
			}

			if (checkPasswordResult && identityUser is not null)

			{

				var jwtToken = await _tokenRepository.CreateJwtToken(identityUser);

				var loginResponseDto = new LoginResponseDto()
				{
					CustomerId = identityUser.Id,
					Email = loginRequestDto.Email,
					Token = jwtToken,
					Roles = await _userManager.GetRolesAsync(identityUser),

				};
				response.Data = loginResponseDto;
				response.Success = true;
				response.Message = "User Logged In Successfully!";

				return Ok(response);


			}
			else
			{
				response.Success = false;
				return Unauthorized();

			}



			ModelState.AddModelError("", "Email Or Password incorrect");

			return ValidationProblem(ModelState);

		}


		[HttpPost("ForgotPassword")]
		[AllowAnonymous]

		public async Task<ActionResult<ServiceResponse<string>>> ForgotPassword(string email)
		{
			var response = await _authService.ForgotPassword(email);
			if (!response.Success)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}

		[HttpPost("EmailVerification")]
		[AllowAnonymous]
		public async Task<ActionResult<ServiceResponse<string>>> EmailVerfication(EmailVerificationDto emailVerificationDto)
		{
			var response = await _authService.EmailVerification(emailVerificationDto);
			if (!response.Success)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}


		[HttpPost("Verify")]
		[AllowAnonymous]
		public async Task<ActionResult<ServiceResponse<string>>> Verify(VerifyOtpDto verifyOtpDto)
		{
			var response = await _authService.Verify(verifyOtpDto);
			if (!response.Success)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}

		[HttpPost("ResetPassword")]
		public async Task<ActionResult<ServiceResponse<string>>> ResetPassword(PasswordChangeDto passwordChangeDto)
		{
			var response =await  _authService.ResetPassword(passwordChangeDto);

			if (response.Success == false)
			{
				return BadRequest(response);
			}
			return Ok(response);	
		}

	}
}