using System.Security.Claims;
using AutoMapper;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.ComboItem;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.Services.Services;

public class ComboItemService : IComboItemService
{
    private readonly IAutoMapperService _autoMapper;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ComboItemService(IUnitOfWork unitOfWork, IAutoMapperService autoMapper, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _autoMapper = autoMapper;
        _mapper = mapper;
    }

    public async Task<ResponseDto> CreateComboItem(ClaimsPrincipal user, CreateComboItemDto createComboItemDto)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return new ResponseDto
            {
                Message = "User not found",
                IsSuccess = false,
                StatusCode = 404
            };

        if (!createComboItemDto.ServicePriorityDtos.Any())
            return new ResponseDto
            {
                Message = "ServicePriorityDtos cannot be empty",
                IsSuccess = false,
                StatusCode = 400
            };

        var serviceIds = createComboItemDto.ServicePriorityDtos.Select(sp => sp.ServiceId).ToList();

        var services = await _unitOfWork.Services.GetAllAsync(s => serviceIds.Contains(s.ServiceId));

        if (services == null || !services.Any())
            return new ResponseDto
            {
                Message = "No valid services found",
                IsSuccess = false,
                StatusCode = 404
            };

        // Calculate total price of all services
        var totalPrice = services.Sum(s => s.Price);

        // Get the ServiceCombo to update its price
        var serviceCombo =
            (await _unitOfWork.ServiceCombo.GetAllAsync(sc => sc.ServiceComboId == createComboItemDto.ServiceComboId))
            .FirstOrDefault();

        if (serviceCombo == null)
            return new ResponseDto
            {
                Message = "Service combo not found",
                IsSuccess = false,
                StatusCode = 404
            };

        serviceCombo.Price = totalPrice;
        serviceCombo.NumberOfService = services.Count();

        var comboItems = createComboItemDto.ServicePriorityDtos.Select(sp => new ComboItem
        {
            ServiceComboId = createComboItemDto.ServiceComboId,
            ServiceId = sp.ServiceId,
            Priority = sp.Priority
        }).ToList();

        await _unitOfWork.ComboItem.AddRangeAsync(comboItems);
        _unitOfWork.ServiceCombo.Update(serviceCombo);
        await _unitOfWork.SaveAsync();

        var comboItemDtos = _autoMapper.MapCollection<ComboItem, GetComboItemDto>(comboItems);

        return new ResponseDto
        {
            Message = "Create Combo Item Successful",
            IsSuccess = true,
            StatusCode = 201,
            Result = new
            {
                ComboItems = comboItemDtos,
                TotalPrice = totalPrice
            }
        };
    }


    public async Task<ResponseDto> GetAllComboItem
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

        var (comboItems, totalComboItems) = await _unitOfWork.ComboItem.GetAllComboItemAsync
        (
            pageNumber,
            pageSize,
            filterOn,
            filterQuery,
            sortBy
        );

        if (!comboItems.Any())
            return new ResponseDto
            {
                Message = "No combo items found",
                IsSuccess = true,
                StatusCode = 200,
                Result = comboItems
            };

        var groupedComboItems = comboItems
            .GroupBy(ci => ci.ServiceComboId)
            .Select(group => new GetComboItemDto
            {
                ServiceComboId = group.Key,
                ServicePriorityDtos = group.Select(ci => new ServicePriorityDto
                {
                    ServiceId = ci.ServiceId,
                    Priority = ci.Priority
                }).ToList()
            }).ToList();

        return new ResponseDto
        {
            Message = "Retrieved combo items successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = new
            {
                TotalPage = (int)Math.Ceiling((double)totalComboItems / pageSize),
                TotalItems = totalComboItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = groupedComboItems
            }
        };
    }
}