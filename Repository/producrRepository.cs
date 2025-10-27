using Ecommerce.Repository.IRepository;
using System.Threading.Tasks;

namespace Ecommerce.Repository
{
    public class producrRepository : Repository<Products> , productIRepositry
    {
        private ApplicationDBContext _context;// = new();

        public producrRepository(ApplicationDBContext context): base(context) 
        {
            _context = context;
        }

        public async Task AddrangeAsync(
            IEnumerable<Products> products,
            CancellationToken cancellationToken= default
            )
        {
            await _context.AddRangeAsync(products, cancellationToken);
        }
    }
}
