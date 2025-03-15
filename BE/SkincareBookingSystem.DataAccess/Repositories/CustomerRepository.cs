﻿using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
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

        public void Update(Customer target, Customer source)
        {
            _context.Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email, string? includeProperties = null)
        {
            var query = _context.Customers.AsQueryable();
            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(c => c.ApplicationUser != null && c.ApplicationUser.Email.Contains(email));
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
                return await query.FirstOrDefaultAsync();
        }

        public async Task<Customer?> GetCustomerByPhoneNumberAsync(string phoneNumber, string? includeProperties = null)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                query = query.Where(c => c.ApplicationUser!.PhoneNumber.Contains(phoneNumber));
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}
