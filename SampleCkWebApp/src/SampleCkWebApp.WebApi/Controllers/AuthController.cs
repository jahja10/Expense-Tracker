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

    public AuthController(
        LogInUser logInUser,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _logInUser = logInUser;
        _jwtTokenGenerator = jwtTokenGenerator;
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
}
