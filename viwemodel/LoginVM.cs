using System.ComponentModel.DataAnnotations;

namespace Ecommerce.viwemodel
{
    public class LoginVM
    {
        public int id { get; set; }
        [Required]
        public string usernameOREmail { get; set; }=string.Empty;
        [Required,DataType(DataType.Password)]
        public string password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }

    }
}
