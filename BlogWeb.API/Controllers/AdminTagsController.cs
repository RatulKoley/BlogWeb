using BlogWeb.API.Data;
using BlogWeb.API.Models.Helpers;
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
        private readonly BlogDBContext dbcon;
        public AdminTagsController(ITagRepository tagrepo, BlogDBContext dbcon)
        {
            this.tagrepo = tagrepo;
            this.dbcon = dbcon;
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
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Index(string searchterm, string orderBy = "", int CurrentPage = 1)
        {
            searchterm = string.IsNullOrEmpty(searchterm) ? "" : searchterm.ToLower();
            TagListViewModel ObjModel = new();
            ObjModel.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "name_desc" : "";

            var IndexList = dbcon.Tags.Where(_ => _.IsActive == true
                        && _.Name.ToLower().Contains(searchterm) || searchterm == null);

            switch (orderBy)
            {
                case "name_desc":
                    IndexList = IndexList.OrderByDescending(a => a.Name);
                    break;
                default:
                    IndexList = IndexList.OrderBy(a => a.Name);
                    break;
            }
            int TotalRecords = IndexList.Count();
            int PageSize = 5;
            int TotalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
            IndexList = IndexList.Skip((CurrentPage - 1) * PageSize).Take(PageSize);

            ObjModel.TagList = IndexList;
            ObjModel.CurrentPage = CurrentPage;
            ObjModel.TotalPage = TotalPages;
            ObjModel.PageSize = PageSize;
            ObjModel.Term = searchterm;
            ObjModel.OrderBy = orderBy;

            return View(ObjModel);
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
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await tagrepo.DeleteAsync(Id);
            if (result != null)
                return RedirectToAction("List");
            return RedirectToAction("Edit", new { id = Id });
        }
    }
}
