using BlogWeb.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlogWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {

        private readonly IImageRepository imagerepo;

        public ImageController(IImageRepository imagerepo)
        {
            this.imagerepo = imagerepo;
        }
        [HttpPost]
        public async Task<ActionResult> UploadAsync(IFormFile file)
        {
            var result = await imagerepo.UploadAsync(file);
            if (result == null)
            {
                return Problem("Image Upload Unsuccessfull", null, (int)HttpStatusCode.InternalServerError);
            }
            return new JsonResult(new
            {
                link = result
            });

        }
    }
}
