using Microsoft.AspNetCore.Mvc;
using Reddit.DTOs;
using Reddit.Services;
using Reddit.Exceptions;

[ApiController]
[Route("api/auth")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(UserLoginDTO dto)
    {
        try 
        {
            var authToken = await _accountService.AuthenticateAsync(dto);
            return Ok(authToken);

        }
        catch(Exception ex) 
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(UserRegisterDTO dto)
    {
        try 
        {
            await _accountService.RegisterMemberAsync(dto);
            return Ok("Account created successfully");
        }
        catch(System.Exception ex) 
        {
            return BadRequest(new { message = ex.Message } );

        }
    }

}
