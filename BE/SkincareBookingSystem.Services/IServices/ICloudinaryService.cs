using Microsoft.AspNetCore.Http;

namespace SkincareBookingSystem.Services.IServices
{
    public interface ICloudinaryService
    {
        Task<string> UploadImage();
        Task<string> UploadVideo();
        string GetFileType(string mimeType);
        void SetFileData(IFormFile inputFile, string fileType);
    }
}
