using Management.BL.DTOs;

namespace Management.BL.Services.Abstractions;

public interface IReservationService
{
    Task<ICollection<ReservationListItemDTO>> GetListItemsAsync(int page = 0, int count = 0);
    Task<ICollection<ReservationTableItemDTO>> GetTableItemsByDateRangeAsync(DateTime startDate, DateTime endDate, int page = 0, int count = 10);
    Task<ReservationUpdateDTO> GetByIdForUpdateAsync(int id);
    Task CreateRoomReservation(ReservationCreateDTO dto);
    Task CreateRoomReservationByUser(ReservationCreateByUserDTO dto);
    Task<ICollection<ReservationTableItemForUserDTO>> GetReservationsForUser(DateTime startDate, DateTime endDate, int page = 0, int count = 10);
    Task UpdateAsync(ReservationUpdateDTO dto);
    Task DeleteAsync(int id);
    Task<int> SaveChangesAsync();
}
