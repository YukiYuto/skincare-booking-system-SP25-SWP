using System;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface ITestQuestionRepository : IRepository<TestQuestion>
{
    void Update(TestQuestion target, TestQuestion source);
}
