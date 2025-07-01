using Management.BL.DTOs;
using Management.Core.Enums;

namespace Management.BL.Services.Abstractions;

public interface IRoomService
{
    Task<ICollection<RoomListDTO>> GetListItemsAsync(int page = 0, int count = 0);
    Task<ICollection<RoomTableDTO>> GetTableItemsAsync(RoomStatus? status = null, RoomType? type = null, string ? query = null, int page = 0, int count = 10);
    Task<RoomUpdateDTO> GetByIdForUpdateAsync(int id);
    Task ChangeRoomStatusAsync(int id, RoomStatus status);
    Task CreateAsync(RoomCreateDTO dto);
    Task UpdateAsync(RoomUpdateDTO dto);
    Task DeleteAsync(int id);
    Task<int> SaveChangesAsync();
}
