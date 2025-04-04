﻿using SkincareBookingSystem.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.DataAccess.IRepositories
{
    public interface IFeedbacksRepository : IRepository<Feedbacks>
    {
        void Update(Feedbacks target, Feedbacks source);
    }
}
