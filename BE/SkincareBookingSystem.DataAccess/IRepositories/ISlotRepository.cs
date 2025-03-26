using System;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface ISlotRepository : IRepository<Slot>
{
    void Update(Slot target, Slot source);
}
