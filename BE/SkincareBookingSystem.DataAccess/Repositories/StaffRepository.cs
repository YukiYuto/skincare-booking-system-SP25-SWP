using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories;

public class StaffRepository : Repository<Staff>, IStaffRepository
{
    private readonly ApplicationDbContext _context;
    
    public StaffRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<string> GetNextStaffCodeAsync()
    {
        
        // Lấy 2 số cuối của năm hiện tại
        int currentYear = DateTime.Now.Year % 100;
        string prefix = $"S{currentYear:D2}-";

        // Lấy danh sách các phần số của StaffCode (sau dấu gạch)
        var codeParts = await _context.Staff
            .Where(s => s.StaffCode.StartsWith(prefix))
            .Select(s => s.StaffCode.Substring(prefix.Length))
            .ToListAsync();

        // Chuyển đổi chuỗi sang số và tìm số lớn nhất
        int maxNumber = codeParts
            .Select(codePart =>
            {
                int number;
                return int.TryParse(codePart, out number) ? number : 0;
            })
            .DefaultIfEmpty(0)
            .Max();

        int nextNumber = maxNumber + 1;
        return $"{prefix}{nextNumber:D4}";
    }
}