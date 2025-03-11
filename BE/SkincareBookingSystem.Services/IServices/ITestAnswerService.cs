using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.TestAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.IServices
{
    public interface ITestAnswerService
    {
        Task<ResponseDto> CreateTestAnswer(ClaimsPrincipal User, CreateTestAnswerDto createTestAnswerDto);
        Task<ResponseDto> UpdateTestAnswer(ClaimsPrincipal User, UpdateTestAnswerDto updateTestAnswerDto);
        Task<ResponseDto> GetTestAnswerById(ClaimsPrincipal User, Guid testAnswerId);
        Task<ResponseDto> GetAllTestAnswers();
        Task<ResponseDto> DeleteTestAnswer(ClaimsPrincipal User, Guid testAnswerId);
        Task<ResponseDto> GetTestAnswerByTestQuestionId(Guid testQuestionId);
    }
}
