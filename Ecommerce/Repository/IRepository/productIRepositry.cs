using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repository.IRepository
{
    public interface productIRepositry : IRepository<Products>
    {
         Task AddrangeAsync(
          IEnumerable<Products> products,
          CancellationToken cancellationToken = default
          );
      
    }
}
