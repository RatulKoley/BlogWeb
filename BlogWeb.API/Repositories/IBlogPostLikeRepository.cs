using BlogWeb.API.Models.Domains;

namespace BlogWeb.API.Repositories
{
    public interface IBlogPostLikeRepository
    {
        Task<int> GetTotalLikes(Guid blogPostId);
        Task<BlogPostLike> AddLikes(BlogPostLike objModel);
        Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid BlogPostId);

        Task<BlogPostComment> AddComment(BlogPostComment objModel);
        Task<IEnumerable<BlogPostComment>> GetCommentsForBlog(Guid BlogPostId);
    }
}
