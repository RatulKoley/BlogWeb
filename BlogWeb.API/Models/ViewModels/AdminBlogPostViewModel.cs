using BlogWeb.API.Models.Domains;

namespace BlogWeb.API.Models.ViewModels
{
    public class AdminBlogPostViewModel
    {
        public BlogPost BlogPostInfo { get;set;}
        public IEnumerable<SelectListItem> Tags { get;set;}
        public string SelectedTag { get;set;}
    }
}
