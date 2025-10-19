
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ecommerce.Repository
{
    
    public class CategoryRepository
    {
        ApplicationDBContext _context = new ApplicationDBContext();
        public async Task Add(Categores categores,CancellationToken cancellationToken)
        { 
          await  _context.Categores.AddAsync(categores);

        }
        public void Update(Categores categores)
        {
            _context.Categores.Update(categores);
        }
        public void Delete(Categores categores)
        {
            _context.Categores.Remove(categores);
        }

        public async Task<IEnumerable<Categores>> GetAllAsync(
            Expression<Func<Categores,bool>>? expression,
            CancellationToken cancellationToken=default)
        {
            var categories= _context.Categores.AsQueryable();
            if(expression!=null)
            {
                categories= categories.Where(expression);
            }
            return await categories.ToListAsync(cancellationToken);
        }
        public async Task<Categores> GetoneAsync(
            Expression<Func<Categores, bool>>? expression,
            CancellationToken cancellationToken = default)
        {
            return (await GetAllAsync(expression,cancellationToken)).FirstOrDefault();

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
