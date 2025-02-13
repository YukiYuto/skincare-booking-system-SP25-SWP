using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
//using SkincareBookingSystem.Models.Dto.BlogCategory;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;
using System.ComponentModel;
using System.Net;
using System.Security.Claims;

namespace SkincareBookingSystem.Services.Services
{
    public class BlogCategoryService : IBlogCategoryService
    {
        /*private readonly IUnitOfWork _unitOfWork;
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
            BlogCategory blogCategory = _mapperService.Map<CreateBlogCategoryDto, BlogCategory>(blogCategoryDto);

            await _unitOfWork.BlogCategory.AddAsync(blogCategory);
            await _unitOfWork.SaveAsync();

            return new ResponseDto()
            {
                Result = blogCategory,
                IsSuccess = true,
                Message = "Blog Category created successfully",
                StatusCode = 201
            };
        }

        public async Task<ResponseDto> GetAllBlogCategories()
        {
            IEnumerable<BlogCategory> blogCategories = await _unitOfWork.BlogCategory.GetAllAsync();
            // Map the BlogCategory collection to a BlogCategoryDto collection
            IEnumerable<GetBlogCategoryDto> blogCategoryDtos = _mapperService.MapCollection<BlogCategory, GetBlogCategoryDto>(blogCategories);

            return new ResponseDto()
            {
                Result = blogCategoryDtos,
                IsSuccess = true,
                Message = "All Blog Categories",
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> GetBlogCategory(Guid blogCategoryId)
        {
            BlogCategory? dataFromDb = await _unitOfWork.BlogCategory.GetAsync(x => x.BlogCategoryId == blogCategoryId);

            if (dataFromDb is null)
            {
                return new ResponseDto()
                {
                    Result = null,
                    IsSuccess = false,
                    Message = "Blog Category not found",
                    StatusCode = 404
                };
            }

            GetBlogCategoryDto blogCategoryDto = _mapperService.Map<BlogCategory, GetBlogCategoryDto>(dataFromDb);

            return new ResponseDto()
            {
                Result = blogCategoryDto,
                IsSuccess = true,
                Message = "Blog Category found",
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> UpdateBlogCategory(ClaimsPrincipal User, UpdateBlogCategoryDto updateBlogCategoryDto)
        {
            BlogCategory? dataFromDb = await _unitOfWork.BlogCategory
                .GetAsync(x => x.BlogCategoryId == updateBlogCategoryDto.Id);

            if (dataFromDb is null)
            {
                return new ResponseDto()
                {
                    Result = null,
                    IsSuccess = false,
                    Message = "Blog Category not found",
                    StatusCode = 404
                };
            }

            BlogCategory dataUpdate = _mapperService.Map<UpdateBlogCategoryDto, BlogCategory>(updateBlogCategoryDto);

            _unitOfWork.BlogCategory.Update(dataFromDb, dataUpdate);
            await _unitOfWork.SaveAsync();

            return new ResponseDto()
            {
                Result = dataFromDb,
                IsSuccess = true,
                Message = "Blog Category updated successfully",
                StatusCode = 200
            };
        }

        public Task<ResponseDto> DeleteBlogCategory(ClaimsPrincipal User, Guid blogCategoryId)
        {
            throw new NotImplementedException();
        }*/
    }
}
