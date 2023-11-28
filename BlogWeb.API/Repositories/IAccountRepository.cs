using BlogWeb.API.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogWeb.API.Repositories
{
    public interface IAccountRepository
    {
        Task<RegisterViewModel> Register(RegisterViewModel objModel);
        Task<LoginViewModel> Login(LoginViewModel objModel);
        Task<IActionResult> Logout();
    }
}
