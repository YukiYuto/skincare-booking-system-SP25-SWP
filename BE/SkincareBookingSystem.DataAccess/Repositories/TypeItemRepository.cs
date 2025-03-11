using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Services;
using SkincareBookingSystem.Models.Dto.TypeItem;

namespace SkincareBookingSystem.DataAccess.Repositories;

public class TypeItemRepository : Repository<TypeItem>, ITypeItemRepository
{
    private readonly ApplicationDbContext _context;

    public TypeItemRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(List<GetTypeItemsDto> typeItems, int TotalTypeItems)> GetAllTypeItemAsync
    (
        int pageNumber,
        int pageSize,
        string? filterOn,
        string? filterQuery,
        string? sortBy
    )
    {
        var query = _context.TypeItem
            .Include(ti => ti.ServiceType)
            .ThenInclude(st => st.TypeItems)
            .ThenInclude(ti => ti.Services)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
        {
            filterQuery = filterQuery.Trim().ToLower();
            query = filterOn.ToLower() switch
            {
                "servicename" => query.Where(ti => ti.ServiceType.TypeItems
                    .Any(s => s.Services.ServiceName.ToLower().Contains(filterQuery))),
                "servicetypename" => query.Where(ti => ti.ServiceType.ServiceTypeName.ToLower().Contains(filterQuery)),
                _ => query
            };
        }

        var totalTypeItems = await query.CountAsync();

        // Sắp xếp dữ liệu
        query = sortBy?.ToLower() switch
        {
            "service_name" => query.OrderBy(ti => ti.ServiceType.TypeItems.FirstOrDefault().Services.ServiceName),
            "service_name_desc" => query.OrderByDescending(ti =>
                ti.ServiceType.TypeItems.FirstOrDefault().Services.ServiceName),
            "service_type_name" => query.OrderBy(ti => ti.ServiceType.ServiceTypeName),
            "service_type_name_desc" => query.OrderByDescending(ti => ti.ServiceType.ServiceTypeName),
            _ => query.OrderBy(ti => ti.ServiceType.ServiceTypeName)
        };

        var typeItems = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(ti => new GetTypeItemsDto
            {
                ServiceTypeId = ti.ServiceTypeId,
                ServiceTypeName = ti.ServiceType.ServiceTypeName,
                Services = ti.ServiceType.TypeItems
                    .Select(s => new ServiceDto
                    {
                        ServiceId = s.Services.ServiceId,
                        ServiceName = s.Services.ServiceName
                    }).Distinct().ToList()
            })
            .ToListAsync();

        return (typeItems, totalTypeItems);
    }
}