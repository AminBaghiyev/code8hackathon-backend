using FluentValidation;
using Management.Core.Enums;
using Microsoft.Identity.Client;

namespace Management.BL.DTOs;

public record class RoomCreateDTO
{
    public int Number { get; set; }
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public RoomStatus Status { get; set; }
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
    }
}


