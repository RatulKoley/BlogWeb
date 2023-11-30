using BlogWeb.API.Data;
using BlogWeb.API.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogWeb.API.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AuthDBContext authdbcon;
        public AccountRepository(UserManager<IdentityUser> usermanager, SignInManager<IdentityUser> signInManager, AuthDBContext authdbcon)
        {
            _userManager = usermanager;
            this.signInManager = signInManager;
            this.authdbcon = authdbcon;
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


        public async Task<RegisterViewModel> RegisterAdmin(RegisterViewModel objModel)
        {
            var FindUser = await _userManager.FindByNameAsync(objModel.UserName);
            if (FindUser != null)
            {
                if (!await _userManager.IsInRoleAsync(FindUser, "Admin"))
                {
                    var identityRole = await _userManager.AddToRoleAsync(FindUser, "Admin");
                    if (identityRole.Succeeded)
                    {
                        objModel.SuccessMassage = "Admin Role Assigned";
                    }
                }
                else
                {
                    objModel.SuccessMassage = "Already Admin";
                }
            }
            return objModel;
        }

        //User Repositories
        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            var result = await authdbcon.Users.ToListAsync();
            var SuperUser = result
                .FirstOrDefault(_ => _.Email == "SuperAdmin@gmail.com");
            if (SuperUser != null)
            {
                result.Remove(SuperUser);
            }
            return result;
        }
        public async Task<UserListViewModel> RegisterUser(UserListViewModel objModel)
        {
            IdentityUser newUser = new()
            {
                UserName = objModel.newUserInfo.newRegister.UserName,
                Email = objModel.newUserInfo.newRegister.Email
            };
            var identityResult = await _userManager.CreateAsync(
                newUser, objModel.newUserInfo.newRegister.Password);
            if (identityResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "User");
                if (objModel.newUserInfo.isAdmin)
                {
                    await _userManager.AddToRoleAsync(newUser, "Admin");
                }
                objModel.newUserInfo.newRegister.SuccessMassage = "User Registered Successfully";
            }
            return objModel;
        }

        public async Task<UserListViewModel?> GetUser(Guid Id)
        {
            UserListViewModel returnModel = new();
            returnModel.newUserInfo = new();
            returnModel.newUserInfo.newRegister = new();
            var FindUser = await _userManager.FindByIdAsync(Id.ToString());
            if (FindUser != null)
            {
                returnModel.newUserInfo.newRegister.UserId = FindUser.Id;
                returnModel.newUserInfo.newRegister.UserName = FindUser.UserName;
                returnModel.newUserInfo.newRegister.Email = FindUser.Email;

                if (await _userManager.IsInRoleAsync(FindUser, "Admin"))
                    returnModel.newUserInfo.isAdmin = true;
                else
                    returnModel.newUserInfo.isAdmin = false;
                return returnModel;
            }
            return null;
        }

        public async Task<UserListViewModel?> EditUser(UserListViewModel objModel)
        {
            var FindUser = await _userManager.FindByIdAsync(objModel.newUserInfo.newRegister.UserId);
            if (FindUser != null)
            {
                FindUser.UserName = objModel.newUserInfo.newRegister.UserName;
                FindUser.Email = objModel.newUserInfo.newRegister.Email;
                await _userManager.UpdateAsync(FindUser);
                return objModel;
            }
            return null;
        }

        public async Task<string?> DeleteUser(Guid Id)
        {
            var FindUser = await _userManager.FindByIdAsync(Id.ToString());
            if (FindUser != null)
            {
                await _userManager.DeleteAsync(FindUser);
                return "User Deleted";
            }
            return null;
        }
    }
}
