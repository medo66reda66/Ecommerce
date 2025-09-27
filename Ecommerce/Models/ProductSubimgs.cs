namespace Ecommerce.Models
{
    public class ProductSubimgs
    {
       
        public int ProductId { get; set; }
        public Products Product { get; set; }=null!;
        public string Img { get; set; }=string.Empty;
    }
}
