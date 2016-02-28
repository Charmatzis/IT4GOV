using System.ComponentModel.DataAnnotations;

namespace IT4GOV.IdentityServer.UI.Login
{
    public class LoginInputModel
    {
        [Required]
        [Display(Name = "Όνομα χρήστη")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Κωδικός")]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string SignInId { get; set; }
    }
}