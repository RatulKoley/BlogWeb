using BlogWeb.API.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogWeb.API.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AccountRepository(UserManager<IdentityUser> usermanager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = usermanager;
            this.signInManager = signInManager;
        }

        public async Task<LoginViewModel> Login(LoginViewModel objModel)
        {
            var signInResult = await signInManager.PasswordSignInAsync(
                objModel.UserName, objModel.Password, false, false);
            if (signInResult.Succeeded && signInResult != null)
            {
                objModel.SuccessMsg = "Login Successfully";
                return objModel;
            }
            return objModel;
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return new OkResult();
        }

        public async Task<RegisterViewModel> Register(RegisterViewModel objModel)
        {
            IdentityUser newUser = new()
            {
                UserName = objModel.UserName,
                Email = objModel.Email
            };
            var identityResult = await _userManager.CreateAsync(
                newUser, objModel.Password);
            if (identityResult.Succeeded)
            {
                //Assign Role To New User
                var identityRole = await _userManager.AddToRoleAsync(newUser, "User");
                if (identityRole.Succeeded)
                {
                    objModel.SuccessMassage = "User Registered Successfully";
                }
            }
            return objModel;
        }
    }
}
