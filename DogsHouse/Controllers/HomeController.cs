

using Microsoft.AspNetCore.Mvc;


namespace DogsHouse.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{

    public HomeController()
    {
    }

    [HttpGet("ping")]
    public async Task<IActionResult> Ping()
    {
        return Ok("Dogshouseservice.Version1.0.1");
    }

}