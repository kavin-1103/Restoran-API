//using Microsoft.AspNetCore.Identity;
//using Microsoft.IdentityModel.Tokens;
//using Restaurant_Reservation_Management_System_Api.Model;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace Restaurant_Reservation_Management_System_Api.Repository
//{
//    public class TokenRepository : ITokenRepository
//    {
//        private readonly IConfiguration _configuration;
//        private readonly UserManager<ApplicationUser> _userManager;

//        public TokenRepository(IConfiguration configuration , UserManager<ApplicationUser> userManager)

//        {
//            _configuration = configuration;
//            _userManager = userManager;
//        }


//        public async Task<string> CreateJwtToken(ApplicationUser user)
//        {
//            //Create Claims

//            var claims = new List<Claim>
//            {

//                new Claim(JwtRegisteredClaimNames.NameId , user.Id.ToString()),
//                new Claim(JwtRegisteredClaimNames.UniqueName ,user.UserName ),

//                new Claim(ClaimTypes.Email, user.Email)

//            };

//            var roles =await _userManager.GetRolesAsync(user);

//            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

//            claims.AddRange(roles.Select(role => new Claim("Roles", role)));

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

//            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            //var tokenDescriptor = new SecurityTokenDescriptor
//            //{
//            //    Subject = new ClaimsIdentity(claims),
//            //    Expires = DateTime.Now.AddDays(7),
//            //    SigningCredentials = credentials
//            //};

//            var token = new JwtSecurityToken(
//                issuer: _configuration["Jwt:Issuer"],
//                audience: _configuration["Jwt:Audience"],
//                claims: claims,
//                expires: DateTime.Now.AddHours(1),
//                signingCredentials: credentials
//                );
//            //var tokenHandler = new JwtSecurityTokenHandler();
//            //var token = tokenHandler.CreateToken(tokenDescriptor);

//            //return tokenHandler.WriteToken(token);


//            return new JwtSecurityTokenHandler().WriteToken(token);

//            //Return Token


//        }
//        //public string CreateJwtToken(ApplicationUser user, List<string> roles)
//        //{ 
//        //    //Create Claims

//        //    var claims = new List<Claim>
//        //    {

//        //        new Claim(JwtRegisteredClaimNames.NameId , user.Id.ToString()),
//        //        new Claim(JwtRegisteredClaimNames.UniqueName ,user.UserName ),

//        //        new Claim(ClaimTypes.Email, user.Email)

//        //    };
//        //    foreach(var role in roles)
//        //    {
//        //        claims.Add(new Claim(ClaimTypes.Role, role));
//        //    }


//        //    //claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

//        //    //Jwt Security Token Parameters

//        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

//        //    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//        //    var token = new JwtSecurityToken(
//        //        issuer: _configuration["Jwt:Issuer"],
//        //        audience: _configuration["Jwt:Audience"],
//        //        claims: claims,
//        //        expires: DateTime.Now.AddMinutes(15),
//        //        signingCredentials: credentials
//        //        );

//        //    //Return Token

//        //    return new JwtSecurityTokenHandler().WriteToken(token);
//        //}

//        //public string CreateJwtToken(List<Claim> claims)
//        //{
//        //    var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

//        //    var tokenObject = new JwtSecurityToken(
//        //            issuer: _configuration["JWT:Issuer"],
//        //            audience: _configuration["JWT:Audience"],
//        //            expires: DateTime.Now.AddHours(1),
//        //            claims: claims,
//        //            signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
//        //        );

//        //    string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

//        //    return token;
//        //}

//    }
//}

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Restaurant_Reservation_Management_System_Api.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Restaurant_Reservation_Management_System_Api.Repository
{
	public class TokenRepository : ITokenRepository
	{
		private readonly IConfiguration _configuration;
		private readonly UserManager<ApplicationUser> _userManager;

		public TokenRepository(IConfiguration configuration, UserManager<ApplicationUser> userManager)

		{
			_configuration = configuration;
			_userManager = userManager;
		}


		public async Task<string> CreateJwtToken(ApplicationUser user)
		{
			//Create Claims

			var claims = new List<Claim>
			{

				new Claim(JwtRegisteredClaimNames.NameId , user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.UniqueName ,user.UserName ),

				new Claim(ClaimTypes.Email, user.Email)

			};

			var roles = await _userManager.GetRolesAsync(user);

			claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

			claims.AddRange(roles.Select(role => new Claim("Roles", role)));



			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			//var tokenDescriptor = new SecurityTokenDescriptor
			//{
			//    Subject = new ClaimsIdentity(claims),
			//    Expires = DateTime.Now.AddDays(7),
			//    SigningCredentials = credentials
			//};

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.Now.AddHours(1),
				signingCredentials: credentials
				);
			//var tokenHandler = new JwtSecurityTokenHandler();
			//var token = tokenHandler.CreateToken(tokenDescriptor);

			//return tokenHandler.WriteToken(token);


			return new JwtSecurityTokenHandler().WriteToken(token);

			//Return Token


		}
		//public string CreateJwtToken(ApplicationUser user, List<string> roles)
		//{ 
		//    //Create Claims

		//    var claims = new List<Claim>
		//    {

		//        new Claim(JwtRegisteredClaimNames.NameId , user.Id.ToString()),
		//        new Claim(JwtRegisteredClaimNames.UniqueName ,user.UserName ),

		//        new Claim(ClaimTypes.Email, user.Email)

		//    };
		//    foreach(var role in roles)
		//    {
		//        claims.Add(new Claim(ClaimTypes.Role, role));
		//    }


		//    //claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

		//    //Jwt Security Token Parameters

		//    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

		//    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		//    var token = new JwtSecurityToken(
		//        issuer: _configuration["Jwt:Issuer"],
		//        audience: _configuration["Jwt:Audience"],
		//        claims: claims,
		//        expires: DateTime.Now.AddMinutes(15),
		//        signingCredentials: credentials
		//        );

		//    //Return Token

		//    return new JwtSecurityTokenHandler().WriteToken(token);
		//}

		//public string CreateJwtToken(List<Claim> claims)
		//{
		//    var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

		//    var tokenObject = new JwtSecurityToken(
		//            issuer: _configuration["JWT:Issuer"],
		//            audience: _configuration["JWT:Audience"],
		//            expires: DateTime.Now.AddHours(1),
		//            claims: claims,
		//            signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
		//        );

		//    string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

		//    return token;
		//}

	}
}