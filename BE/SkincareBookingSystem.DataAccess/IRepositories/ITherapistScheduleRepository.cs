using System;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface ITherapistScheduleRepository : IRepository<TherapistSchedule>
{
    void Update(TherapistSchedule target, TherapistSchedule source);

    Task<TherapistSchedule?> GetTherapistScheduleByTherapistIdAsync(Guid therapistId, string? includeProperties = null);
}
