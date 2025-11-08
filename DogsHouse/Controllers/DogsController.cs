using System.Linq;
using DAL.Context;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace DogsHouse.Controllers;

[ApiController]
[Route("")]
public class DogsController : ControllerBase
{

    private readonly ILogger<DogsController> _logger;

    private readonly DogsHouseDBContext _context;

    public DogsController(ILogger<DogsController> logger, DogsHouseDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]

    [Route("/dogs")]
    public async Task<IActionResult> Dogs([FromQuery] string? attribute, [FromQuery] string? order, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = _context.Dogs.AsQueryable();

        //sorting

        if (!string.IsNullOrWhiteSpace(attribute))
        {
            switch (attribute)
            {
                case "name":
                    {
                        result = order == "desc" ? result.OrderByDescending((d) => d.name) : result.OrderBy((d) => d.name);
                        break;
                    }
                case "color":
                    {
                        result = order == "desc" ? result.OrderByDescending((d) => d.color) : result.OrderBy((d) => d.color);
                        break;
                    }
                case "tail_length":
                    {
                        result = order == "desc" ? result.OrderByDescending((d) => d.tail_length) : result.OrderBy((d) => d.tail_length);
                        break;
                    }
                case "weight":
                    {
                        result = order == "desc" ? result.OrderByDescending((d) => d.weight) : result.OrderBy((d) => d.weight);
                        break;
                    }
            }
        }

        //Paginating

        result = result.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        return Ok(result);
    }

    [HttpPost]
    [Route("/dog")]
    public async Task<IActionResult> CreateDog([FromBody] Dog dog)
    {
        await _context.AddAsync(dog);
        await _context.SaveChangesAsync();
        return StatusCode(201);
    }

}
