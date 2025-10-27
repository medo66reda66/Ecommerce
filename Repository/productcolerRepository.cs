using Ecommerce.Repository.IRepository;

namespace Ecommerce.Repository
{
    public class productcolerRepository : Repository<ProductColors> , productcolerIRepositry
    {
        private ApplicationDBContext _dbContext;// = new ApplicationDBContext();

        public productcolerRepository(ApplicationDBContext dbContext): base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public void RemoveRang(IEnumerable<ProductColors> productColors)
        {
            _dbContext.RemoveRange(productColors);
        }
    }
}
