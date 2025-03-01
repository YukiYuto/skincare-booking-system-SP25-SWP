using System;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface IServicesRepository : IRepository<Services>
{

    void Update(Services target, Services source);

}
