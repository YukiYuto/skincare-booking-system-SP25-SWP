namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface IUnitOfWork
{
    Task<int> SaveAsync();
}