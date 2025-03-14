using SkincareBookingSystem.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.DataAccess.IRepositories
{
    public interface IAppointmentsRepository : IRepository<Appointments>
    {
        void Update(Appointments target, Appointments source);

        Task<List<Appointments>> GetAppointmentsByDateAsync(Guid customerId, DateTime date, string? includeProperties = null);
    }
}
