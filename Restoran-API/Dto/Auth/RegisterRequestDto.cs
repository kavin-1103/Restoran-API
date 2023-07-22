using AutoMapper.Configuration.Conventions;

namespace Restaurant_Reservation_Management_System_Api.Dto.Auth
{
    public class RegisterRequestDto
    {

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; } 

        public string Password { get; set; }    



    }
}
