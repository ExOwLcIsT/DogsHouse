using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;

namespace DAL.Repository.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<IReadOnlyCollection<TEntity>> GetAllAsync();
        Task CreateAsync(TEntity entity);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);
    }
}
