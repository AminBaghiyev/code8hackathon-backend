using AutoMapper;
using Management.BL.DTOs;
using Management.BL.Services.Abstractions;
using Management.BL.Utilities;
using Management.Core.Entities;
using Management.Core.Enums;
using Management.DL.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Management.BL.Services.Implementations;

public class RoomService : IRoomService
{
    readonly IRepository<Room> _repository;
    readonly IMapper _mapper;

    public RoomService(IRepository<Room> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task ChangeRoomStatusAsync(int id, RoomStatus status)
    {
        Room? room = await _repository.GetOneAsync(item => item.Id == id);
        if (room is null) throw new Exception("Room not found.");
        if (room.Status == status) throw new Exception($"Status is already {status}");

        room.Status = status;
        _repository.Update(room);
    }

    public async Task CreateAsync(RoomCreateDTO dto)
    {
        Room? room = await _repository.GetOneAsync(item => item.Number == dto.Number);
        if (room is not null) throw new Exception($"{dto.Number} is existed");

        Room newRoom = _mapper.Map<Room>(dto);

        newRoom.Thumbnail = await dto.File.SaveAsync("rooms");

        await _repository.CreateAsync(newRoom);
    }

    public async Task UpdateAsync(RoomUpdateDTO dto)
    {
        Room? room = await _repository.GetOneAsync(item => item.Id == dto.Id);
        if (room is null) throw new Exception("Room not found.");
        Room newRoom = _mapper.Map<Room>(dto);

        newRoom.Thumbnail = dto.File is not null ? await dto.File.SaveAsync("rooms") : room.Thumbnail;

        _repository.Update(newRoom);

        if (dto.File is not null) File.Delete(Path.Combine(Path.GetFullPath("wwwroot"), "uploads", "rooms", room.Thumbnail));
    }

    public async Task DeleteAsync(int id)
    {
        Room room = await _repository.GetOneAsync(item => item.Id == id) ?? throw new Exception("Room not found.");
        _repository.Delete(room);
        File.Delete(Path.Combine(Path.GetFullPath("wwwroot"), "uploads", "rooms", room.Thumbnail));
    }

    public async Task<RoomUpdateDTO> GetByIdForUpdateAsync(int id)
    {
        Room? room = await _repository.GetOneAsync(item => item.Id == id);

        if (room is null) throw new Exception("Room not found.");

        RoomUpdateDTO dto = _mapper.Map<RoomUpdateDTO>(room);

        return dto;
    }

    public async Task<ICollection<RoomListDTO>> GetListItemsAsync(int page = 0, int count = 0)
    {
        ICollection<Room> rooms = await _repository.GetAllAsync(item => item.Status == RoomStatus.Empty , page, count);

        return _mapper.Map<ICollection<RoomListDTO>>(rooms);
    }

    public async Task<ICollection<RoomTableDTO>> GetTableItemsAsync(RoomStatus? status = null, RoomType? type = null, string? q = null, int page = 0, int count = 10)
    {
        IQueryable<Room> query = _repository.Table.AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            string normalizedQ = q.Trim().ToLower();
            query = query.Where(r => r.Number.ToString().Contains(normalizedQ));
        }

        if (status.HasValue) query = query.Where(r => r.Status == status.Value);
        if (type.HasValue) query = query.Where(r => r.Type == type.Value);

        if (count > 0) query = query.Skip(page * count).Take(count);

        return _mapper.Map<ICollection<RoomTableDTO>>(await query.ToListAsync());
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _repository.SaveChangesAsync();
    }
}
