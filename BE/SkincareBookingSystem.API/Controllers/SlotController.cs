using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.Slots;
using SkincareBookingSystem.Services.IServices;
using System;
using System.Threading.Tasks;

namespace SkincareBookingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlotController : ControllerBase
    {
        private readonly ISlotService _slotService;

        public SlotController(ISlotService slotService)
        {
            _slotService = slotService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSlot([FromBody] CreateSlotDto createSlotDto)
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

            var result = await _slotService.CreateSlot(User, createSlotDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<ResponseDto>> GetSlotById(Guid id)
        {
            var result = await _slotService.GetSlotById(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("all")]
        public async Task<ActionResult<ResponseDto>> GetAllSlots()
        {
            var result = await _slotService.GetAllSlots();
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<ActionResult<ResponseDto>> UpdateSlot([FromBody] UpdateSlotDto updateSlotDto)
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

            var result = await _slotService.UpdateSlot(User, updateSlotDto);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<ResponseDto>> DeleteSlot(Guid id)
        {
            var result = await _slotService.DeleteSlot(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}
