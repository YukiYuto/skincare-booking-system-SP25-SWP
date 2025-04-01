using SkincareBookingSystem.Models.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SkincareBookingSystem.Models.Dto.Customer;

namespace SkincareBookingSystem.Services.IServices
{
    public interface ICustomerService
    {
        Task<ResponseDto> GetCustomerDetailsById(ClaimsPrincipal user, Guid customerId);
        Task<ResponseDto> GetAllCustomers();
        Task<ResponseDto> GetCustomerIdByUserId(ClaimsPrincipal User);
        Task<ResponseDto> GetCustomerTimeTable(ClaimsPrincipal User);
        
        Task<ResponseDto> GetOrderByCustomer(ClaimsPrincipal User);
        Task<ResponseDto> GetRecommendationBySkinProfile(RecommendationDto recommendationDto);
    }
}
