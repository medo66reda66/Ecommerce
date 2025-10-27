using System.ComponentModel.DataAnnotations;

namespace Ecommerce.viwemodel
{
    public class VolidateOtpVM
    {
        public int id {  get; set; }
        [Required] 
        public string Otp { get; set; }=string.Empty;
        public string Applicationuserid { get; set; }= string.Empty;
    }
}
