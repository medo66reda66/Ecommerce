using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.DateAcsess.Entitytypeconficration
{
    public class ProductImgEntitytypeconfidration: IEntityTypeConfiguration<ProductSubimgs>
    {
        public void Configure(EntityTypeBuilder<ProductSubimgs> builder)
        {
            builder.HasKey(p => new { p.ProductId, p.Img });
        }

    }
    
    
}
