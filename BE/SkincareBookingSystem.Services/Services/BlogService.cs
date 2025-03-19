using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Blog;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.Helpers.Responses;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.Services
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapperService _autoMapperService;

        public BlogService(IUnitOfWork unitOfWork, IAutoMapperService autoMapperService)
        {
            _unitOfWork = unitOfWork;
            _autoMapperService = autoMapperService;
        }

        public async Task<ResponseDto> CreateBlog(ClaimsPrincipal User, CreateBlogDto createBlogDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var createBlog = _autoMapperService.Map<CreateBlogDto, Blog>(createBlogDto);
            createBlog.CreatedBy = User.FindFirstValue("Fullname");
            createBlog.CreatedTime = StaticOperationStatus.Timezone.Vietnam;
            createBlog.Status = StaticOperationStatus.Blog.Published;

            try
            {
                await _unitOfWork.Blog.AddAsync(createBlog);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex) 
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Blog.NotCreated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
            return SuccessResponse.Build(
                message: StaticResponseMessage.Blog.Created,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: createBlog);
        }

        public async Task<ResponseDto> DeleteBlog(ClaimsPrincipal User, Guid blogId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var deleteBlog = await _unitOfWork.Blog.GetAsync(b => b.BlogId == blogId);
            if (deleteBlog == null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Blog.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }
            if (deleteBlog.Status == StaticOperationStatus.Blog.Deleted)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Blog.AlreadyDeleted,
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);
            }

            deleteBlog.Status = StaticOperationStatus.Blog.Deleted;
            deleteBlog.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
            deleteBlog.UpdatedBy = User.FindFirstValue("Fullname");

            return (await SaveChangesAsync()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.Blog.Deleted,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: deleteBlog)
                :
                ErrorResponse.Build(
                    message: StaticResponseMessage.Blog.NotDeleted,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }

        public async Task<ResponseDto> GetAllBlog()
        {
            var getAllBlog = await _unitOfWork.Blog.GetAllAsync(b => b.Status != StaticOperationStatus.Blog.Deleted);
            return (getAllBlog.Any()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.Blog.RetrievedAll,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: getAllBlog)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.Blog.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: new List<Blog>());
        }

        public async Task<ResponseDto> GetBlogById(ClaimsPrincipal User, Guid blogId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Blog.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var getBlogById = await _unitOfWork.Blog.GetAsync(b => b.BlogId == blogId && b.Status != StaticOperationStatus.Blog.Deleted);
            return (getBlogById is null) ?
                ErrorResponse.Build(
                    message: StaticResponseMessage.Blog.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.Blog.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: getBlogById);
        }

        public async Task<ResponseDto> UpdateBlog(ClaimsPrincipal User, UpdateBlogDto updateBlogDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Blog.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var updateBlog = await _unitOfWork.Blog.GetAsync(b => b.BlogId == updateBlogDto.BlogId);
            if (updateBlog is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Blog.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var updatedData = _autoMapperService.Map<UpdateBlogDto, Blog>(updateBlogDto);
            updatedData.UpdatedBy = User.FindFirstValue("Fullname");
            updatedData.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
            updatedData.Status = StaticOperationStatus.Blog.Modified;
            _unitOfWork.Blog.Update(updateBlog, updatedData);

            return (await SaveChangesAsync()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.Blog.Updated,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: updatedData)
                :
                ErrorResponse.Build(
                    message: StaticResponseMessage.Blog.NotUpdated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }

        private async Task<bool> SaveChangesAsync()
        {
            return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
        }
    }
}
