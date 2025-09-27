namespace Ecommerce.Models
{
    public class ProductColors
    {
        public int ProductId { get; set; }
        public Products Product { get; set; } = null!;
        public string Color { get; set; }
       

    }
}
