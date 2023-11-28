using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net;

namespace BlogWeb.API.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly Account account;
        private readonly IConfiguration configuration;
        public ImageRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            account = new Account(
                configuration.GetSection("Cloudinary")["CloudName"],
                configuration.GetSection("Cloudinary")["ApiKey"],
                configuration.GetSection("Cloudinary")["ApiSecret"]);
        }
        public async Task<string> UploadAsync(IFormFile file)
        {
            Cloudinary client = new(account);
            ImageUploadParams uploadParams = new()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.FileName
            };
            var uploadResult = await client.UploadAsync(uploadParams);
            if (uploadResult != null && uploadResult.StatusCode == HttpStatusCode.OK)
            {
                return uploadResult.SecureUri.ToString();
            }
            return null;
        }
    }
}
