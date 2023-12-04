using BlogWeb.API.Models.Domains;

namespace BlogWeb.API.Models.Helpers
{
    public class BlogPostListViewModel : CommonModel
    {
        public IEnumerable<BlogPost> BlogPostList { get; set; }
    }
}
