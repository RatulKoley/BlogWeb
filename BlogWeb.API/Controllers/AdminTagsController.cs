using Microsoft.AspNetCore.Mvc;

namespace BlogWeb.API.Controllers
{
    public class AdminTagsController : Controller
    {
        public IActionResult Add()
        {
            return View();
        }
    }
}
