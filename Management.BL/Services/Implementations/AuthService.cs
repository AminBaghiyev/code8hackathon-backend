using AutoMapper;
using Management.BL.DTOs;
using Management.BL.Services.Abstractions;
using Management.BL.Utilities;
using Management.Core.Entities;
using Management.Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Management.BL.Services.Implementations;
public class AuthService : IAuthService
{
    readonly UserManager<AppUser> _userManager;
    readonly SignInManager<AppUser> _signInManager;
    readonly IHttpContextAccessor _httpContextAccessor;
    readonly IMapper _mapper;
    readonly JWTService _service;
    readonly EmailService _emailService;

    public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, IHttpContextAccessor httpContextAccessor, JWTService service, EmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _service = service;
        _emailService = emailService;
    }

    public async Task<object> RegisterAsync(RegisterDTO dto)
    {
        if (await _userManager.FindByEmailAsync(dto.Email) is not null) throw new Exception("An account already exists with this email");
        if (await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber) is not null) throw new Exception("An account already exists with this phone number");

        AppUser user = _mapper.Map<AppUser>(dto);
        user.UserName = user.Email;

        IdentityResult result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded) throw new Exception("User could not be created");

        result = await _userManager.AddToRoleAsync(user, Role.Customer.ToString());
        if (!result.Succeeded) throw new Exception("Could not add user to role");

        IEnumerable<Claim> claims =
        [
            new Claim(ClaimTypes.Role, Role.Customer.ToString()),
            new Claim(ClaimTypes.SerialNumber, user.Id),
            new Claim(ClaimTypes.GivenName, user.FullName),
            new Claim(ClaimTypes.Email, user.Email)
        ];

        string accessToken = _service.GenerateToken(TimeSpan.FromHours(1), claims);
        string refreshToken = _service.GenerateToken(TimeSpan.FromDays(7), [new Claim(ClaimTypes.Email, user.Email)]);

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("access_token", accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(1)
        });

        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        await _emailService.SendTokenAsync(
            user.Email, user.FullName,
            "ConfirmEmailTemplate.cshtml", "Confirm Email",
            $"https://code8hackathon.duckdns.org/user/confirm-email?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}"
        );

        return new
        {
            name = user.FullName,
            role = Role.Customer.ToString()
        };
    }

    public async Task<object> LoginAsync(LoginDTO dto)
    {
        AppUser user = await _userManager.FindByEmailAsync(dto.Email) ?? throw new Exception("User not found");
        if (!user.EmailConfirmed) throw new Exception("Email must be confirmed");
        SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded) throw new Exception("Username or password is wrong");

        IEnumerable<string> roles = await _userManager.GetRolesAsync(user);

        IEnumerable<Claim> claims =
        [
            new Claim(ClaimTypes.Role, roles.First()),
            new Claim(ClaimTypes.SerialNumber, user.Id),
            new Claim(ClaimTypes.GivenName, user.FullName),
            new Claim(ClaimTypes.Email, user.Email)
        ];

        string accessToken = _service.GenerateToken(TimeSpan.FromHours(1), claims);
        string refreshToken = _service.GenerateToken(TimeSpan.FromDays(7), [new Claim(ClaimTypes.Email, user.Email)]);

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("access_token", accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(1)
        });

        return new
        {
            name = user.FullName,
            role = roles.First()
        };
    }

    public async Task<ProfileUpdateDTO> GetUpdateProfileAsync()
    {
        string userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.SerialNumber)?.Value ?? throw new Exception("Unauthorized");

        AppUser user = await _userManager.FindByIdAsync(userId) ?? throw new Exception("User not found");

        return _mapper.Map<ProfileUpdateDTO>(user);
    }

    public async Task UpdateProfileAsync(ProfileUpdateDTO dto)
    {
        string userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.SerialNumber)?.Value ?? throw new Exception("Unauthorized");

        AppUser user = await _userManager.FindByIdAsync(userId) ?? throw new Exception("User not found");

        if (user.Email != dto.Email)
        {
            AppUser? existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser is not null && existingUser.Id != user.Id) throw new Exception("An account already exists with this email");
        }

        if (user.PhoneNumber != dto.PhoneNumber)
        {
            AppUser? existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);
            if (existingUser is not null && existingUser.Id != user.Id) throw new Exception("An account already exists with this phone number");
        }

        _mapper.Map(dto, user);

        IdentityResult result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) throw new Exception("User could not be updated");
    }

    public async Task ConfirmEmailAsync(ConfirmEmailDTO dto)
    {
        AppUser user = await _userManager.FindByEmailAsync(dto.Email) ?? throw new Exception("User not found");

        IdentityResult result = await _userManager.ConfirmEmailAsync(user, dto.Token);
        if (!result.Succeeded) throw new Exception("An error occurred while saving the confirm email");
    }

    public async Task SendResetPasswordAsync(string email)
    {
        AppUser user = await _userManager.FindByEmailAsync(email) ?? throw new Exception("User not found");
        string token = await _userManager.GeneratePasswordResetTokenAsync(user);

        await _emailService.SendTokenAsync(
            user.Email, user.FullName,
            "ChangePasswordTemplate.cshtml", "Change Password",
            $"https://code8hackathon.duckdns.org/user/change-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}"
        );
    }

    public async Task ResetPasswordAsync(ResetPasswordDTO dto)
    {
        AppUser user = await _userManager.FindByEmailAsync(dto.Email) ?? throw new Exception("User not found");

        if (!await _userManager.CheckPasswordAsync(user, dto.OldPassword)) throw new Exception("Password is wrong");

        IdentityResult result;

        if (!user.EmailConfirmed)
        {
            user.EmailConfirmed = true;
            result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) throw new Exception("User info could not be updated");
        }

        result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.Password);
        if (!result.Succeeded) throw new Exception("An error occurred while saving the password");
    }

    public async Task RefreshAccessTokenAsync()
    {
        string? token = _httpContextAccessor.HttpContext?.Request.Cookies["refresh_token"];
        if (!_service.ValidateRefreshToken(token)) throw new Exception("Invalid refresh token");

        IEnumerable<Claim> payload = new JwtSecurityTokenHandler().ReadJwtToken(token).Claims;
        string email = payload.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? throw new Exception("User not found");

        AppUser user = await _userManager.FindByEmailAsync(email) ?? throw new Exception("User not found");

        IEnumerable<string> roles = await _userManager.GetRolesAsync(user);

        IEnumerable<Claim> claims =
        [
            new Claim(ClaimTypes.Role, Role.Customer.ToString()),
            new Claim(ClaimTypes.GivenName, user.FullName),
            new Claim(ClaimTypes.Email, user.Email)
        ];

        string accessToken = _service.GenerateToken(TimeSpan.FromHours(1), claims);
        string refreshToken = _service.GenerateToken(TimeSpan.FromDays(7), [new Claim(ClaimTypes.Email, user.Email)]);

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("access_token", accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(1)
        });
    }

    public void Logout()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("access_token");
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("refresh_token");
    }
}
