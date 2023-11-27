using BlogWeb.API.Models.Domains;

namespace BlogWeb.API.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync(); 
        Task<Tag?> GetAsync(Guid Id);
        Task<Tag> AddAsync(Tag tag);
        Task<Tag?> EditAsync(Tag tag);
        Task<Tag?> DeleteAsync(Guid Id);
    }
}
