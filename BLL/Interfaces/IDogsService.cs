using DogsHouse.Models;

namespace DogsHouse.Interfaces;

public interface IDogsService
{
    public Task<IEnumerable<Dog>> GetSortedAndPaginatedDogs(string? attribute, string? order, int pageNumber, int pageSize);
    public Task CreateDog(Dog dog);
}