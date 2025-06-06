﻿using System.Security.Claims;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.TypeItem;
using SkincareBookingSystem.Services.Helpers.Responses;
using SkincareBookingSystem.Services.Helpers.Users;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.Services.Services;

public class TypeItemService : ITypeItemService
{
    private readonly IAutoMapperService _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public TypeItemService(IUnitOfWork unitOfWork, IAutoMapperService mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto> CreateTypeItem(ClaimsPrincipal user, CreateTypeItemDto createTypeItemDto)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return new ResponseDto
            {
                Message = "User not found",
                IsSuccess = false,
                StatusCode = 404
            };

        if (createTypeItemDto.ServiceTypeIdList == null || !createTypeItemDto.ServiceTypeIdList.Any())
            return new ResponseDto
            {
                Message = "ServiceType cannot be empty",
                IsSuccess = false,
                StatusCode = 400
            };

        var typeItems = createTypeItemDto.ServiceTypeIdList.Select(sp => new TypeItem
        {
            ServiceId = createTypeItemDto.ServiceId,
            ServiceTypeId = sp
        }).ToList();

        await _unitOfWork.TypeItem.AddRangeAsync(typeItems);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "Create Type Item Successful",
            IsSuccess = true,
            StatusCode = 201,
            Result = typeItems
        };
    }

    public async Task<ResponseDto> GetAllTypeItem
    (
        ClaimsPrincipal User,
        int pageNumber = 1,
        int pageSize = 10,
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null
    )
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return new ResponseDto
            {
                Message = "User not found",
                IsSuccess = false,
                StatusCode = 404
            };

        // Xác định có phải Manager không
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        var isManager = userRole == StaticUserRoles.Manager;

        // Lấy danh sách TypeItems đã được filter/sort
        var (typeItemsFromDb, totalTypeItems) = await _unitOfWork.TypeItem.GetAllTypeItemAsync
        (
            pageNumber,
            pageSize,
            filterOn,
            filterQuery,
            sortBy
        );

        if (!typeItemsFromDb.Any())
            return new ResponseDto
            {
                Message = "No Type Items found",
                IsSuccess = false,
                StatusCode = 404
            };

        var typeItems = typeItemsFromDb.Distinct().ToList();

        return new ResponseDto
        {
            Result = new
            {
                TotalTypeItems = totalTypeItems,
                TotalPages = (int)Math.Ceiling((double)totalTypeItems / pageSize),
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TypeItems = typeItems
            },
            Message = "Type Items retrieved successfully",
            IsSuccess = true,
            StatusCode = 200
        };
    }

    public async Task<ResponseDto> UpdateTypeItem(ClaimsPrincipal User, UpdateTypeItemDto updateTypeItemDto)
    {
        if (UserError.NotExists(User))
            return ErrorResponse.Build(StaticResponseMessage.User.NotFound, StaticOperationStatus.StatusCode.NotFound);

        if (updateTypeItemDto.ServiceTypeIdList == null || !updateTypeItemDto.ServiceTypeIdList.Any())
            return ErrorResponse.Build(message: "Empty service type list.", StaticOperationStatus.StatusCode.BadRequest);

        var typeItemsFromDb = await _unitOfWork.TypeItem.GetAllAsync(ti => ti.ServiceId == updateTypeItemDto.ServiceId);
        if (!typeItemsFromDb.Any())
            return ErrorResponse.Build(message: "Type Items not found.", StaticOperationStatus.StatusCode.NotFound);

        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var typeItemsToUpdate = updateTypeItemDto.ServiceTypeIdList.Select(st => new TypeItem
            {
                ServiceId = updateTypeItemDto.ServiceId,
                ServiceTypeId = st
            }).ToList();

            _unitOfWork.TypeItem.RemoveRange(typeItemsFromDb);
            await _unitOfWork.TypeItem.AddRangeAsync(typeItemsToUpdate);
            await _unitOfWork.SaveAsync();

            await transaction.CommitAsync();
            return SuccessResponse.Build(
                message: "Service types updated successfully.",
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: new
                {
                    ServiceId = updateTypeItemDto.ServiceId,
                    ServiceTypeIdList = updateTypeItemDto.ServiceTypeIdList
                });
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return ErrorResponse.Build(message: "An error occurred updating service types: " + e.Message, StaticOperationStatus.StatusCode.InternalServerError);
        }
    }
}