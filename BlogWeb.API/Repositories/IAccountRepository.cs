using BlogWeb.API.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogWeb.API.Repositories
{
    public interface IAccountRepository
    {
        Task<RegisterViewModel> Register(RegisterViewModel objModel);
        Task<RegisterViewModel> RegisterAdmin(RegisterViewModel objModel);
        Task<LoginViewModel> Login(LoginViewModel objModel);
        Task<IActionResult> Logout();

        //User Section
        Task<IEnumerable<IdentityUser>> GetAll();
        Task<UserListViewModel> RegisterUser(UserListViewModel objModel);
        Task<UserListViewModel?> GetUser(Guid Id);
        Task<UserListViewModel?> EditUser(UserListViewModel objModel);
        Task<string?> DeleteUser(Guid Id);
    }
}
