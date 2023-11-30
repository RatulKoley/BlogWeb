
using BlogWeb.API.Data;
using BlogWeb.API.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace BlogWeb.API.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BlogDBContext dbcon;
        public BlogPostLikeRepository(BlogDBContext dbcon)
        {
            this.dbcon = dbcon;
        }

        public async Task<BlogPostComment> AddComment(BlogPostComment objModel)
        {
            await dbcon.BlogPostComments.AddAsync(objModel);
            await dbcon.SaveChangesAsync();
            return objModel;
        }

        public async Task<BlogPostLike> AddLikes(BlogPostLike objModel)
        {
            await dbcon.BlogPostLikes.AddAsync(objModel);
            await dbcon.SaveChangesAsync();
            return objModel;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsForBlog(Guid BlogPostId)
        {
            return await dbcon.BlogPostComments
                 .Where(_ => _.BlogPostId == BlogPostId)
                 .ToListAsync();
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid BlogPostId)
        {
            return await dbcon.BlogPostLikes
                 .Where(_ => _.BlogPostId == BlogPostId)
                 .ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            return await dbcon.BlogPostLikes
                .CountAsync(_ => _.BlogPostId == blogPostId);
        }
    }
}
