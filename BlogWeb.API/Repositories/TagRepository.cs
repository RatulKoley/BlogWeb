using BlogWeb.API.Data;
using BlogWeb.API.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace BlogWeb.API.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogDBContext dbcon;
        public TagRepository(BlogDBContext dbcon)
        {
            this.dbcon = dbcon;
        }
        public async Task<Tag> AddAsync(Tag tag)
        {
            tag.IsActive = true;
            dbcon.Tags.Add(tag);
            await dbcon.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid Id)
        {
            var result = await dbcon.Tags.FirstOrDefaultAsync(_ => _.Id == Id);
            if (result != null)
            {
                result.IsActive = false;
                var updatedTag = dbcon.Tags.Update(result);
                await dbcon.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<Tag?> EditAsync(Tag tag)
        {
            var result = await dbcon.Tags.FirstOrDefaultAsync(_ => _.Id == tag.Id);
            if (result != null)
            {
                result.Name = tag.Name;
                result.DisplayName = tag.DisplayName;
                result.CategoryId = tag.CategoryId;
                var updatedTag = dbcon.Tags.Update(result);
                await dbcon.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await dbcon.Tags.Where(_ => _.IsActive == true).ToListAsync();
        }

        public Task<Tag?> GetAsync(Guid Id)
        {
            return dbcon.Tags.FirstOrDefaultAsync(_ => _.Id == Id);
        }
    }
}
