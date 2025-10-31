using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Ecommerce.Models
{
    public class Appliccationusr: IdentityUser
    {
        public string firstname { get; set; }=string.Empty;
        public string lastname { get; set; }= string.Empty;
        public string? adreeas {  get; set; }
    }
}
