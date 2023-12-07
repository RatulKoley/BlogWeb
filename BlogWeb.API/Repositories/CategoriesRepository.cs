using BlogWeb.API.Data;
using BlogWeb.API.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace BlogWeb.API.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly BlogDBContext dbcon;
        public CategoriesRepository(BlogDBContext dbcon)
        {
            this.dbcon = dbcon;
        }
        public async Task<Category> AddAsync(Category category)
        {
            category.PostCount = 0;
            dbcon.Categorys.Add(category);
            await dbcon.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> DeleteAsync(Guid Id)
        {
            var result = await dbcon.Categorys.FirstOrDefaultAsync(_ => _.Id == Id);
            if (result != null)
            {
                dbcon.Categorys.Remove(result);
                await dbcon.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<Category?> EditAsync(Category category)
        {
            var result = await dbcon.Categorys.FirstOrDefaultAsync(_ => _.Id == category.Id);
            if (result != null)
            {
                result.Name = category.Name;
                result.PostCount = category.PostCount;
                var updatedcategory = dbcon.Categorys.Update(result);
                await dbcon.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbcon.Categorys.ToListAsync();
        }

        public Task<Category?> GetAsync(Guid Id)
        {
            return dbcon.Categorys.FirstOrDefaultAsync(_ => _.Id == Id);
        }
    }
}
