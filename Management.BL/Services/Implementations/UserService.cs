using AutoMapper;
using Management.BL.DTOs;
using Management.BL.Services.Abstractions;
using Management.BL.Utilities;
using Management.Core.Entities;
using Management.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Management.BL.Services.Implementations;

public class UserService : IUserService
{
    readonly EmailService _emailService;
    readonly UserManager<AppUser> _userManager;
    readonly IMapper _mapper;

    public UserService(EmailService emailService, UserManager<AppUser> userManager, IMapper mapper)
    {
        _emailService = emailService;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ICollection<UserListItemDTO>> GetListItemsAsync(int page = 0, int count = 0)
    {
        IEnumerable<AppUser> users = await _userManager.Users.ToListAsync();

        if (count > 0) users = [..users.Skip(page * count).Take(count)];

        return _mapper.Map<ICollection<UserListItemDTO>>(users);
    }

    public async Task<ICollection<UserTableItemDTO>> GetTableItemsAsync(string? q = null, int page = 0, int count = 10)
    {
        IQueryable<AppUser> query = _userManager.Users;

        if (!string.IsNullOrWhiteSpace(q))
        {
            string normalizedQ = q.Trim().ToLower();

            query = query.Where(u =>
                u.FullName.ToLower().Contains(normalizedQ) ||
                u.Email.ToLower().Contains(normalizedQ) ||
                u.PhoneNumber.ToLower().Contains(normalizedQ)
            );
        }

        if (count > 0) query = query.Skip(page * count).Take(count);

        return _mapper.Map<ICollection<UserTableItemDTO>>(await query.ToListAsync());
    }

    public async Task<UserUpdateDTO> GetByEmailForUpdateAsync(string email) => _mapper.Map<UserUpdateDTO>(await _userManager.FindByEmailAsync(email) ?? throw new Exception("User not found"));

    public async Task CreateAsync(UserCreateDTO dto)
    {
        if (await _userManager.FindByEmailAsync(dto.Email) is not null) throw new Exception("An account already exists with this email");
        if (await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber) is not null) throw new Exception("An account already exists with this phone number");

        AppUser user = _mapper.Map<AppUser>(dto);

        IdentityResult result;

        result = await _userManager.CreateAsync(user);
        if (!result.Succeeded) throw new Exception("User could not be created");

        result = await _userManager.AddToRoleAsync(user, Role.Customer.ToString());
        if (!result.Succeeded) throw new Exception("Could not add user to role");

        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        await _emailService.SendTokenAsync(
            user.Email, user.FullName,
            "SetPasswordTemplate.cshtml", "Welcome | Set Password",
            $"https://code8hackathon.duckdns.org/user/change-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}"
        );
    }

    public async Task UpdateAsync(UserUpdateDTO dto)
    {
        AppUser user = await _userManager.FindByIdAsync(dto.Id) ?? throw new Exception("User not found");

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

    public async Task DeleteAsync(string email)
    {
        AppUser user = await _userManager.FindByEmailAsync(email) ?? throw new Exception("User not found");

        IdentityResult  result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded) throw new Exception("User could not be deleted");
    }
}
