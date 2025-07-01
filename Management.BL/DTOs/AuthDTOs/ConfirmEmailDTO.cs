using FluentValidation;

namespace Management.BL.DTOs;

public record ConfirmEmailDTO
{
    public string Token { get; set; }
    public string Email { get; set; }
}

public class ConfirmEmailDTOValidator : AbstractValidator<ConfirmEmailDTO>
{
    public ConfirmEmailDTOValidator()
    {
        RuleFor(e => e.Token)
            .NotEmpty().NotNull().WithMessage("Token is required");

        RuleFor(e => e.Email)
            .NotEmpty().NotNull().WithMessage("Email can't be empty")
            .EmailAddress().WithMessage("A valid email address is required");
    }
}