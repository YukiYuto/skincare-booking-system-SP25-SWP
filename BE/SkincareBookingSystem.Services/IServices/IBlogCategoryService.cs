using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.BlogCategory;
using System.Security.Claims;

namespace SkincareBookingSystem.Services.IServices
{
    public interface IBlogCategoryService
    {
        Task<ResponseDto> CreateBlogCategory(ClaimsPrincipal User, CreateBlogCategoryDto blogCategoryDto);
        Task<ResponseDto> GetAllBlogCategories();
        Task<ResponseDto> GetBlogCategory(Guid blogCategoryId);
        Task<ResponseDto> UpdateBlogCategory(ClaimsPrincipal User, UpdateBlogCategoryDto blogCategoryDto);
        Task<ResponseDto> DeleteBlogCategory(ClaimsPrincipal User, Guid blogCategoryId);
    }
}
