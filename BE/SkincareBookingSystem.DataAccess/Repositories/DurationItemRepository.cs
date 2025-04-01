using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.DurationItem;

namespace SkincareBookingSystem.DataAccess.Repositories;

public class DurationItemRepository : Repository<DurationItem>, IDurationItemRepository
{
    private readonly ApplicationDbContext _context;

    public DurationItemRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(List<GetDurationItemDto> durationItemsDto, int TotalDurationItems)> GetAllDurationItemsAsync
    (
        int pageNumber,
        int pageSize
    )
    {
        var query = _context.DurationItem.AsQueryable();

        var totalDurationItems = await query.CountAsync();

        // 🔹 Phân trang và lấy dữ liệu
        var durationItems = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(di => new GetDurationItemDto
            {
                ServiceId = di.ServiceId,
                ServiceDurationId = di.ServiceDurationId
            })
            .ToListAsync();

        return (durationItems, totalDurationItems);
    }
}