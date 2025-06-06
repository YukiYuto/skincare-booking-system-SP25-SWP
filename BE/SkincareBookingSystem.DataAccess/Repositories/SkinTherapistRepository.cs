﻿using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class SkinTherapistRepository : Repository<SkinTherapist>, ISkinTherapistRepository
    {
        private readonly ApplicationDbContext _context; 
        public SkinTherapistRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
