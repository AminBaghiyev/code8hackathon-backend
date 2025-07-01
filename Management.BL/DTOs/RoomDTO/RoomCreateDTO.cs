using FluentValidation;
using Management.Core.Enums;
using Management.BL.Utilities;
using Microsoft.AspNetCore.Http;

namespace Management.BL.DTOs;

public record RoomCreateDTO
{
    public int Number { get; set; }
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public RoomStatus Status { get; set; }
    public IFormFile File { get; set; }
}

public class RoomCreateDTOValidator : AbstractValidator<RoomCreateDTO>
{
    public RoomCreateDTOValidator()
    {
        RuleFor(x => x.Number)
            .GreaterThan(0).WithMessage("Room number must be greater than 0.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Room type is invalid.");

        RuleFor(x => x.PricePerNight)
            .GreaterThan(0).WithMessage("Price per night must be greater than 0.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Room status is invalid.");

        RuleFor(e => e.File)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Thumbnail can't be null")
            .Must(file => file.Length > 0).WithMessage("Thumbnail can't be empty")
            .Must(e => e.CheckType("jpg", "jpeg", "png")).WithMessage("Thumbnail type must be JPG, JPEG, or PNG");
    }
}
