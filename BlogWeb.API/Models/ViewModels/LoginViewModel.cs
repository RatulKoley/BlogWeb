namespace BlogWeb.API.Models.ViewModels
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? SuccessMsg { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
