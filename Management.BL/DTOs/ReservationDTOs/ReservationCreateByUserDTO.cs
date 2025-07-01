using FluentValidation;

namespace Management.BL.DTOs;

public record ReservationCreateByUserDTO
{
    public int RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public ICollection<ServiceCreateDTO> Services { get; set; }
}

public class ReservationCreateByUserDTOValidator : AbstractValidator<ReservationCreateByUserDTO>
{
    public ReservationCreateByUserDTOValidator()
    {
        RuleFor(e => e.RoomId).GreaterThan(0).WithMessage("Id must be natural");

        RuleFor(e => e.CheckInDate)
            .LessThan(e => e.CheckOutDate).WithMessage("The entry date must be before the date");

        RuleFor(e => e.CheckInDate)
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("The login date cannot be past");
    }
}
