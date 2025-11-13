using DAL.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository.Interfaces
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private DbSet<TEntity> _entities;
        protected DbSet<TEntity> Entities => this._entities ??= _context.Set<TEntity>();
        protected DogsHouseDbContext _context;
        public BaseRepository(DogsHouseDbContext context) => _context = context;
        public async Task CreateAsync(TEntity entity) => await this.Entities.AddAsync(entity).ConfigureAwait(false);
        public virtual async Task<IReadOnlyCollection<TEntity>> GetAllAsync() => await this.Entities.ToListAsync().ConfigureAwait(false);
        public virtual async Task SaveChangesAsync()
        {
            try
            {
                await this._context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {

            }
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression) => await this.Entities.AnyAsync(expression);
    }
}
