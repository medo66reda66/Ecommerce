namespace Ecommerce.ViewModel

{
    public class ProductVM
    {
        public bool Status { get; set; }
        public Products? Products { get; set; }
        public List<Categores> Categores { get; set; }
        public List<Brands> Brands { get; set; }
        //public List<ProductSubimgs> ProductSubimgs { get; set; }
       
    }
}
