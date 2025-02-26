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

        public CloudinaryService(IConfiguration configuration, ApplicationDbContext context)
        {
            Configuration = configuration;
            settings = Configuration.GetSection(StaticCloudinarySettings.CloudinarySettingsSection).Get<CloudinarySettingsDto>();
            Account account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
            cloudinary = new Cloudinary(account);
            this._context = context;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folderPath, Transformation? transformation = null)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(
                   file.FileName.Replace(" ", ","),
                   file.OpenReadStream()),
                PublicId = Guid.NewGuid().ToString(),
                Folder = folderPath,
                UseFilename = true,
                UniqueFilename = false,
                Transformation = transformation
            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }

            throw new Exception($"Failed to upload image to Cloudinary. Error: {uploadResult.Error.Message}");
        }

        public async Task<string> GetImageUrlAsync(string publicId)
        {
            var getResourceParams = new GetResourceParams(publicId);
            
            try
            {
                var resource = await cloudinary.GetResourceAsync(getResourceParams);
                return resource.SecureUrl.ToString();
            } catch (Exception e)
            {
                throw new Exception($"Failed to retrieve image from Cloudinary. Error: {e.Message}");
            }
        }

        public async Task<string> UploadVideoAsync(IFormFile file, string folderPath)
        {
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(
                   file.FileName.Replace(" ", ","),
                   file.OpenReadStream()),
                PublicId = Guid.NewGuid().ToString(),
            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }

            throw new Exception($"Failed to upload video to Cloudinary. Error: {uploadResult.Error.Message}");
        }

    }
}
