﻿using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly ApplicationDbContext _context; 
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
