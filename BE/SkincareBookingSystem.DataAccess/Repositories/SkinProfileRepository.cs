using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class SkinProfileRepository : Repository<SkinProfile>, ISkinProfileRepository
    {
        private readonly ApplicationDbContext _context;
        public SkinProfileRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(SkinProfile target, SkinProfile source)
        {
            _context.Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
