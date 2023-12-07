namespace BlogWeb.API.Models.Domains
{
    public class Category
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public int? PostCount { get; set; }
        public ICollection<Tag>? Tags { get; set; }
    }
}
