using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.TypeItem;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface ITypeItemRepository : IRepository<TypeItem>
{
    Task<(List<GetTypeItemsDto> typeItems, int TotalTypeItems)> GetAllTypeItemAsync
    (
        int pageNumber,
        int pageSize,
        string? filterOn,
        string? filterQuery,
        string? sortBy
    );
}