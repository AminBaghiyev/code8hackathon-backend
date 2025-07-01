using Management.BL.DTOs;

namespace Management.BL.Services.Abstractions;

public interface IUserService
{
    Task<ICollection<UserListItemDTO>> GetListItemsAsync(int page = 0, int count = 0);
    Task<ICollection<UserTableItemDTO>> GetTableItemsAsync(string? q = null, int page = 0, int count = 10);
    Task<UserUpdateDTO> GetByEmailForUpdateAsync(string email);
    Task CreateAsync(UserCreateDTO dto);
    Task UpdateAsync(UserUpdateDTO dto);
    Task DeleteAsync(string email);
}
