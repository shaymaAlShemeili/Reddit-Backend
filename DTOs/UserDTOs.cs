namespace Reddit.DTOs
{
    public class UserLoginDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UserRegisterDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
