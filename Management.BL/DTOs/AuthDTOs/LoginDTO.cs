using FluentValidation;

namespace Management.BL.DTOs;

public record LoginDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
}
public class LoginDTOValidator : AbstractValidator<LoginDTO>
{
    public LoginDTOValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty().NotNull().WithMessage("Email can't be empty")
            .EmailAddress().WithMessage("A valid email address is required");

        RuleFor(e => e.Password)
            .NotEmpty().NotNull().WithMessage("Password can't be empty")
            .MinimumLength(6).WithMessage("Password must contain at least 6 symbols");
    }
}
