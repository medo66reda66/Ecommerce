﻿namespace Ecommerce.Models
{
    public class Categores
    {
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}
