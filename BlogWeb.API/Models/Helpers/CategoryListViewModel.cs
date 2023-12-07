using BlogWeb.API.Models.Domains;

namespace BlogWeb.API.Models.Helpers
{
    public class CategoryListViewModel : CommonModel
    {
        public List<CategoryInfo> CategoryList { get; set; }
    }
    public class CategoryInfo
    {
        public Category CategoryModel { get; set; }
        public int TagsCount { get; set; } = 0;
    }
}
