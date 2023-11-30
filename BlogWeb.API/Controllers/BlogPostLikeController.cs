using BlogWeb.API.Models.Domains;
using BlogWeb.API.Models.ViewModels;
using BlogWeb.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BlogWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostLikeController : ControllerBase
    {
        private readonly IBlogPostLikeRepository blogpostlikerepo;
        public BlogPostLikeController(IBlogPostLikeRepository blogpostlikerepo)
        {
            this.blogpostlikerepo = blogpostlikerepo;
        }
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequestViewModel objModel)
        {
            BlogPostLike newModel = new()
            {
                BlogPostId = objModel.BlogPostId,
                UserId = objModel.UserId
            };
            await blogpostlikerepo.AddLikes(newModel);
            return Ok();
        }
        [HttpGet]
        [Route("{blogPostId:Guid}/totalLikes")]
        public async Task<IActionResult> GetTotalLikesBlog([FromRoute] Guid blogPostId)
        {
            var totalLikes = await blogpostlikerepo.GetTotalLikes(blogPostId);
            return Ok(totalLikes);
        }

    }
}
