namespace BlogWeb.API.Models.ViewModels
{
    public class AddLikeRequestViewModel
    {
        public Guid BlogPostId { get; set; }
        public Guid UserId { get; set; }
    }
}
