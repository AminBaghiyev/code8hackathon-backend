using Management.BL.DTOs;
using Management.BL.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Management.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Admin,Customer")]
    [HttpGet("me")]
    public IActionResult Me()
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, new
            {
                name = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
                role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
            });
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

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        if (User.Identity.IsAuthenticated) return StatusCode(StatusCodes.Status409Conflict);

        try
        {
            return StatusCode(StatusCodes.Status201Created, await _service.RegisterAsync(registerDTO));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { errors = new Dictionary<string, string[]>
                {
                    {
                        "Error", new[] { ex.Message }
                    }
                }
            });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        if (User.Identity.IsAuthenticated) return StatusCode(StatusCodes.Status409Conflict);

        try
        {
            return StatusCode(StatusCodes.Status200OK, await _service.LoginAsync(loginDTO));
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

    [HttpGet("update-profile")]
    public async Task<IActionResult> GetUpdateProfile()
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _service.GetUpdateProfileAsync());
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

    [HttpPut("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateDTO dto)
    {
        try
        {
            await _service.UpdateProfileAsync(dto);
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

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDTO dto)
    {
        try
        {
            await _service.ConfirmEmailAsync(dto);
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

    [HttpGet("change-password")]
    public async Task<IActionResult> SendChangePassword([FromQuery] string email)
    {
        try
        {
            await _service.SendResetPasswordAsync(email);
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

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ResetPasswordDTO dto)
    {
        try
        {
            await _service.ResetPasswordAsync(dto);
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

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        try
        {
            await _service.RefreshAccessTokenAsync();
            return StatusCode(StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new
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

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        try
        {
            _service.Logout();
            return Ok(new { message = "Logged out successfully" });
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
