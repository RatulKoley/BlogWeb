using BlogWeb.API.Data;
using BlogWeb.API.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace BlogWeb.API.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BlogDBContext dbcon;
        public BlogPostRepository(BlogDBContext dbcon)
        {
            this.dbcon = dbcon;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            dbcon.BlogPosts.Add(blogPost);
            await dbcon.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid Id)
        {
            var result = await dbcon.BlogPosts
               .Include(_ => _.Tags)
               .FirstOrDefaultAsync(_ => _.Id == Id);
            if (result != null)
            {
                result.Visible = false;
                dbcon.BlogPosts.Update(result);
                await dbcon.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<BlogPost?> EditAsync(BlogPost blogPost)
        {
            var result = await dbcon.BlogPosts
                .Include(_ => _.Tags)
                .FirstOrDefaultAsync(_ => _.Id == blogPost.Id);
            if (result != null)
            {
                result.Heading = blogPost.Heading;
                result.PageTitle = blogPost.PageTitle;
                result.Content = blogPost.Content;
                result.ShortDescription = blogPost.ShortDescription;
                result.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                result.UrlHandle = blogPost.UrlHandle;
                result.PublishedDate = blogPost.PublishedDate;
                result.Author = blogPost.Author;
                result.Visible = blogPost.Visible;
                result.Tags = blogPost.Tags;

                dbcon.BlogPosts.Update(result);
                await dbcon.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await dbcon.BlogPosts.Include(_ => _.Tags)
                 .Where(_ => _.Visible == true).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid Id)
        {
            var result = await dbcon.BlogPosts
                .Include(_ => _.Tags)
                .FirstOrDefaultAsync(_ => _.Id == Id);
            if (result != null)
                return result;
            return null;
        }

        public async Task<BlogPost?> GetAsync(string urlHandle)
        {
            var result = await dbcon.BlogPosts
                .Include(_ => _.Tags)
                .FirstOrDefaultAsync(_ => _.UrlHandle == urlHandle);
            if (result != null)
                return result;
            return null;
        }
    }
}
