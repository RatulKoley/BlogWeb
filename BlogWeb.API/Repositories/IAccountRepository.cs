using BlogWeb.API.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogWeb.API.Repositories
{
    public interface IAccountRepository
    {
        Task<RegisterViewModel> Register(RegisterViewModel objModel);
        Task<LoginViewModel> Login(LoginViewModel objModel);
        Task<IActionResult> Logout();

        //User Section
        Task<IEnumerable<IdentityUser>> GetAll();
        Task<UserListViewModel> RegisterUser(UserListViewModel objModel);
        Task<UserListViewModel?> GetUser(Guid Id);
        Task<UserListViewModel?> GetUserByName(string UserName);
        Task<UserListViewModel?> EditUser(UserListViewModel objModel);
        Task<UserListViewModel?> EditUserByName(UserListViewModel objModel);
        Task<UserListViewModel?> ChangePassword(UserListViewModel objModel);
        Task<string?> DeleteUser(Guid Id);
    }
}
