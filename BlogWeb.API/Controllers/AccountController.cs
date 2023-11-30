using BlogWeb.API.Models.ViewModels;
using BlogWeb.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogWeb.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository accountrepo;
        public AccountController(IAccountRepository accountrepo)
        {
            this.accountrepo = accountrepo;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel objModel)
        {
            if (ModelState.IsValid)
            {
                var result = await accountrepo.Register(objModel);
                if (result.SuccessMassage != null)
                {
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        public IActionResult Login(string ReturnUrl)
        {
            LoginViewModel newModel = new()
            {
                ReturnUrl = ReturnUrl
            };
            return View(newModel);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel objModel)
        {
            if (ModelState.IsValid)
            {
                var result = await accountrepo.Login(objModel);
                if (result.SuccessMsg != null)
                {
                    if (!string.IsNullOrWhiteSpace(objModel.ReturnUrl))
                    {
                        return Redirect(objModel.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await accountrepo.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View(); ;
        }
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult RegisterAdmin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(RegisterViewModel objModel)
        {
            var result = await accountrepo.RegisterAdmin(objModel);
            if (result.SuccessMassage != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserList()
        {
            var users = await accountrepo.GetAll();
            UserListViewModel NewModel = new();
            NewModel.UserList = new();
            foreach (var user in users)
            {
                UserModel newEntry = new()
                {
                    Id = Guid.Parse(user.Id),
                    UserName = user.UserName,
                    EmailAddress = user.Email
                };
                NewModel.UserList.Add(newEntry);
            }
            return View(NewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UserList(UserListViewModel objModel)
        {
            await accountrepo.RegisterUser(objModel);
            return RedirectToAction("UserList");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            await accountrepo.DeleteUser(Id);
            return RedirectToAction("UserList");
        }
        public async Task<IActionResult> Edit(Guid Id)
        {
            UserListViewModel objModel = new();
            var result = await accountrepo.GetUser(Id);
            if (result != null)
            {
                objModel = result;
                return View(objModel);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserListViewModel objModel)
        {
            await accountrepo.EditUser(objModel);
            if (objModel != null)
            {
                return RedirectToAction("UserList");
            }
            return View();

        }

    }
}
