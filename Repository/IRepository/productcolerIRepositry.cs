using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repository.IRepository
{
    public interface productcolerIRepositry :IRepository<ProductColors>
    {
         void RemoveRang(IEnumerable<ProductColors> productColors);
     
    }
}
