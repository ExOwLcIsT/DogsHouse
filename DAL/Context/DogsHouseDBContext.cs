using Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace DAL.Context
{
    public class DogsHouseDBContext : DbContext
    {
        public DbSet<Dog> Dogs { get; set; }
        public DogsHouseDBContext(DbContextOptions<DogsHouseDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}