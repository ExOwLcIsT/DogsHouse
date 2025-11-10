using DogsHouse.Models;
using Microsoft.EntityFrameworkCore;


namespace DogsHouse.Context
{
    public class DogsHouseDbContext : DbContext
    {
        public DbSet<Dog> Dogs { get; set; }
        public DogsHouseDbContext(DbContextOptions<DogsHouseDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}