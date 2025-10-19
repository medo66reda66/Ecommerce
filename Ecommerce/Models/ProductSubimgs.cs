using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Models
{
    [PrimaryKey(nameof(Id))]
    public class ProductSubimgs
    {
       public int Id { get; set; }
        public int ProductId { get; set; }
        public Products Product { get; set; }=null!;
        public string Img { get; set; }=string.Empty;
    }
}
