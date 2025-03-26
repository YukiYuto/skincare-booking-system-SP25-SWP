using System;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface IServiceTypeRepository : IRepository<ServiceType>
{
    void Update(ServiceType target, ServiceType source);
}
