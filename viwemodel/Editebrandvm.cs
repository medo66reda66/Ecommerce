using System.ComponentModel.DataAnnotations;

namespace Ecommerce.viwemodel
{
    public class Editebrandvm
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        [MinLength(4)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public string? Img { get; set; }
        public IFormFile? newImg { get; set; }
    }
}
