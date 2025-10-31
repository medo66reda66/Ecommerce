namespace Ecommerce.Models
{
    public class ApplicationUserOtp
    {
        public string id { get; set; }
        public string Otp { get; set; }
        public DateTime Validto { get; set; }
        public DateTime CreateAt { get; set; }
        public bool Isvalid { get; set; }
        public string Applicationuserid { get; set; }
        public Appliccationusr Applicationuser { get; set; }

    }
}
