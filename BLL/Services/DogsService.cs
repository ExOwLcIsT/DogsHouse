using DogsHouse.Context;
using DogsHouse.Interfaces;
using DogsHouse.Models;

namespace DogsHouse.Services;

public class DogsService : IDogsService
{
    private readonly DogsHouseDbContext _context;

    public DogsService(DogsHouseDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Dog>> GetSortedAndPaginatedDogs(string? attribute, string? order, int pageNumber, int pageSize)
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

        return result;
    }


    public async Task CreateDog(Dog dog)
    {
        await _context.AddAsync(dog);
        await _context.SaveChangesAsync();
    }
}