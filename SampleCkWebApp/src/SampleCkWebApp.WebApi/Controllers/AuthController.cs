using Microsoft.AspNetCore.Mvc;
using SampleCkWebApp.Contracts.Auth;
using SampleCkWebApp.Application.Users.Auth;
using SampleCkWebApp.Application.Users.Interfaces;

namespace SampleCkWebApp.WebApi.Controllers;

[ApiController]
[Route("auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly LogInUser _logInUser;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    private readonly RegisterUser _registerUser;

    public AuthController(
        LogInUser logInUser,
        IJwtTokenGenerator jwtTokenGenerator, RegisterUser registerUser)
    {
        _logInUser = logInUser;
        _jwtTokenGenerator = jwtTokenGenerator;
        _registerUser = registerUser;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var loginResult = await _logInUser.HandleAsync(
            request.Email,
            request.Password,
            cancellationToken);

        if (loginResult.IsError)
        {
            return Unauthorized();
        }

        var user = loginResult.Value;

        var token = _jwtTokenGenerator.GenerateToken(
            user.Id,
            user.Email,
            user.Role);

        return Ok(new LoginResponse
        {
            UserId = user.Id,
            Email = user.Email,
            Token = token
        });
    }
    
      [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var registerResult = await _registerUser.HandleAsync(
            request.Name,
            request.Email,
            request.Password,
            cancellationToken);

        if (registerResult.IsError)
        {
            // možeš i Conflict ako je DuplicateEmail, ali ovo je najbrže:
            return BadRequest(registerResult.Errors);
        }

        var user = registerResult.Value;

        var token = _jwtTokenGenerator.GenerateToken(
            user.Id,
            user.Email,
            user.Role);

        return Ok(new RegisterResponse
        {
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role,
            Token = token
        });
    }

}
