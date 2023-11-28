using BlogWeb.API.Models.Domains;

namespace BlogWeb.API.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<BlogPost> BlogPostList { get; set; }
        public List<Tag> TagList { get; set; }
    }
}
