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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserList(string searchterm, string orderBy = "", int CurrentPage = 1)
        {
            searchterm = string.IsNullOrEmpty(searchterm) ? "" : searchterm.ToLower();
            UserListViewModel ObjModel = new();
            ObjModel.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "name_desc" : "";
            var users = await accountrepo.GetAll();
            ObjModel.UserList = new();
            foreach (var user in users)
            {
                UserModel newEntry = new()
                {
                    Id = Guid.Parse(user.Id),
                    UserName = user.UserName,
                    EmailAddress = user.Email
                };
                ObjModel.UserList.Add(newEntry);
            }
            var IndexList = ObjModel.UserList.Where(_ => _.UserName.ToLower().Contains(searchterm) || searchterm == null).ToList();

            switch (orderBy)
            {
                case "name_desc":
                    IndexList = IndexList.OrderByDescending(a => a.UserName).ToList();
                    break;
                default:
                    IndexList = IndexList.OrderBy(a => a.UserName).ToList();
                    break;
            }
            int TotalRecords = IndexList.Count();
            int PageSize = 5;
            int TotalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
            IndexList = IndexList.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            ObjModel.UserList = IndexList;
            ObjModel.CurrentPage = CurrentPage;
            ObjModel.TotalPage = TotalPages;
            ObjModel.PageSize = PageSize;
            ObjModel.Term = searchterm;
            ObjModel.OrderBy = orderBy;

            return View(ObjModel);
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
        public async Task<IActionResult> EditByName(string username)
        {
            UserListViewModel objModel = new();
            var result = await accountrepo.GetUserByName(username);
            if (result != null)
            {
                objModel = result;
                return View(objModel);
            }
            return View(null);
        }
        public async Task<IActionResult> ChangePassword(string username)
        {
            UserListViewModel objModel = new();
            var result = await accountrepo.GetUserByName(username);
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
        [HttpPost]
        public async Task<IActionResult> EditByName(UserListViewModel objModel)
        {
            await accountrepo.EditUserByName(objModel);
            if (objModel != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserListViewModel objModel)
        {
            await accountrepo.ChangePassword(objModel);
            if (objModel != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();

        }

    }
}
