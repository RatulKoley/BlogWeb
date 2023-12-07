using BlogWeb.API.Data;
using BlogWeb.API.Models.Helpers;
using BlogWeb.API.Models.ViewModels;
using BlogWeb.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogWeb.API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminCategoryController : Controller
    {
        private readonly ICategoriesRepository categoryrepo;
        private readonly BlogDBContext dbcon;
        public AdminCategoryController(ICategoriesRepository categoryrepo, BlogDBContext dbcon)
        {
            this.categoryrepo = categoryrepo;
            this.dbcon = dbcon;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(CategoryViewModel objModel)
        {
            if (ModelState.IsValid)
            {
                await categoryrepo.AddAsync(objModel.CategoryModel);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Index(string searchterm, string orderBy = "", int CurrentPage = 1)
        {
            using var transaction = await dbcon.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            searchterm = string.IsNullOrEmpty(searchterm) ? "" : searchterm.ToLower();
            CategoryListViewModel ObjModel = new();
            ObjModel.CategoryList = new();
            ObjModel.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "name_desc" : "";

            var IndexList = dbcon.Categorys.Include(_ => _.Tags).Where(_ => _.Name.ToLower().Contains(searchterm) || searchterm == null);

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
            if (IndexList.Any())
            {
                foreach (var Item in IndexList)
                {
                    CategoryInfo newModel = new();
                    newModel.CategoryModel = Item;
                    newModel.TagsCount = Item.Tags.Count();
                    ObjModel.CategoryList.Add(newModel);
                }
            }
            ObjModel.CurrentPage = CurrentPage;
            ObjModel.TotalPage = TotalPages;
            ObjModel.PageSize = PageSize;
            ObjModel.Term = searchterm;
            ObjModel.OrderBy = orderBy;
            await transaction.CommitAsync();
            return View(ObjModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            CategoryViewModel objModel = new();
            var result = await categoryrepo.GetAsync(id);
            if (result != null)
            {
                objModel.CategoryModel = result;
                return View(objModel);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel objModel)
        {
            var result = await categoryrepo.EditAsync(objModel.CategoryModel);
            if (result != null)
                return RedirectToAction("Index");
            return RedirectToAction("Edit", new { id = objModel.CategoryModel.Id });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await categoryrepo.DeleteAsync(Id);
            if (result != null)
                return RedirectToAction("Index");
            return RedirectToAction("Edit", new { id = Id });
        }
    }
}
