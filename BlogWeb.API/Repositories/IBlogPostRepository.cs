using BlogWeb.API.Models.Domains;

namespace BlogWeb.API.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost?> GetAsync(Guid Id);
        Task<BlogPost?> GetAsync(string urlHandle);
        Task<BlogPost> AddAsync(BlogPost blogPost);
        Task<BlogPost?> EditAsync(BlogPost blogPost);
        Task<BlogPost?> DeleteAsync(Guid Id);
    }
}
