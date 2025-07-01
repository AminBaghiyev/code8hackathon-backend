using FluentValidation;

namespace Management.BL.DTOs;

public record ServiceCreateDTO
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class ServicesCreateDTOValidator : AbstractValidator<ServiceCreateDTO>
{
    public ServicesCreateDTOValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty().NotNull().WithMessage("Service name can't be empty")
            .MaximumLength(100).WithMessage("FullName cannot exceed 100 characters");

        RuleFor(e => e.Price)
            .NotEmpty().NotNull().WithMessage("Service price can't be empty")
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}
