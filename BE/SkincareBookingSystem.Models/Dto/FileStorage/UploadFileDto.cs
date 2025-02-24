﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.FileStorage
{
    public class UploadFileDto
    {
        public string AccessToken { get; set; } = null!;
        public IFormFile? File { get; set; }
    }
}
