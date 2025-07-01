using Management.BL.DTOs;
using Management.BL.Services.Abstractions;
using Management.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Management.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomController : ControllerBase
{
    readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("add")]
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

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] RoomUpdateDTO dto)
    {
        try
        {
            await _roomService.UpdateAsync(dto);
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

    [Authorize]
    [HttpGet("table")]
    public async Task<IActionResult> GetTableItems([FromQuery] RoomStatus? status = null, RoomType? type = null, string? q = null, int page = 0, int count = 10)
    {
        try
        {
            var result = await _roomService.GetTableItemsAsync(status, type, q, page, count);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                errors = new Dictionary<string, string[]>
        {
            { "Error", new[] { ex.Message } }
        }
            });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("change-status/{id}")]
    public async Task<IActionResult> ChangeStatus(int id, [FromQuery] RoomStatus status)
    {
        try
        {
            await _roomService.ChangeRoomStatusAsync(id, status);
            await _roomService.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK);
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

    [Authorize(Roles = "Admin")]
    [HttpGet("list")]
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

    [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _roomService.DeleteAsync(id);
            await _roomService.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK);

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
