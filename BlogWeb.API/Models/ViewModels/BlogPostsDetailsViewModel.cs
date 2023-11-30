using BlogWeb.API.Models.Domains;

namespace BlogWeb.API.Models.ViewModels
{
    public class BlogPostsDetailsViewModel
    {
        public BlogPost BlogPostInfo { get; set; }
        public int TotalLikes { get; set; }
        public bool IsLiked { get; set; }
        public string CommentDescription { get; set; }
        public IEnumerable<BlogComment> BlogComments { get; set; }
    }
    public class BlogComment
    {
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string UserName { get; set; }
    }
}
