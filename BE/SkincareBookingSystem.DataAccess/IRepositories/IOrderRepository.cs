﻿using SkincareBookingSystem.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.DataAccess.IRepositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> GetOrderByOrderNumber(long orderNumber);
        Task<long> GenerateUniqueNumberAsync();
        void Update(Order target, Order source);
    }
}
