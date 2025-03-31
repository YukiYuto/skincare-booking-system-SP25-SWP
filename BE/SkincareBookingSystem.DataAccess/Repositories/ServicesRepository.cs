using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class ServicesRepository : Repository<Services>, IServicesRepository
    {
        private readonly ApplicationDbContext _context;

        public ServicesRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(List<Services> Services, int TotalServices)> GetServicesAsync
        (
            int pageNumber,
            int pageSize,
            string? filterOn,
            string? filterQuery,
            string? sortBy,
            bool isManager = false,
            string? includeProperties = null
        )
        {
            var query = _context.Services.AsQueryable();

            if (!isManager)
            {
                query = query.Where(s => s.Status == StaticOperationStatus.Service.Active);
            }

            //Query
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                query = filterOn.ToLower() switch
                {
                    "servicename" => query.Where(s => s.ServiceName.Contains(filterQuery)),
                    "price" => double.TryParse(filterQuery, out double price)
                        ? query.Where(s => s.Price == price)
                        : query,
                    "service_type_id" => Guid.TryParse(filterQuery, out Guid serviceTypeId) 
                        ? query.Where(s => s.TypeItems.Any(t => t.ServiceTypeId == serviceTypeId)) 
                        : query,
                    _ => query
                };
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            int totalServices = await query.CountAsync();

            query = sortBy?.ToLower() switch
            {
                "servicename" => query.OrderBy(s => s.ServiceName),
                "servicename_desc" => query.OrderByDescending(s => s.ServiceName),
                "price" => query.OrderBy(s => s.Price),
                "price_desc" => query.OrderByDescending(s => s.Price),
                _ => query.OrderBy(s => s.ServiceName)
            };

            var services = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (services, totalServices);
        }

        public void Update(Services target, Services source)
        {
            _context.Set<Services>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}