using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.TestQuestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.IServices
{
    public interface ITestQuestionService
    {
        Task<ResponseDto> CreateTestQuestion(ClaimsPrincipal User, CreateTestQuestionDto createTestQuestionDto);
        Task<ResponseDto> UpdateTestQuestion(ClaimsPrincipal User, UpdateTestQuestionDto updateTestQuestionDto);
        Task<ResponseDto> GetTestQuestionById(ClaimsPrincipal User, Guid testQuestionId);
        Task<ResponseDto> GetAllTestQuestions();
        Task<ResponseDto> DeleteTestQuestion(ClaimsPrincipal User, Guid testQuestionId);
        Task<ResponseDto> GetTestQuestionBySkinTestId(Guid skinTestId);

    }
}
