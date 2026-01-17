using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleCkWebApp.WebApi.Controllers;

[ApiController]
[Route("test")]
[Produces("application/json")]
public class TestController : ControllerBase
{
    [Authorize]
    [HttpGet("secure")]
    public IActionResult Secure() => Ok("OK - Authorized");
}
