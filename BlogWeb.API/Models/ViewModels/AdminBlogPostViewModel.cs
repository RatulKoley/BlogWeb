using BlogWeb.API.Models.Domains;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogWeb.API.Models.ViewModels
{
    public class AdminBlogPostViewModel
    {
        public BlogPost blogPostInfo { get; set; }
        public IEnumerable<SelectListItem> Tags { get; set; }
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}
