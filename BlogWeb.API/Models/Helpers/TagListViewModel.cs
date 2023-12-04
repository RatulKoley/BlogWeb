using BlogWeb.API.Models.Domains;

namespace BlogWeb.API.Models.Helpers
{
    public class TagListViewModel : CommonModel
    {
        public IQueryable<Tag> TagList { get; set; }
    }
}
