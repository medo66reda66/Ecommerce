﻿namespace Ecommerce.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string MainImg { get; set; }=string.Empty;
        public bool Status { get; set; }
        public double Rate { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public Categores Category { get; set; }
        public int BrandId { get; set; }
        public Brands Brand { get; set; }
        public List<ProductColors> ProductColors { get; set; }


    }
}
