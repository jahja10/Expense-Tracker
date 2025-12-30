using Microsoft.AspNetCore.Mvc;

namespace SampleCkWebApp.WebApi.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)] 
public class ErrorController : ControllerBase
{
    [HttpGet("/error")]
    public IActionResult Error() => Problem();
}
