using BlogWeb.API.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogWeb.API.Repositories
{
    public interface IAccountRepository
    {
        Task<RegisterViewModel> Register(RegisterViewModel objModel);
        Task<RegisterViewModel> RegisterAdmin(RegisterViewModel objModel);
        Task<UserListViewModel> RegisterUser(UserListViewModel objModel);
        Task<LoginViewModel> Login(LoginViewModel objModel);
        Task<IActionResult> Logout();
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}
