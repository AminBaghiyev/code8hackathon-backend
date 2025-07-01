using Management.BL.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Management.API.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    readonly IDashboardService _service;

    public DashboardController(IDashboardService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboardInfo()
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _service.GetDashboardInfo());
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
