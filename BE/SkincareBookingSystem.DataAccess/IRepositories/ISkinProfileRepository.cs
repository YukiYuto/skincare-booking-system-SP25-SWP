using System;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface ISkinProfileRepository : IRepository<SkinProfile>
{
    void Update(SkinProfile target, SkinProfile source);
}
