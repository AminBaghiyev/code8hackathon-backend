using FluentValidation;

namespace Management.BL.DTOs;

public record ResetPasswordDTO
{
    public string Token { get; set; }
    public string Email { get; set; }
    public string OldPassword { get; set; }
    public string Password { get; set; }
}

public class ResetPasswordDTOValidator : AbstractValidator<ResetPasswordDTO>
{
    public ResetPasswordDTOValidator()
    {
        RuleFor(e => e.Token)
            .NotEmpty().NotNull().WithMessage("Token is required");

        RuleFor(e => e.Email)
            .NotEmpty().NotNull().WithMessage("Email can't be empty")
            .EmailAddress().WithMessage("A valid email address is required");

        RuleFor(e => e.OldPassword)
            .NotEmpty().NotNull().WithMessage("Old password can't be empty")
            .MinimumLength(6).WithMessage("Old password must contain at least 6 symbols!");

        RuleFor(e => e.Password)
            .NotEmpty().NotNull().WithMessage("Password can't be empty")
            .MinimumLength(6).WithMessage("Password must contain at least 6 symbols!");
    }
}