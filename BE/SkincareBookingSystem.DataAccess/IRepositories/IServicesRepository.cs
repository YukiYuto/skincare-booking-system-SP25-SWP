using System;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface IServicesRepository : IRepository<Services>
{
    Task<(List<Services> Services, int TotalServices)> GetServicesAsync
    (
        int pageNumber,
        int pageSize,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        bool isManager = false
    );

    void Update(Services target, Services source);
}