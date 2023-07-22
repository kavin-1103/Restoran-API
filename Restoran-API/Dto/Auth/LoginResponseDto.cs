namespace Restaurant_Reservation_Management_System_Api.Dto.Auth
{
    public class LoginResponseDto
    { 
        public string CustomerId { get; set; }  

        public string Email { get; set; }

        public string Token { get; set; }   

        public IEnumerable<string> Roles { get; set; } 

        

    }
}
