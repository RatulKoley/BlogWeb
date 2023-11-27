using BlogWeb.API.Models.ViewModels;
using BlogWeb.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogWeb.API.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly TagRepository tagrepo  ;
        public AdminBlogPostsController(TagRepository tagrepo)
        {
            this.tagrepo = tagrepo;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {  
            var tags = await tagrepo.GetAllAsync();
            AdminBlogPostViewModel model = new()
            {
             Tags = tags.Select(_=>_.Id).ToList
            } ;
            
            

            return View();
        }
    }
}
