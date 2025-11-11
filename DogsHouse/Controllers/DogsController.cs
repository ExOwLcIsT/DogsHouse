using DogsHouse.Services;
using DogsHouse.Models;
using Microsoft.AspNetCore.Mvc;
using DogsHouse.Interfaces;

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

        //TODO
        //Validate passed data
        await dogsService.CreateDog(dog);
        return StatusCode(201);
    }
}
