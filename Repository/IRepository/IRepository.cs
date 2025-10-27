using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecommerce.Repository.IRepository
{
    public interface  IRepository<T>where T : class 
    {

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        void Update(T entity);

        void Delete(T entity);


        Task<IEnumerable<T>> GetAllAsync(
           Expression<Func<T, bool>>? expression = null,
           Expression<Func<T, object>>[]? includes = null,
           bool tracked = true,
           CancellationToken cancellationToken = default);

        Task<T> GetoneAsync(
           Expression<Func<T, bool>>? expression,
           Expression<Func<T, object>>[]? includes = null,
           bool tracked = true,
           CancellationToken cancellationToken = default);


        Task commitASync(CancellationToken cancellationToken);
    }
}

