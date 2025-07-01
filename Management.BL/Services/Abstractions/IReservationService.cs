using Management.BL.DTOs;
using Management.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.BL.Services.Abstractions
{
    public interface IReservationService
    {
        Task<ICollection<ReservationListItemDTO>> GetListItemsAsync(int page = 0, int count = 0);
        Task<ICollection<ReservationTableItemDTO>> GetTableItemsByDateRangeAsync(DateTime startDate, DateTime endDate, int page = 0, int count = 10);
        Task CreateRoomReservation(ReservationCreateDTO dto);
        Task<ReservationUpdateDTO> GetByIdForUpdateAsync(int id);
        Task UpdateAsync(ReservationUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<int> SaveChangesAsync();


    }
}
