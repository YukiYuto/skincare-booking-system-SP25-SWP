using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface IServiceComboRepository : IRepository<ServiceCombo>
{
    Task<(List<ServiceCombo> ServiceCombos, int Total)> GetAll
    (
        int pageNumber,
        int pageSize,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        bool isManager = false
    );
}