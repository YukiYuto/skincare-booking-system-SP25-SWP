using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface IStaffRepository : IRepository<Staff>
{
    Task<string> GetNextStaffCodeAsync();
}