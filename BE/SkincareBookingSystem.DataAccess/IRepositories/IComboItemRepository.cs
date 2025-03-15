using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface IComboItemRepository : IRepository<ComboItem>
{
    public Task<(List<ComboItem> ComboItems, int TotalComboItems)> GetAllComboItemAsync
    (
        int pageNumber,
        int pageSize,
        string? filterOn,
        string? filterQuery,
        string? sortBy
    );
}