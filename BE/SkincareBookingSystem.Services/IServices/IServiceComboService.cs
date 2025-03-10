﻿using System.Security.Claims;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.ServiceCombo;

namespace SkincareBookingSystem.Services.IServices;

public interface IServiceComboService
{
    Task<ResponseDto> CreateServiceCombo(ClaimsPrincipal user, CreateServiceComboDto createServiceComboDto);
    Task<ResponseDto> UpdateServiceCombo(ClaimsPrincipal user, UpdateServiceComboDto updateServiceComboDto);
    Task<ResponseDto> GetAllServiceCombos(ClaimsPrincipal user);
    Task<ResponseDto> GetServiceComboById(ClaimsPrincipal user, Guid serviceComboId);
    Task<ResponseDto> DeleteServiceCombo(ClaimsPrincipal user, Guid serviceComboId);
}