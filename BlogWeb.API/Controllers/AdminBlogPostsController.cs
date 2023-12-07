using BlogWeb.API.Data;
using BlogWeb.API.Models.Domains;
using BlogWeb.API.Models.Helpers;
using BlogWeb.API.Models.ViewModels;
using BlogWeb.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogWeb.API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagrepo;
        private readonly BlogDBContext dbcon;
        private readonly IBlogPostRepository blogrepo;
        public AdminBlogPostsController(ITagRepository tagrepo, IBlogPostRepository blogrepo, BlogDBContext dbcon)
        {
            this.tagrepo = tagrepo;
            this.dbcon = dbcon;
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
            return RedirectToAction("List");
        }

        public async Task<IActionResult> List(string searchterm, string orderBy = "", int CurrentPage = 1)
        {
            using var transaction = await dbcon.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            searchterm = string.IsNullOrEmpty(searchterm) ? "" : searchterm.ToLower();
            BlogPostListViewModel ObjModel = new();
            ObjModel.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "name_desc" : "";

            var IndexList = dbcon.BlogPosts.AsNoTracking().Include(_ => _.Tags).Where(_ => _.Visible == true
                        && _.Heading.ToLower().Contains(searchterm) || searchterm == null);

            switch (orderBy)
            {
                case "name_desc":
                    IndexList = IndexList.OrderByDescending(a => a.Heading);
                    break;
                default:
                    IndexList = IndexList.OrderBy(a => a.Heading);
                    break;
            }
            int TotalRecords = IndexList.Count();
            int PageSize = 5;
            int TotalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
            IndexList = IndexList.Skip((CurrentPage - 1) * PageSize).Take(PageSize);

            ObjModel.BlogPostList = IndexList;
            ObjModel.CurrentPage = CurrentPage;
            ObjModel.TotalPage = TotalPages;
            ObjModel.PageSize = PageSize;
            ObjModel.Term = searchterm;
            ObjModel.OrderBy = orderBy;
            await transaction.CommitAsync();
            return View(ObjModel);
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
                if (result.Tags != null)
                {
                    objModel.SelectedTags = result.Tags
                        .Select(_ => _.Id.ToString())
                        .ToArray();
                }
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
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await blogrepo.DeleteAsync(Id);
            if (result != null)
                return RedirectToAction("List");
            return RedirectToAction("Edit", new { id = Id });
        }
    }
}
