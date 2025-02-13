using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.Models.Dto.Cloudinary;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.Services.Services
{
    /// <summary>
    /// A service to interact with the Cloudinary API.
    /// This service gets the Cloudinary settings from the configuration file (appsettings.json)
    /// and uses them to create a Cloudinary object to interact with the Cloudinary API
    /// </summary>
    public class CloudinaryService : ICloudinaryService
    {
        public IConfiguration Configuration { get; }
        private CloudinarySettingsDto settings;
        private Cloudinary cloudinary;
        private readonly ApplicationDbContext _context;
        private IFormFile inputFile;
        private string fileType;

        public CloudinaryService(IConfiguration configuration, ApplicationDbContext context)
        {
            Configuration = configuration;
            settings = Configuration.GetSection(StaticCloudinarySettings.CloudinarySettingsSection).Get<CloudinarySettingsDto>();
            Account account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
            cloudinary = new Cloudinary(account);
            this._context = context;
        }
        public async Task<string> UploadImage()
        {
            if (fileType != StaticCloudinarySettings.Images)
            {
                throw new Exception("Invalid file type");
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(
                   inputFile.FileName.Replace(" ", ","),
                   inputFile.OpenReadStream()),
                PublicId = Guid.NewGuid().ToString(),
            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }

            return "";
        }

        public async Task<string> UploadVideo()
        {
            if (fileType != StaticCloudinarySettings.Videos)
            {
                throw new Exception("Invalid file type");
            }

            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(
                   inputFile.FileName.Replace(" ", ","),
                   inputFile.OpenReadStream()),
                PublicId = Guid.NewGuid().ToString(),
            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }

            return "";
        }

        public string GetFileType(string mimeType)
        {
            return mimeType.Contains("image") ? StaticCloudinarySettings.Images
                 : mimeType.Contains("video") ? StaticCloudinarySettings.Videos
                 : "";
        }

        public void SetFileData(IFormFile inputFile, string fileType)
        {
            this.inputFile = inputFile;
            this.fileType = fileType;
        }

    }
}
