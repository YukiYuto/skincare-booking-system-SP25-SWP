﻿using SkincareBookingSystem.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.DataAccess.IRepositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        void Update (Customer target, Customer source);
    }
}
