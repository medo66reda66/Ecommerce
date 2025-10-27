using Ecommerce.Validation;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Categores
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [MaxLength(500)]
        [MinLength(3)]
        //[Customlenght(3,8 , ErrorMessage ="mmmmmmmmmmmmmoooooooooooo")]
        public string Name { get; set; }=string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}
