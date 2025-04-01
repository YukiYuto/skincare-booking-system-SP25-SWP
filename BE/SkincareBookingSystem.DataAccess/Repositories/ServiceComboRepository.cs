using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.DataAccess.Repositories;

public class ServiceComboRepository : Repository<ServiceCombo>, IServiceComboRepository
{
    private readonly ApplicationDbContext _context;

    public ServiceComboRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(List<ServiceCombo> ServiceCombos, int Total)> GetAll
    (
        int pageNumber,
        int pageSize,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        bool isManager = false
    )
    {
        var query = _context.ServiceCombo.AsQueryable();

        // Chỉ lọc nếu không phải Manager
        if (!isManager) query = query.Where(sc => sc.Status == StaticOperationStatus.Service.Active);

        // Lọc dữ liệu theo filterOn
        if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            query = filterOn.ToLower() switch
            {
                "servicename" => query.Where(sc => sc.ComboName.Contains(filterQuery)),
                "price" => double.TryParse(filterQuery, out var price)
                    ? query.Where(sc => sc.Price == price)
                    : query,
                _ => query
            };

        var total = await query.CountAsync();

        // Sắp xếp dữ liệu
        query = sortBy?.ToLower() switch
        {
            "servicename" => query.OrderBy(sc => sc.ComboName),
            "servicename_desc" => query.OrderByDescending(sc => sc.ComboName),
            "price" => query.OrderBy(sc => sc.Price),
            "price_desc" => query.OrderByDescending(sc => sc.Price),
            _ => query.OrderBy(sc => sc.ComboName)
        };

        var serviceCombos = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (serviceCombos, total);
    }

    public void Update(ServiceCombo serviceCombo)
    {
        _context.ServiceCombo.Update(serviceCombo);
    }
}