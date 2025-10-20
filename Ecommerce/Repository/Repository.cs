
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ecommerce.Repository
{
    
    public class Repository<T> where T : class
    {
        ApplicationDBContext _context = new ApplicationDBContext();
        private DbSet<T> _db;
        public Repository()
        {
            _db = _context.Set<T>();
        }
        public async Task AddAsync(T entity,CancellationToken cancellationToken=default)
        { 
          await _db.AddAsync(entity,cancellationToken);

        }
        public void Update(T entity)
        {
            _db.Update(entity);
        }
        public void Delete(T entity)
        {
            _db.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T,bool>>? expression=null,
            Expression<Func<T, object>>[]? includes=null,
            bool tracked = true,
            CancellationToken cancellationToken=default)
        {
            var entities= _db.AsQueryable();
            if(expression!=null)
            {
                entities= entities.Where(expression);
            }

            if(includes is not null)
            {
                foreach (var include in includes)
                {
                    entities= entities.Include(include);
                }
            }

            if(!tracked)
            {
                entities= entities.AsNoTracking();
            }
            return await entities.ToListAsync(cancellationToken);
        }
        public async Task<T> GetoneAsync(
            Expression<Func<T, bool>>? expression,
            Expression<Func<T, object>>[]? includes = null,
            bool tracked = true,
            CancellationToken cancellationToken = default)
        {
            return (await GetAllAsync(expression,includes,tracked, cancellationToken)).FirstOrDefault();

        }

        public async Task commitASync(CancellationToken cancellationToken)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine("eroo"+ex.Message);
            }

        }
    }
}
