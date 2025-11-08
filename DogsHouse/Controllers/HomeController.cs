

using Microsoft.AspNetCore.Mvc;


namespace DogsHouse.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet("ping")]
    public async Task<IActionResult> Ping()
    {
        return Ok("Dogshouseservice.Version1.0.1");
    }

}