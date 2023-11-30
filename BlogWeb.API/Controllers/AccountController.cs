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
            var result = await accountrepo.Register(objModel);
            if (result.SuccessMassage != null)
            {
                return RedirectToAction("Login");
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
            var result = await accountrepo.Login(objModel);
            if (result.SuccessMsg != null)
            {
                if (!string.IsNullOrWhiteSpace(objModel.ReturnUrl))
                {
                    return Redirect(objModel.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
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

    }
}
