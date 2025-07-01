using FluentValidation;

namespace Management.BL.DTOs;

public record ProfileUpdateDTO
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}

public class ProfileUpdateDTOValidator : AbstractValidator<ProfileUpdateDTO>
{
    public ProfileUpdateDTOValidator()
    {
        RuleFor(e => e.FullName)
            .NotEmpty().NotNull().WithMessage("FullName can't be empty")
            .MaximumLength(100).WithMessage("FullName cannot exceed 100 characters");

        RuleFor(e => e.PhoneNumber)
            .NotEmpty().NotNull().WithMessage("Phone number can't be empty")
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters");

        RuleFor(x => x.Email)
            .NotEmpty().NotNull().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}