using DAL.Context;
using BLL.Interfaces;
using Domain.Models;
using DAL.Repository.Interfaces;
using DAL.Repository;
using Domain.Attributes;
using DAL.Repository.UnitOfWork;

namespace BLL.Services;

public class DogsService : IDogsService
{

    private readonly IUnitOfWork unitOfWork;

    public DogsService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }


    public async Task<IEnumerable<Dog>> GetSortedAndPaginatedDogs(string? attribute, string? order, int pageNumber, int pageSize)
    {
        var dogs = await unitOfWork.Dogs.GetAllAsync();

        //sorting



        if (!string.IsNullOrWhiteSpace(attribute) && Enum.TryParse<Attributes>(attribute, true, out var sortAttr))
        {
            dogs = (sortAttr switch
            {
                Attributes.name => (order == "desc" ? dogs.OrderByDescending((d) => d.name) : dogs.OrderBy((d) => d.name)).ToList(),

                Attributes.color => (order == "desc" ? dogs.OrderByDescending((d) => d.color) : dogs.OrderBy((d) => d.color)).ToList(),

                Attributes.tail_length => (order == "desc" ? dogs.OrderByDescending((d) => d.tail_length) : dogs.OrderBy((d) => d.tail_length)).ToList(),

                Attributes.weight => (order == "desc" ? dogs.OrderByDescending((d) => d.weight) : dogs.OrderBy((d) => d.weight)).ToList(),
                _ => dogs,
            });
        }

        //Paginating

        dogs = dogs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return dogs;
    }


    public async Task CreateDog(Dog dog)
    {
        if (await unitOfWork.Dogs.AnyAsync(d => d.name.Equals(dog.name)))
        {
            throw new ArgumentException("Dog`s name must be unique");
        }
        await unitOfWork.Dogs.CreateAsync(dog);
        await unitOfWork.Dogs.SaveChangesAsync();
    }
}