namespace Reddit.DTOs
{   
    public class LoginResponse
    {
        public string AuthenticationToken { get; set; }
        public string Username { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

}