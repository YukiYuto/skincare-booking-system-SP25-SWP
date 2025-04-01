using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SkincareBookingSystem.Models.Dto.Appointment;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/appointment")]
    [Authorize]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // POST : api/Appointment
        [HttpPost]
        [Authorize(Roles = $"{StaticUserRoles.Customer},{StaticUserRoles.ManagerStaff}")]
        [SwaggerOperation(Summary = "API creates a new Appointment", Description = "Requires customer, staff roles")]
        public async Task<ActionResult<ResponseDto>> CreateAppointment([FromBody] CreateAppointmentDto appointmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid input data.",
                    Result = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            var result = await _appointmentService.CreateAppointment(User, appointmentDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [Authorize(Roles = $"{StaticUserRoles.Customer},{StaticUserRoles.ManagerStaff}")]
        [SwaggerOperation(Summary = "API gets all available Appointments", Description = "Requires customer, staff roles")]
        public async Task<ActionResult<ResponseDto>> GetAllAppointments()
        {
            var result = await _appointmentService.GetAllAppointments();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("customer/appointments")]
        [Authorize(Roles = $"{StaticUserRoles.Customer}")]
        [SwaggerOperation(Summary = "API gets all available Appointments by customer id", Description = "Requires customer, staff roles")]
        public async Task<ActionResult<ResponseDto>> GetAppointmentsByCustomer()
        {
            var result = await _appointmentService.GetAppointmentsByCustomer(User);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{appointmentId}")]
        [SwaggerOperation(Summary = "API gets an available Appointment by id", Description = "Requires customer, staff roles")]
        public async Task<ActionResult<ResponseDto>> GetAppointmentById(Guid appointmentId)
        {
            var result = await _appointmentService.GetAppointmentById(User, appointmentId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "API updates an available Appointment", Description = "Requires customer, staff roles")]
        public async Task<ActionResult<ResponseDto>> UpdateAppointment([FromBody] UpdateAppointmentDto appointmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid input data.",
                    Result = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            var result = await _appointmentService.UpdateAppointment(User, appointmentDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{appointmentId}")]
        [SwaggerOperation(Summary = "API soft deletes an available Appointment", Description = "Requires customer, staff roles")]
        public async Task<ActionResult<ResponseDto>> DeleteAppointment(Guid appointmentId)
        {
            var result = await _appointmentService.DeleteAppointment(User, appointmentId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
