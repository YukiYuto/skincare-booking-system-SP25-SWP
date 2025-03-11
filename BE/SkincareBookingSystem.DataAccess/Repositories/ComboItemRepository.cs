using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories;

public class ComboItemRepository : Repository<ComboItem>, IComboItemRepository
{
    private readonly ApplicationDbContext _context;

    public ComboItemRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(List<ComboItem> ComboItems, int TotalComboItems)> GetAllComboItemAsync
    (
        int pageNumber,
        int pageSize,
        string? filterOn,
        string? filterQuery,
        string? sortBy
    )
    {
        var query = _context.ComboItem
            .Include(ci => ci.ServiceCombo)
            .Include(ci => ci.Services)
            .AsQueryable();

        // 🔹 Lọc dữ liệu
        if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            query = filterOn.Trim().ToLower() switch
            {
                "serviceid" => query.Where(ci => ci.ServiceId.ToString().Contains(filterQuery)),
                "servicecomboid" => query.Where(ci => ci.ServiceComboId.ToString().Contains(filterQuery)),
                _ => query
            };

        // 🔹 Sắp xếp
        if (!string.IsNullOrEmpty(sortBy))
        {
            var sortParams = sortBy.Trim().ToLower().Split('_');
            var sortField = sortParams[0];
            var sortDirection = sortParams.Length > 1 ? sortParams[1] : "asc";

            query = sortField switch
            {
                "priority" => sortDirection.Equals("desc")
                    ? query.OrderByDescending(ci => ci.Priority)
                    : query.OrderBy(ci => ci.Priority),
                _ => query.OrderBy(ci => ci.Priority)
            };
        }

        var totalComboItems = await query.CountAsync();
        var comboItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return (comboItems, totalComboItems);
    }
}