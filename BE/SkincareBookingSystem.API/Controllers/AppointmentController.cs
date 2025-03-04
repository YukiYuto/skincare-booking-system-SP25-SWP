﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SkincareBookingSystem.Models.Dto.Appointment;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/appointment")]
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
        public async Task<ActionResult<ResponseDto>> GetAllAppointments()
        {
            var result = await _appointmentService.GetAllAppointments();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{customerId}/appointments")]
        public async Task<ActionResult<ResponseDto>> GetAppointmentsByCustomerId(Guid customerId)
        {
            var result = await _appointmentService.GetAppointmentsByCustomerId(customerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{appointmentId}")]
        public async Task<ActionResult<ResponseDto>> GetAppointmentById(Guid appointmentId)
        {
            var result = await _appointmentService.GetAppointmentById(User, appointmentId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
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
        public async Task<ActionResult<ResponseDto>> DeleteAppointment(Guid appointmentId)
        {
            var result = await _appointmentService.DeleteAppointment(User, appointmentId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
