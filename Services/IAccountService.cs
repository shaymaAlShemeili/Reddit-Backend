using Reddit.DTOs;

namespace Reddit.Services
{
    public interface IAccountService
    {
        Task<LoginResponse> AuthenticateAsync(UserLoginDTO dto);
        Task RegisterMemberAsync(UserRegisterDTO dto);
    }
}
