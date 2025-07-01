using Management.BL.DTOs;
using Management.BL.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Management.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationController : ControllerBase
{
    readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("list")]
    public async Task<IActionResult> GetListItems([FromQuery] int page = 0, int count = 0)
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _reservationService.GetListItemsAsync(page, count));
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
    public async Task<IActionResult> Update([FromBody] ReservationUpdateDTO dto)
    {
        try
        {
            await _reservationService.UpdateAsync(dto);
            await _reservationService.SaveChangesAsync();
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
    [HttpPost("add")]
    public async Task<IActionResult> Create([FromBody] ReservationCreateDTO dto)
    {
        try
        {
            await _reservationService.CreateRoomReservation(dto);
            await _reservationService.SaveChangesAsync();
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

    [Authorize(Roles = "Customer")]
    [HttpPost("add-reservation")]
    public async Task<IActionResult> AddReservation([FromBody] ReservationCreateByUserDTO dto)
    {
        try
        {
            await _reservationService.CreateRoomReservationByUser(dto);
            await _reservationService.SaveChangesAsync();
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
    [HttpGet("table")]
    public async Task<IActionResult> GetReservationsByDateRange([FromQuery] DateTime startDate, DateTime endDate, int page = 0, int count = 10)
    {
        try
        {
            var result = await _reservationService.GetTableItemsByDateRangeAsync(startDate, endDate, page, count);
            return Ok(result);
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

    [Authorize(Roles = "Customer")]
    [HttpGet("my-reservations")]
    public async Task<IActionResult> GetMyReservationsByDateRange([FromQuery] DateTime startDate, DateTime endDate, int page = 0, int count = 10)
    {
        try
        {
            var result = await _reservationService.GetReservationsForUser(startDate, endDate, page, count);
            return Ok(result);
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
            await _reservationService.DeleteAsync(id);
            await _reservationService.SaveChangesAsync(); 
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
    [HttpGet("update/{id}")]
    public async Task<IActionResult> GetByIdForUpdate(int id)
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _reservationService.GetByIdForUpdateAsync(id));
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
