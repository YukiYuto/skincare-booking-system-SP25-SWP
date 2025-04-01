using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.ServiceDuration;

namespace SkincareBookingSystem.DataAccess.Repositories;

public class ServiceDurationRepository : Repository<ServiceDuration>, IServiceDurationRepository
{
    private readonly ApplicationDbContext _context;

    public ServiceDurationRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(List<GetServiceDurationDto> serviceDurations, int TotalServiceDurations)>
        GetAllServiceDurationAsync
        (
            int pageNumber,
            int pageSize,
            string? filterOn,
            string? filterQuery,
            string? sortOn,
            bool sortAscending
        )
    {
        var query = _context.ServiceDurations.AsQueryable();

        if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
        {
            filterQuery = filterQuery.Trim().ToLower();
            query = filterOn.ToLower() switch
            {
                "durationminutes" => query.Where(sd => sd.DurationMinutes.ToString().Contains(filterQuery)),
                _ => query
            };
        }

        var totalServiceDurations = await query.CountAsync();

        query = sortOn?.ToLower() switch
        {
            "durationminutes" => sortAscending
                ? query.OrderBy(sd => sd.DurationMinutes)
                : query.OrderByDescending(sd => sd.DurationMinutes),
            _ => sortAscending
                ? query.OrderBy(sd => sd.ServiceDurationId)
                : query.OrderByDescending(sd => sd.ServiceDurationId)
        };

        var serviceDurations = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var serviceDurationDtos = serviceDurations.Select(sd => new GetServiceDurationDto
        {
            ServiceDurationId = sd.ServiceDurationId,
            //DurationMinutes = sd.DurationMinutes,
            FormattedDuration = sd.GetFormattedDuration()
        }).ToList();

        return (serviceDurationDtos, totalServiceDurations);
    }
}