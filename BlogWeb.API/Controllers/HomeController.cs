using BlogWeb.API.Models;
using BlogWeb.API.Models.Domains;
using BlogWeb.API.Models.ViewModels;
using BlogWeb.API.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogWeb.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository blogrepo;
        private readonly ITagRepository tagrepo;
        private readonly IBlogPostLikeRepository bloglikerepo;

        private SignInManager<IdentityUser> signinManager;
        private UserManager<IdentityUser> userManager;

        public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogrepo, ITagRepository tagrepo, IBlogPostLikeRepository bloglikerepo
            , UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            this.blogrepo = blogrepo;
            this.tagrepo = tagrepo;
            this.bloglikerepo = bloglikerepo;
            this.userManager = userManager;
            this.signinManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var blogposts = await blogrepo.GetAllAsync();
            var tags = await tagrepo.GetAllAsync();
            HomeViewModel objModel = new();
            objModel.BlogPostList = blogposts.ToList();
            objModel.TagList = tags.ToList();
            return View(objModel);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string urlHandle)
        {
            BlogPostsDetailsViewModel newModel = new();
            var result = await blogrepo.GetAsync(urlHandle);
            if (result != null)
            {
                if (signinManager.IsSignedIn(User))
                {
                    var AllLikesForBlog = await
                        bloglikerepo.GetLikesForBlog(result.Id);

                    var userId = userManager.GetUserId(User);
                    if (userId != null)
                    {
                        var LikeStatus = AllLikesForBlog
                            .FirstOrDefault(_ => _.UserId == Guid.Parse(userId));
                        if (LikeStatus != null)
                            newModel.IsLiked = true;
                        else
                            newModel.IsLiked = false;
                    }
                }
                var AllComments = await bloglikerepo.GetCommentsForBlog(result.Id);
                List<BlogComment> CommentList = new();
                foreach (var Comment in AllComments)
                {
                    BlogComment newEntry = new()
                    {
                        Description = Comment.Description,
                        DateAdded = Comment.DateAdded,
                        UserName = (await userManager.FindByIdAsync(Comment.UserId.ToString())).UserName
                    };
                    CommentList.Add(newEntry);
                }
                var LikesCount = await bloglikerepo.GetTotalLikes(result.Id);
                newModel.BlogPostInfo = result;
                newModel.TotalLikes = LikesCount;
                newModel.BlogComments = CommentList;
                return View(newModel);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Details(BlogPostsDetailsViewModel objModel)
        {
            if (signinManager.IsSignedIn(User))
            {
                BlogPostComment newComment = new()
                {
                    BlogPostId = objModel.BlogPostInfo.Id,
                    DateAdded = DateTime.Now,
                    Description = objModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User))
                };
                await bloglikerepo.AddComment(newComment);
                return RedirectToAction("Details", "Home", new
                {
                    urlHandle = objModel.BlogPostInfo.UrlHandle
                });
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}