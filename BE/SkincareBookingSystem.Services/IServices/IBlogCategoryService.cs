using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.BlogCategory;

namespace SkincareBookingSystem.Services.IServices
{
    public interface IBlogCategoryService
    {
        Task<ResponseDto> CreateBlogCategory(BlogCategoryDto blogCategoryDto);
    }
}
