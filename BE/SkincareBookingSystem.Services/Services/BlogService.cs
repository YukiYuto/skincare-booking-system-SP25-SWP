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

                // After saving, retrieve the blog with related entities
                var createdBlog = await _unitOfWork.Blog.GetAsync(
                    b => b.BlogId == createBlog.BlogId,
                    includeProperties: $"{nameof(Blog.BlogCategory)},{nameof(Blog.ApplicationUser)}");

                if (createdBlog == null)
                {
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Blog.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);
                }

                // Map to BlogDetailDto to include related data
                var blogDto = _autoMapperService.Map<Blog, BlogDetailDto>(createdBlog);
                return SuccessResponse.Build(
                    message: StaticResponseMessage.Blog.Created,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: blogDto);
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

            var deleteBlog = await _unitOfWork.Blog.GetAsync(b => b.BlogId == blogId,
                includeProperties: $"{nameof(Blog.BlogCategory)},{nameof(Blog.ApplicationUser)}");
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

            //if (deleteBlog.AuthorId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            //{
            //    return ErrorResponse.Build(
            //        message: StaticResponseMessage.Blog.NotAuthorized,
            //        statusCode: StaticOperationStatus.StatusCode.Forbidden);
            //}

            deleteBlog.Status = StaticOperationStatus.Blog.Deleted;
            deleteBlog.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
            deleteBlog.UpdatedBy = User.FindFirstValue("Fullname");

            //return (await SaveChangesAsync()) ?
            //    SuccessResponse.Build(
            //        message: StaticResponseMessage.Blog.Deleted,
            //        statusCode: StaticOperationStatus.StatusCode.Ok,
            //        result: deleteBlog)
            //    :
            //    ErrorResponse.Build(
            //        message: StaticResponseMessage.Blog.NotDeleted,
            //        statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            if (await SaveChangesAsync())
            {
                // Map the deleted blog to BlogDetailDto
                var deletedBlogDto = _autoMapperService.Map<Blog, BlogDetailDto>(deleteBlog);

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Blog.Deleted,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: deletedBlogDto);
            }
            else
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Blog.NotDeleted,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> GetAllBlog()
        {
            var getAllBlog = await _unitOfWork.Blog.GetAllAsync(b => b.Status != StaticOperationStatus.Blog.Deleted,
                includeProperties: $"{nameof(Blog.BlogCategory)},{nameof(Blog.ApplicationUser)}");
            var blogDtos = _autoMapperService.Map<IEnumerable<Blog>, IEnumerable<BlogDetailDto>>(getAllBlog);
            return (getAllBlog.Any()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.Blog.RetrievedAll,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: blogDtos)
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

            var getBlogById = await _unitOfWork.Blog.GetAsync(b => b.BlogId == blogId 
                && b.Status != StaticOperationStatus.Blog.Deleted,
                includeProperties: $"{nameof(Blog.BlogCategory)},{nameof(Blog.ApplicationUser)}");
            var blogDto = _autoMapperService.Map<Blog, BlogDetailDto>(getBlogById);

            return (getBlogById is null) ?
                ErrorResponse.Build(
                    message: StaticResponseMessage.Blog.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.Blog.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: blogDto);
        }

        public async Task<ResponseDto> UpdateBlog(ClaimsPrincipal User, UpdateBlogDto updateBlogDto)
        {
            //if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            //{
            //    return ErrorResponse.Build(
            //        message: StaticResponseMessage.Blog.NotFound,
            //        statusCode: StaticOperationStatus.StatusCode.NotFound);
            //}

            //var updateBlog = await _unitOfWork.Blog.GetAsync(b => b.BlogId == updateBlogDto.BlogId,
            //    nameof(Blog.BlogCategory), nameof(Blog.ApplicationUser));

            //if (updateBlog is null)
            //{
            //    return ErrorResponse.Build(
            //        message: StaticResponseMessage.Blog.NotFound,
            //        statusCode: StaticOperationStatus.StatusCode.NotFound);
            //}

            //var updatedData = _autoMapperService.Map<UpdateBlogDto, Blog>(updateBlogDto);
            //updatedData.UpdatedBy = User.FindFirstValue("Fullname");
            //updatedData.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
            //updatedData.Status = StaticOperationStatus.Blog.Modified;

            //_unitOfWork.Blog.Update(updateBlog, updatedData);

            //return (await SaveChangesAsync()) ?
            //    SuccessResponse.Build(
            //        message: StaticResponseMessage.Blog.Updated,
            //        statusCode: StaticOperationStatus.StatusCode.Ok,
            //        result: updatedData)
            //    :
            //    ErrorResponse.Build(
            //        message: StaticResponseMessage.Blog.NotUpdated,
            //        statusCode: StaticOperationStatus.StatusCode.InternalServerError);

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Blog.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            // Retrieve the existing blog with related entities
            var updateBlog = await _unitOfWork.Blog.GetAsync(
                b => b.BlogId == updateBlogDto.BlogId,
                includeProperties: $"{nameof(Blog.BlogCategory)},{nameof(Blog.ApplicationUser)}"); // Ensure these names match the properties
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
            updatedData.CreatedBy = updateBlog.CreatedBy;
            updatedData.CreatedTime = updateBlog.CreatedTime;

            _unitOfWork.Blog.Update(updateBlog, updatedData);


            //return (await SaveChangesAsync()) ?
            //    SuccessResponse.Build(
            //        message: StaticResponseMessage.Blog.Updated,
            //        statusCode: StaticOperationStatus.StatusCode.Ok,
            //        result: updateBlog)
            //    :
            //    ErrorResponse.Build(
            //        message: StaticResponseMessage.Blog.NotUpdated,
            //        statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            if (await SaveChangesAsync())
            {
                // Map the updated blog to BlogDetailDto
                var updatedBlogDto = _autoMapperService.Map<Blog, BlogDetailDto>(updateBlog);

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Blog.Updated,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: updatedBlogDto);
            }
            else
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Blog.NotUpdated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        private async Task<bool> SaveChangesAsync()
        {
            return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
        }
    }
}
