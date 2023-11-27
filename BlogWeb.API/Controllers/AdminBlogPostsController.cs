using BlogWeb.API.Models.Domains;
using BlogWeb.API.Models.ViewModels;
using BlogWeb.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogWeb.API.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagrepo;
        private readonly IBlogPostRepository blogrepo;
        public AdminBlogPostsController(ITagRepository tagrepo, IBlogPostRepository blogrepo)
        {
            this.tagrepo = tagrepo;
            this.blogrepo = blogrepo;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await tagrepo.GetAllAsync();
            AdminBlogPostViewModel model = new()
            {
                Tags = tags.Select(x => new SelectListItem
                {
                    Text = x.DisplayName,
                    Value = x.Id.ToString()
                })
            };
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Add(AdminBlogPostViewModel objModel)
        {
            var AllTags = new List<Tag>();
            foreach (var tag in objModel.SelectedTags)
            {
                var GuidId = Guid.Parse(tag);
                var NewTag = await tagrepo.GetAsync(GuidId);
                if (NewTag != null)
                {
                    AllTags.Add(NewTag);
                }
            }
            objModel.blogPostInfo.Tags = AllTags;
            await blogrepo.AddAsync(objModel.blogPostInfo);
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var result = await blogrepo.GetAllAsync();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            AdminBlogPostViewModel objModel = new();
            var result = await blogrepo.GetAsync(Id);
            var tagsFetch = await tagrepo.GetAllAsync();
            if (result != null)
            {
                objModel.blogPostInfo = result;
                objModel.Tags = tagsFetch.Select(_ => new SelectListItem
                {
                    Text = _.DisplayName,
                    Value = _.Id.ToString()
                });
                objModel.SelectedTags = result.Tags
                    .Select(_ => _.Id.ToString()).ToArray();

                return View(objModel);
            };
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AdminBlogPostViewModel objModel)
        {
            var AllTags = new List<Tag>();
            foreach (var tag in objModel.SelectedTags)
            {
                var GuidId = Guid.Parse(tag);
                var NewTag = await tagrepo.GetAsync(GuidId);
                if (NewTag != null)
                {
                    AllTags.Add(NewTag);
                }
            }
            objModel.blogPostInfo.Tags = AllTags;
            var result = await blogrepo.EditAsync(objModel.blogPostInfo);
            if (result != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = objModel.blogPostInfo.Id });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(AdminBlogPostViewModel objModel)
        {
            var result = await blogrepo.DeleteAsync(objModel.blogPostInfo.Id);
            if (result != null)
                return RedirectToAction("List");
            return RedirectToAction("Edit", new { id = objModel.blogPostInfo.Id });
        }
    }
}
