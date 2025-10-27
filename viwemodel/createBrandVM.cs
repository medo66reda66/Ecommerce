using System.ComponentModel.DataAnnotations;

namespace Ecommerce.viwemodel
{
    public class createBrandVM
    {
        [Required]
        [MaxLength(10)]
        [MinLength(4)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public IFormFile Img { get; set; }
    }
}
