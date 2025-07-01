using Management.BL.DTOs;

namespace Management.BL.Services.Abstractions;

public interface IDashboardService
{
    Task<DashboardDTO> GetDashboardInfo();
}
