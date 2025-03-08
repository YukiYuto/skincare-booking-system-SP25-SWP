using SkincareBookingSystem.Models.Dto.Blog;
using SkincareBookingSystem.Models.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.IServices
{
    public interface IBlogService
    {
        Task<ResponseDto> CreateBlog(ClaimsPrincipal User, CreateBlogDto createBlogDto);
        Task<ResponseDto> UpdateBlog(ClaimsPrincipal User, UpdateBlogDto updateBlogDto);
        Task<ResponseDto> DeleteBlog(ClaimsPrincipal User,  Guid blogId);
        Task<ResponseDto> GetBlogById(ClaimsPrincipal User, Guid blogId);
        Task<ResponseDto> GetAllBlog(); 
        Task<ResponseDto> GetBlogByCustomerId(Guid customerId);
    }
}
