using BlogWeb.API.Models.ViewModels;
using BlogWeb.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogWeb.API.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdminUserController : Controller
    {
        private readonly IAccountRepository accountrepo;
        public AdminUserController(IAccountRepository accountrepo)
        {
            this.accountrepo = accountrepo;
        }
        public async Task<IActionResult> List()
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
        public async Task<IActionResult> List(UserListViewModel objModel)
        {
            await accountrepo.RegisterUser(objModel);
            return RedirectToAction("List");
        }
    }
}
