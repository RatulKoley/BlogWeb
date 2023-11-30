using System.ComponentModel.DataAnnotations;

namespace BlogWeb.API.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Passowrd 8 Characters")]
        public string Password { get; set; }
        public string? SuccessMassage { get; set; }
        public string? UserId { get; set; }
    }
}
