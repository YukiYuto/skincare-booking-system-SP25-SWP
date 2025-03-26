﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.BlogCategories
{
    public class UpdateBlogCategoryDto
    {
        public Guid BlogCategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
