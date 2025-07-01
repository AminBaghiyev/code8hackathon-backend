using Management.BL.DTOs;
using Management.Core.Entities;

namespace Management.BL.Services.Abstractions;

public interface IRoomService
{
    Task<ICollection<RoomListDTO>> GetListItemsAsync(int page = 0, int count = 0);
    Task<ICollection<RoomTableDTO>> GetTableItemsAsync(int page = 0, int count = 10);
    Task<RoomUpdateDTO> GetByIdForUpdateAsync(int id);
    Task<Room> GetByIdAsync(int id);
    Task CreateAsync(RoomCreateDTO dto);
    Task UpdateAsync(RoomUpdateDTO dto, int id);
    Task DeleteAsync(int id);
    Task<int> SaveChangesAsync();
}
