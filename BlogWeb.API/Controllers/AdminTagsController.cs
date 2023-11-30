using BlogWeb.API.Models.ViewModels;
using BlogWeb.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogWeb.API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagrepo;
        public AdminTagsController(ITagRepository tagrepo)
        {
            this.tagrepo = tagrepo;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(TagViewModel objModel)
        {
            if (ModelState.IsValid)
            {
                await tagrepo.AddAsync(objModel.TagInfo);
            }
            return RedirectToAction("List");
        }
        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            return View(await tagrepo.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            TagViewModel objModel = new() { TagInfo = null };
            var result = await tagrepo.GetAsync(id);
            if (result != null)
            {
                objModel.TagInfo = result;
                return View(objModel);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TagViewModel objModel)
        {
            var result = await tagrepo.EditAsync(objModel.TagInfo);
            if (result != null)
                return RedirectToAction("List");
            return RedirectToAction("Edit", new { id = objModel.TagInfo.Id });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(TagViewModel objModel)
        {
            var result = await tagrepo.DeleteAsync(objModel.TagInfo.Id);
            if (result != null)
                return RedirectToAction("List");
            return RedirectToAction("Edit", new { id = objModel.TagInfo.Id });
        }
    }
}
