using AutoMapper;
using Management.BL.DTOs;
using Management.BL.Services.Abstractions;
using Management.Core.Entities;
using Management.DL.Repositories.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Management.BL.Services.Implementations;

public class ReservationService : IReservationService
{
    readonly IRepository<Reservation> _repository;
    readonly UserManager<AppUser> _userManager;
    readonly IHttpContextAccessor _httpContextAccessor;
    readonly IMapper _mapper;
    readonly IRepository<Room> _roomRepository;
    readonly IRepository<Service> _serviceRepository;

    public ReservationService(IRepository<Reservation> repository, UserManager<AppUser> userManager, IMapper mapper, IRepository<Room> roomRepository, IRepository<Service> serviceRepository, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _userManager = userManager;
        _mapper = mapper;
        _roomRepository = roomRepository;
        _serviceRepository = serviceRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task CreateRoomReservation(ReservationCreateDTO dto)
    {
        AppUser? user = await _userManager.FindByIdAsync(dto.CustomerId);
        if (user is null) throw new Exception("User is not found");

        Room? room = await _roomRepository.GetOneAsync(r => r.Id == dto.RoomId);
        if (room is null) throw new Exception("Room not found");

        var overlappingReservations = await _repository.GetOneAsync(
            r => r.RoomId == dto.RoomId &&
            (
                (dto.CheckInDate >= r.CheckInDate && dto.CheckInDate < r.CheckOutDate) ||
                (dto.CheckOutDate > r.CheckInDate && dto.CheckOutDate <= r.CheckOutDate) ||
                (dto.CheckInDate <= r.CheckInDate && dto.CheckOutDate >= r.CheckOutDate)
            )
        );

        if (overlappingReservations is not null) throw new Exception("Room is already booked for the selected dates.");

        var reservation = _mapper.Map<Reservation>(dto);
        reservation.CustomerId = user.Id;
        reservation.RoomId = room.Id;

        await _repository.CreateAsync(reservation);

        if (dto.Services is not null && dto.Services.Count != 0)
        {
            var services = _mapper.Map<ICollection<Service>>(dto.Services);

            foreach (var service in services)
            {
                service.Reservation = reservation;
                await _serviceRepository.CreateAsync(service);
            }
        }
    }

    public async Task CreateRoomReservationByUser(ReservationCreateByUserDTO dto)
    {
        string userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.SerialNumber)?.Value ?? throw new Exception("Unauthorized");

        AppUser? user = await _userManager.FindByIdAsync(userId);
        if (user is null) throw new Exception("User is not found");

        Room? room = await _roomRepository.GetOneAsync(r => r.Id == dto.RoomId);
        if (room is null) throw new Exception("Room not found");

        var overlappingReservations = await _repository.GetOneAsync(
            r => r.RoomId == dto.RoomId &&
            (
                (dto.CheckInDate >= r.CheckInDate && dto.CheckInDate < r.CheckOutDate) ||
                (dto.CheckOutDate > r.CheckInDate && dto.CheckOutDate <= r.CheckOutDate) ||
                (dto.CheckInDate <= r.CheckInDate && dto.CheckOutDate >= r.CheckOutDate)
            )
        );

        if (overlappingReservations is not null) throw new Exception("Room is already booked for the selected dates.");

        var reservation = _mapper.Map<Reservation>(dto);
        reservation.CustomerId = user.Id;
        reservation.RoomId = room.Id;

        await _repository.CreateAsync(reservation);

        if (dto.Services is not null && dto.Services.Count != 0)
        {
            var services = _mapper.Map<ICollection<Service>>(dto.Services);

            foreach (var service in services)
            {
                service.Reservation = reservation;
                await _serviceRepository.CreateAsync(service);
            }
        }
    }

    public async Task<ICollection<ReservationTableItemDTO>> GetTableItemsByDateRangeAsync(DateTime startDate, DateTime endDate, int page = 0, int count = 10)
    {
        if (startDate == default) startDate = new DateTime(1900, 1, 1);
        if (endDate == default) endDate = new DateTime(2100, 1, 1);

        var reservations = await _repository.GetAllAsync(
            predicate: r => r.CheckInDate.Date >= startDate.Date && r.CheckOutDate.Date <= endDate.Date,
            includes: q => q.Include(r => r.Customer).Include(r => r.Room),
            page: page,
            count: count
        );

        var dtoList = _mapper.Map<ICollection<ReservationTableItemDTO>>(reservations);
        return dtoList;
    }

    public async Task<ICollection<ReservationTableItemForUserDTO>> GetReservationsForUser(DateTime startDate, DateTime endDate, int page = 0, int count = 10)
    {
        string userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.SerialNumber)?.Value ?? throw new Exception("Unauthorized");

        AppUser? user = await _userManager.FindByIdAsync(userId);
        if (user is null) throw new Exception("User is not found");

        if (startDate == default) startDate = new DateTime(1900, 1, 1);
        if (endDate == default) endDate = new DateTime(2100, 1, 1);

        var reservations = await _repository.GetAllAsync(
            predicate: r => r.CheckInDate.Date >= startDate.Date && r.CheckOutDate.Date <= endDate.Date && r.CustomerId == user.Id,
            includes: q => q.Include(r => r.Customer).Include(r => r.Room),
            page: page,
            count: count
        );

        var dtoList = _mapper.Map<ICollection<ReservationTableItemForUserDTO>>(reservations);
        return dtoList;
    }

    public async Task<ReservationUpdateDTO> GetByIdForUpdateAsync(int id)
    {
        Reservation? reservation = await _repository.GetOneAsync(r => r.Id == id);
        if (reservation == null) throw new Exception("Reservation not found");

        var updateDto = _mapper.Map<ReservationUpdateDTO>(reservation);
        return updateDto;
    }

    public async Task<ICollection<ReservationListItemDTO>> GetListItemsAsync(int page = 0, int count = 0)
    {
        ICollection<Reservation> reservations = await _repository.GetAllAsync(page: page, count: count);

        return _mapper.Map<ICollection<ReservationListItemDTO>>(reservations);
    }

    public async Task UpdateAsync(ReservationUpdateDTO dto)
    {
        Reservation? reservation = await _repository.GetOneAsync(e => e.Id == dto.Id);
        if (reservation is null) throw new Exception("Reservation not found");

        _mapper.Map(dto, reservation); 
        _repository.Update(reservation);
    }

    public async Task DeleteAsync(int id)
    {
        Reservation? reservation = await _repository.GetOneAsync(r => r.Id == id);
        if (reservation is null) throw new Exception("Reservation not found");

        _repository.Delete(reservation);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _repository.SaveChangesAsync();
    }
}
