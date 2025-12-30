using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using SampleCkWebApp.Users;
using SampleCkWebApp.Application.Users.Interfaces.Application;
using SampleCkWebApp.Application.Users.Mappings;

namespace SampleCkWebApp.WebApi.Controllers.Users;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class UsersController : ApiControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // ------------------------------------------------------------------------
    // GET /Users
    // ------------------------------------------------------------------------
    [HttpGet]
    [ProducesResponseType(typeof(GetUsersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        // Pretpostavka: u service-u se metoda zove GetUsersAsync.
        // Ako se kod tebe zove drugačije (npr. GetUserAsync), uskladi ime.
        var result = await _userService.GetUsersAsync(cancellationToken);

        return result.Match(
            usersResult => Ok(usersResult.ToResponse()),
            Problem
        );
    }

    // ------------------------------------------------------------------------
    // GET /Users/{id}
    // ------------------------------------------------------------------------
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(
        [FromRoute, Required] int id,
        CancellationToken cancellationToken)
    {
        var result = await _userService.GetUserByIdAsync(id, cancellationToken);

        return result.Match(
            user => Ok(user.ToResponse()),
            Problem
        );
    }

    // ------------------------------------------------------------------------
    // POST /Users
    // ------------------------------------------------------------------------
    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser(
        [FromBody, Required] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.CreateUserAsync(
            request.Name,
            request.Email,
            request.Password,
            cancellationToken
        );

        return result.Match(
            user => CreatedAtAction(
                nameof(GetUserById),
                new { id = user.Id },
                user.ToResponse()
            ),
            Problem
        );
    }




   [HttpPut("{id:int}")]
   [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   [ProducesResponseType(StatusCodes.Status409Conflict)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public async Task<IActionResult> UpdateUser(

    [FromRoute, Required] int id,
    [FromBody, Required] UpdateUserRequest request,
    CancellationToken cancellationToken)

    {
        
        var result = await _userService.UpdateUserAsync(


            id,
            request.Name,
            request.Email,
            request.Password,
            cancellationToken


        );


        return result.Match(

            user=> Ok(user.ToResponse()),
            Problem

        );


    }


    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser(

        [FromRoute, Required] int id,
        CancellationToken cancellationToken)


    {
        

        var result = await _userService.DeleteUserAsync(id, cancellationToken);

        return result.Match(

            _ => NoContent(),
            Problem

        );


    }


    
   


   


}
