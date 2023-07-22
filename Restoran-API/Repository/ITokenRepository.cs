using Restaurant_Reservation_Management_System_Api.Model;
using System.Security.Claims;

namespace Restaurant_Reservation_Management_System_Api.Repository
{
	public interface ITokenRepository
	{
		// string CreateJwtToken(ApplicationUser user, List<string> roles);

		// string CreateJwtToken(List<Claim> claims);

		Task<string> CreateJwtToken(ApplicationUser user);

	}
}