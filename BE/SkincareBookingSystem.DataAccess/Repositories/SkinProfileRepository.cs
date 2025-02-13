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
        public SkinProfileRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
