using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.ServiceDuration;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface IServiceDurationRepository : IRepository<ServiceDuration>
{
    Task<(List<GetServiceDurationDto> serviceDurations, int TotalServiceDurations)> GetAllServiceDurationAsync
    (
        int pageNumber,
        int pageSize,
        string? filterOn,
        string? filterQuery,
        string? sortOn,
        bool sortAscending
    );
}