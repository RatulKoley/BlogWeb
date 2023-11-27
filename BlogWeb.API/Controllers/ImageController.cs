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
        public async Task<ActionResult> UploadAsync(IFormFile formFile, string imagePath)
        {
            var result = await imagerepo.UploadAsync(formFile);
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
