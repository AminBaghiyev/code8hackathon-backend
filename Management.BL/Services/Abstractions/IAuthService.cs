using Management.BL.DTOs;

namespace Management.BL.Services.Abstractions;
public interface IAuthService
{
    Task<object> RegisterAsync(RegisterDTO dto);
    Task<object> LoginAsync(LoginDTO dto);
    Task ConfirmEmailAsync(ConfirmEmailDTO dto);
    Task SendResetPasswordAsync(string email);
    Task ResetPasswordAsync(ResetPasswordDTO dto);
    Task RefreshAccessTokenAsync();
    void Logout();
}
