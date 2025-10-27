using System.ComponentModel.DataAnnotations;

namespace Ecommerce.viwemodel
{
    public class ResendEmailConfirmVM
    {
        public int id { get; set; }
        [Required]
        public string usernameOREmail { get; set; } = string.Empty;
    }
}
