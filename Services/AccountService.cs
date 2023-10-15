using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Reddit.Models;
using Reddit.DTOs;
using System.Text;

namespace Reddit.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ForumUser> _userMgr;
        private readonly SignInManager<ForumUser> _signInMgr;
        private readonly IConfiguration _settings;

        public AccountService(UserManager<ForumUser> userManager, 
                                 SignInManager<ForumUser> signInManager, 
                                 IConfiguration settings)
        {
            _userMgr = userManager;
            _signInMgr = signInManager;
            _settings = settings;
        }

        public async Task<LoginResponse> AuthenticateAsync(UserLoginDTO credentials)
        {
            var appUser = await _userMgr.FindByNameAsync(credentials.UserName);
            
            if (appUser == null)
            {
                throw new System.Exception("Invalid username or password");
            }

            var authResult = await _signInMgr.CheckPasswordSignInAsync(appUser, credentials.Password, false);
            
            if (!authResult.Succeeded)
            {
                throw new System.Exception("Invalid username or password");
            }

            var jwtHandler = new JwtSecurityTokenHandler();
            var encryptionKey = Encoding.ASCII.GetBytes(_settings.GetValue<string>("Jwt:SecretKey"));
            var tokenDetails = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, credentials.UserName) }),
                Expires = System.DateTime.UtcNow.AddDays(5), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encryptionKey), SecurityAlgorithms.HmacSha256)
            };
            
            var jwtToken = jwtHandler.CreateToken(tokenDetails);
            
            return new LoginResponse 
            {
                AuthenticationToken = jwtHandler.WriteToken(jwtToken),
                Username = credentials.UserName
            };
        }

        public async Task RegisterMemberAsync(UserRegisterDTO memberInfo)
        {
            var isExistingUser = await _userMgr.FindByNameAsync(memberInfo.UserName);
            
            if (isExistingUser != null)
            {
                throw new System.Exception("Username is already taken");
            }

            var isEmailExisting = await _userMgr.FindByEmailAsync(memberInfo.Email);
            if (isEmailExisting != null)
            {
                throw new System.Exception("Email is already in use");
            }

            var newUser = new ForumUser
            {
                UserName = memberInfo.UserName,
                Email = memberInfo.Email
            };

            var creationResult = await _userMgr.CreateAsync(newUser, memberInfo.Password);
            
            if (!creationResult.Succeeded)
            {
                // Check the errors to send a more detailed message
                foreach (var error in creationResult.Errors)
                {
                    if (error.Code == "PasswordRequiresNonAlphanumeric" 
                        || error.Code == "PasswordRequiresDigit"
                        || error.Code == "PasswordRequiresUpper")
                    {
                        throw new System.Exception("Password must contain alphanumeric and special characters");
                    }
                }

                // General error if none of the specific conditions matched
                throw new System.Exception("Failed to create new account");
            }
        }

    }
}