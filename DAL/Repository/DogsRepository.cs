using DAL.Context;
using DAL.Repository.Interfaces;
using Domain.Models;

namespace DAL.Repository;

public class DogsRepository : BaseRepository<Dog>
{
    public DogsRepository(DogsHouseDbContext context) : base(context)
    {

    }
}