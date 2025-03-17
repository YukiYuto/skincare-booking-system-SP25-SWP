using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.DurationItem;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface IDurationItemRepository : IRepository<DurationItem>
{
    Task<(List<GetDurationItemDto> durationItemsDto, int TotalDurationItems)> GetAllDurationItemsAsync
    (
        int pageNumber,
        int pageSize
    );
}