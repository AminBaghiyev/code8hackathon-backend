using FluentValidation;

namespace Management.BL.DTOs;
public record RegisterDTO
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}

public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
{
    public RegisterDTOValidator()
    {
        RuleFor(e => e.FullName)
            .NotEmpty().NotNull().WithMessage("FullName can't be empty")
            .MaximumLength(100).WithMessage("FullName cannot exceed 100 characters");

        RuleFor(e => e.Email)
            .NotEmpty().NotNull().WithMessage("Email can't be empty")
            .EmailAddress().WithMessage("A valid email address is required");

        RuleFor(e => e.PhoneNumber)
            .NotEmpty().NotNull().WithMessage("Phone number can't be empty")
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters");

        RuleFor(e => e.Password)
            .NotEmpty().NotNull().WithMessage("Password can't be empty")
            .MinimumLength(6).WithMessage("Password must contain at least 6 symbols");

        RuleFor(e => e.ConfirmPassword)
            .NotEmpty().NotNull().WithMessage("ConfirmPassword can't be empty")
            .MinimumLength(6).WithMessage("ConfirmPassword must contain at least 6 symbols")
            .Equal(e => e.Password).WithMessage("Password doesnt match");
    }
}
