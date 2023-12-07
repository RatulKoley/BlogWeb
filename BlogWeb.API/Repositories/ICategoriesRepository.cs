using BlogWeb.API.Models.Domains;

namespace BlogWeb.API.Repositories
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetAsync(Guid Id);
        Task<Category> AddAsync(Category category);
        Task<Category?> EditAsync(Category category);
        Task<Category?> DeleteAsync(Guid Id);
    }
}
