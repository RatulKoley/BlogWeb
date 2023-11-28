using BlogWeb.API.Models;
using BlogWeb.API.Models.ViewModels;
using BlogWeb.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogWeb.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository blogrepo;
        private readonly ITagRepository tagrepo;

        public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogrepo, ITagRepository tagrepo)
        {
            _logger = logger;
            this.blogrepo = blogrepo;
            this.tagrepo = tagrepo;
        }

        public async Task<IActionResult> Index()
        {
            var blogposts = await blogrepo.GetAllAsync();
            var tags = await tagrepo.GetAllAsync();
            HomeViewModel objModel = new();
            objModel.BlogPostList = blogposts.ToList();
            objModel.TagList = tags.ToList();
            return View(objModel);
        }
        public async Task<IActionResult> Details(string urlHandle)
        {
            var result = await blogrepo.GetAsync(urlHandle);
            if (result != null)
            {
                return View(result);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}