using Management.BL.DTOs;
using Management.BL.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost("createRoom")]
        public async Task<IActionResult> Create([FromBody] RoomCreateDTO dto)
        {
            try
            {
                await _roomService.CreateAsync(dto);
                await _roomService.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errors = new Dictionary<string, string[]>
                {
                    {
                        "Error", new[] { ex.Message }
                    }
                }
                });
            }
        }

        [HttpGet("update/{id}")]
        public async Task<IActionResult> GetByIdForUpdate(int id)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await _roomService.GetByIdForUpdateAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errors = new Dictionary<string, string[]>
                {
                    {
                        "Error", new[] { ex.Message }
                    }
                }
                });
            }
        }

        [HttpPut("updateRoom")]
        public async Task<IActionResult> Update([FromBody] RoomUpdateDTO dto, int id)
        {
            try
            {
                await _roomService.UpdateAsync(dto, id);
                await _roomService.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errors = new Dictionary<string, string[]>
                {
                    {
                        "Error", new[] { ex.Message }
                    }
                }
                });
            }
        }

        [HttpGet("roomTable")]
        public async Task<IActionResult> GetTableItems([FromQuery] int page = 0, int count = 10)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await _roomService.GetTableItemsAsync(page, count));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errors = new Dictionary<string, string[]>
                {
                    {
                        "Error", new[] { ex.Message }
                    }
                }
                });
            }
        }

        [HttpGet("roomlist")]
        public async Task<IActionResult> GetListItems([FromQuery] int page = 0, int count = 0)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await _roomService.GetListItemsAsync(page, count));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errors = new Dictionary<string, string[]>
                {
                    {
                        "Error", new[] { ex.Message }
                    }
                }
                });
            }
        }

        [HttpDelete("deleteRoom/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _roomService.DeleteAsync(id);
                await _roomService.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errors = new Dictionary<string, string[]>
                {
                    {
                        "Error", new[] { ex.Message }
                    }
                }
                });
            }
        }
    }
}
