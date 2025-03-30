using System;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface ISkinTestRepository : IRepository<SkinTest>
{
    void Update(SkinTest target, SkinTest source);
}
