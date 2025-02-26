using SkincareBookingSystem.Models.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.IServices
{
    public interface ISkinTherapistService
    {
        Task<ResponseDto> GetTherapistDetailsById(Guid therapistId);
        Task<ResponseDto> GetAllTherapists();
    }
}
