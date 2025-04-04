﻿using SkincareBookingSystem.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.DataAccess.IRepositories
{
    public interface IBlogRepository : IRepository<Blog>
    {
        void Update(Blog target, Blog source);
        //Task<Blog> GetAsync(Func<Blog, bool> filter, params string[] includeProperties);
        //Task<IEnumerable<Blog>> GetAllAsync(Func<Blog, bool> filter, params string[] includeProperties);
    }
}
