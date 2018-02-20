using System.ComponentModel.DataAnnotations;

namespace StudentServisWebScraper.Api.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Korisničko ime"), Required(ErrorMessage ="Korisničko ime je obavezno.")]
        public string Username { get; set; }

        [Display(Name = "Lozinka"), Required(ErrorMessage = "Lozinka je obavezna."), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
