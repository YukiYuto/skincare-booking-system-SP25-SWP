using System;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface ITestAnswerRepository : IRepository<TestAnswer>
{
    void Update(TestAnswer target, TestAnswer source);
}
