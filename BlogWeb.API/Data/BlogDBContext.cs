using BlogWeb.API.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace BlogWeb.API.Data
{
    public class BlogDBContext : DbContext
    {
        public BlogDBContext(DbContextOptions<BlogDBContext> opt) : base(opt) { }

        public virtual DbSet<BlogPost> BlogPosts { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }



    }
}
