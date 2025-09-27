using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.DateAcsess.Entitytypeconficration
{
    public class ProuductColerEntitytypeconfigration : IEntityTypeConfiguration<ProductColors>
    {
        public void Configure(EntityTypeBuilder<ProductColors> builder)
        {
            builder.HasKey(p => new { p.ProductId ,p.Color});
        }
    }
}
