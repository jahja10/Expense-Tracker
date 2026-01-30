using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleCkWebApp.Application.Users.Interfaces.Application;
using SampleCkWebApp.Application.Users.Interfaces.Infrastructure;
using SampleCkWebApp.Application.Users.Mappings;
using SampleCkWebApp.Users;

namespace SampleCkWebApp.WebApi.Controllers.Me;

[Authorize(Roles = "User,Admin")]
[ApiController]
[Route("me")]
[Produces("application/json")]
public class MeController : ApiControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserRepository _users;


    public MeController(IUserService userService, IUserRepository users)
    {
        _userService = userService;
        _users = users;
    }

    [HttpGet]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var result = await _userService.GetUserByIdAsync(CurrentUserId, cancellationToken);

        return result.Match(
            user => Ok(user.ToResponse()),
            Problem
        );
    }

    [HttpPatch("password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangeMyPassword(
        [FromBody] ChangeMyPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.ChangeMyPasswordAsync(
            CurrentUserId,
            request.CurrentPassword,
            request.NewPassword,
            request.ConfirmNewPassword,
            cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem
        );
    }

    [HttpPost("deactivate")]
    public async Task<IActionResult> DeactivateMe(CancellationToken cancellationToken)
    {
        var result = await _users.DeactivateUserAsync(CurrentUserId, cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem
        );
    }

    
}
