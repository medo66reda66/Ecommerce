﻿namespace Ecommerce.Models
{
    public class Brands
    {
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public string Img { get; set; } = "default.png";
    }
}
