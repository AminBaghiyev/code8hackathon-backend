using FluentValidation;

namespace Management.BL.DTOs;

public record UserUpdateDTO
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}

public class UserUpdateDTOValidator : AbstractValidator<UserUpdateDTO>
{
    public UserUpdateDTOValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().NotNull().WithMessage("Id is required")
            .Length(36, 36).WithMessage("Invalid Id");

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