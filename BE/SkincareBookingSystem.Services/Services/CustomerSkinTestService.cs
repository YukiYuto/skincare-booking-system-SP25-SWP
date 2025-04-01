using System.Security.Claims;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.CustomerSkinTest;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.Services.Services;

public class CustomerSkinTestService : ICustomerSkinTestService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAutoMapperService _autoMapperService;

    public CustomerSkinTestService(IUnitOfWork unitOfWork, IAutoMapperService autoMapperService)
    {
        _unitOfWork = unitOfWork;
        _autoMapperService = autoMapperService;
    }

    public async Task<ResponseDto> CreateCustomerSkinTest(ClaimsPrincipal user,
        CreateCustomerSkinTestDto createCustomerSkinTestDto)
    {
        var customer = await _unitOfWork.Customer.GetAsync(c => c.CustomerId == createCustomerSkinTestDto.CustomerId);
        if (customer == null)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Customer not found",
                StatusCode = 404
            };
        }

        var skinTest = await _unitOfWork.SkinTest.GetAsync(st => st.SkinTestId == createCustomerSkinTestDto.SkinTestId);
        if (skinTest == null)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Fixed SkinTest not found",
                StatusCode = 404
            };
        }
        
        var customerSkinTest = new CustomerSkinTest
        {
            CustomerSkinTestId = Guid.NewGuid(),
            CustomerId = createCustomerSkinTestDto.CustomerId,
            TakeAt = DateTime.UtcNow,
            Score = 0
        };


        int totalScore = 0;
        foreach (var answer in createCustomerSkinTestDto.Answers)
        {
            var testAnswer = await _unitOfWork.TestAnswer.GetAsync(a => a.TestAnswerId == answer.TestAnswerId);
            if (testAnswer != null)
            {
                totalScore += testAnswer.Score;
            }
        }

        customerSkinTest.Score = totalScore;


        await _unitOfWork.CustomerSkinTest.AddAsync(customerSkinTest);


        skinTest.CustomerSkinTestId = customerSkinTest.CustomerSkinTestId;
        _unitOfWork.SkinTest.Update(skinTest, skinTest);
        
        var skinProfile = await _unitOfWork.SkinProfile.GetAsync(sp =>
            sp.ScoreMin <= totalScore &&
            sp.ScoreMax >= totalScore);

        if (skinProfile == null)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "No matching skin profile found for this score",
                StatusCode = 404
            };
        }

        try
        {
            customer.SkinProfileId = skinProfile.SkinProfileId;
            await _unitOfWork.SaveAsync();
        }
        catch (Exception)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Failed to save skin test result",
                StatusCode = 500
            };
        }
        
        var responseDto = new CustomerSkinTestResponseDto
        {
            Score = totalScore,
            SkinProfile = _autoMapperService.Map<SkinProfile, SkinProfileDto>(skinProfile)
        };

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Skin test completed successfully",
            StatusCode = 200,
            Result = responseDto
        };
    }
}