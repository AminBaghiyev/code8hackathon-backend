using Management.BL.DTOs;
using Management.BL.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Management.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet("update/{email}")]
    public async Task<IActionResult> GetByEmailForUpdate(string email)
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _service.GetByEmailForUpdateAsync(email));
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

    [HttpGet("table")]
    public async Task<IActionResult> GetTableItems([FromQuery] string? q = null, int page = 0, int count = 0)
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _service.GetTableItemsAsync(q, page, count));
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

    [HttpGet("list")]
    public async Task<IActionResult> GetListItems([FromQuery] int page = 0, int count = 0)
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _service.GetListItemsAsync(page, count));
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

    [HttpPost("add")]
    public async Task<IActionResult> Create([FromBody] UserCreateDTO dto)
    {
        try
        {
            await _service.CreateAsync(dto);
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

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UserUpdateDTO dto)
    {
        try
        {
            await _service.UpdateAsync(dto);
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

    [HttpDelete("delete/{email}")]
    public async Task<IActionResult> Delete(string email)
    {
        try
        {
            await _service.DeleteAsync(email);
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
