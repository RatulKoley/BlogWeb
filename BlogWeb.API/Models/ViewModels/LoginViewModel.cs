using System.ComponentModel.DataAnnotations;

namespace BlogWeb.API.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string? SuccessMsg { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
