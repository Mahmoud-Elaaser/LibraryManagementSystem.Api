namespace LibraryManagementSystem.Application.DTOs.Auth
{
    public class TokenDto
    {
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
    }
}
