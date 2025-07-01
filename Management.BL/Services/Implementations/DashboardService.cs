using Management.BL.DTOs;
using Management.BL.Services.Abstractions;
using Management.Core.Entities;
using Management.Core.Enums;
using Management.DL.Repositories.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Management.BL.Services.Implementations;

public class DashboardService : IDashboardService
{
    readonly IRepository<Room> _roomRepository;
    readonly UserManager<AppUser> _userManager;

    public DashboardService(IRepository<Room> roomRepository, UserManager<AppUser> userManager)
    {
        _roomRepository = roomRepository;
        _userManager = userManager;
    }

    public async Task<DashboardDTO> GetDashboardInfo()
    {
        IEnumerable<Room> fullRooms = await _roomRepository.GetAllAsync(e => e.Status == RoomStatus.Full);
        IEnumerable<Room> emptyRooms = await _roomRepository.GetAllAsync(e => e.Status == RoomStatus.Empty);
        IEnumerable<AppUser> customers = await _userManager.GetUsersInRoleAsync(Role.Customer.ToString());

        return new DashboardDTO
        {
            FullRoomCount = fullRooms.Count(),
            EmptyRoomCount = emptyRooms.Count(),
            CustomerCount = customers.Count(),
        };
    }
}
