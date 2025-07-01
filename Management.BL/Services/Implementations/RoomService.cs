using AutoMapper;
using Management.BL.DTOs;
using Management.BL.Services.Abstractions;
using Management.Core.Entities;
using Management.DL.Repositories.Abstractions;
using Microsoft.AspNetCore.Identity;
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

    public async Task CreateAsync(RoomCreateDTO dto)
    {
        Room? room = await _repository.GetOneAsync(item => item.Number == dto.Number);

        if (room is not null) throw new Exception($"{dto.Number} is existed");

        Room newRoom = _mapper.Map<Room>(dto);

        await _repository.CreateAsync(newRoom);
    }

    public async Task UpdateAsync(RoomUpdateDTO dto, int id)
    {
        Room? room = await _repository.GetOneAsync(item => item.Id == dto.Id);
        if (room is null) throw new Exception("Room not found.");

        _mapper.Map(dto, room);
        _repository.Update(room);
    }

    public async Task DeleteAsync(int id)
    {
        Room room = await _repository.GetOneAsync(item => item.Id == id) ?? throw new Exception("Room not found.");
        _repository.Delete(room);
    }

    public async Task<Room> GetByIdAsync(int id)
    {
        Room? room = await _repository.GetOneAsync(item => item.Id == id, includes: e => e.Include(item => item.Reservations));

        if (room is null)
        {
            throw new Exception("Room not found.");
        }

        return room;
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
        ICollection<Room> rooms = await _repository.GetAllAsync(item=>item.Status == Core.Enums.RoomStatus.Full ,page: page,count: count,orderAsc: true
        );

        ICollection<RoomListDTO> listDtos = _mapper.Map<ICollection<RoomListDTO>>(rooms);
        return listDtos;
    }

    public Task<ICollection<RoomTableDTO>> GetTableItemsAsync(int page = 0, int count = 10)
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveChangesAsync()
    {
        return _repository.SaveChangesAsync();
        throw new NotImplementedException();
    }
}
