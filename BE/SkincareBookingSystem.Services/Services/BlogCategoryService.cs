using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.BlogCategories;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.Helpers.Responses;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using System.ComponentModel;
using System.Net;
using System.Security.Claims;

namespace SkincareBookingSystem.Services.Services
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapperService _mapperService;
        public BlogCategoryService(IUnitOfWork unitOfWork, IAutoMapperService mapperService)
        {
            _unitOfWork = unitOfWork;
            _mapperService = mapperService;
        }
        /// <summary>
        /// Create a Blog Category from the BlogCategoryDto
        /// </summary>
        /// <param name="blogCategoryDto"></param>
        /// <returns>
        /// A ResponseDto with the BlogCategory object, a success message and a status code
        /// </returns>
        public async Task<ResponseDto> CreateBlogCategory(ClaimsPrincipal User, CreateBlogCategoryDto blogCategoryDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var blogCategory = _mapperService.Map<CreateBlogCategoryDto, BlogCategory>(blogCategoryDto);
            blogCategory.CreatedBy = User.FindFirstValue("Fullname");

            try
            {
                await _unitOfWork.BlogCategory.AddAsync(blogCategory);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.BlogCategory.NotCreated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }

            return SuccessResponse.Build(
                message: StaticResponseMessage.BlogCategory.Created,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: blogCategory);
        }

        public async Task<ResponseDto> GetAllBlogCategories()
        {
            var blogCategoryFromDb = await _unitOfWork.BlogCategory.GetAllAsync(a => a.Status != StaticOperationStatus.BlogCategory.Deleted);

            return (blogCategoryFromDb.Any()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.BlogCategory.RetrievedAll,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: blogCategoryFromDb)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.BlogCategory.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: new List<BlogCategory>());
        }

        public async Task<ResponseDto> GetBlogCategory(ClaimsPrincipal User, Guid blogCategoryId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.BlogCategory.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var getBlogCategoryById = await _unitOfWork.BlogCategory.GetAsync(b => b.BlogCategoryId == blogCategoryId && b.Status != StaticOperationStatus.BlogCategory.Deleted);
            return (getBlogCategoryById is null) ?
                ErrorResponse.Build(
                    message: StaticResponseMessage.BlogCategory.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.BlogCategory.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: getBlogCategoryById);
        }

        public async Task<ResponseDto> UpdateBlogCategory(ClaimsPrincipal User, UpdateBlogCategoryDto updateBlogCategoryDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var blogCategoryFromDb = await _unitOfWork.BlogCategory
                .GetAsync(x => x.BlogCategoryId == updateBlogCategoryDto.BlogCategoryId);

            if (blogCategoryFromDb is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.BlogCategory.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var blogCategoryToUpdate = _mapperService.Map<UpdateBlogCategoryDto, BlogCategory>(updateBlogCategoryDto);
            blogCategoryToUpdate.UpdatedBy = User.FindFirstValue("FullName");
            blogCategoryToUpdate.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;

            _unitOfWork.BlogCategory.Update(blogCategoryFromDb, blogCategoryToUpdate);

            if (!await SaveChangesAsync())
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.BlogCategory.NotUpdated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }

            return SuccessResponse.Build(
                message: StaticResponseMessage.BlogCategory.Updated,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: blogCategoryToUpdate);
        }

        public Task<ResponseDto> DeleteBlogCategory(ClaimsPrincipal User, Guid blogCategoryId)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> SaveChangesAsync()
        {
            return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
        }

    }
}
