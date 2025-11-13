using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;

namespace DogsHouse.Controllers;

[ApiController]
[Route("")]
public class DogsController : ControllerBase
{
    private readonly IDogsService dogsService;

    public DogsController(IDogsService dogsService)
    {
        this.dogsService = dogsService;
    }

    [HttpGet]

    [Route("/dogs")]
    public async Task<IActionResult> Dogs([FromQuery] string? attribute = null,
     [FromQuery] string? order = null,
      [FromQuery] int pageNumber = 1,
       [FromQuery] int pageSize = 10)
    {
        var result = await dogsService.GetSortedAndPaginatedDogs(attribute, order, pageNumber, pageSize);
        return Ok(result);
    }

    [HttpPost]
    [Route("/dog")]
    public async Task<IActionResult> CreateDog([FromBody] Dog dog)
    {

        try
        {
            await dogsService.CreateDog(dog);
            return StatusCode(201);
        }
        catch (ArgumentException ae)
        {
            return BadRequest(new { message = ae.Message });
        }
    }
}
