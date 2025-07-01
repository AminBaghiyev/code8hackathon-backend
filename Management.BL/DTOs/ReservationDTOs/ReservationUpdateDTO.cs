using FluentValidation;
using Management.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.BL.DTOs;

public record ReservationUpdateDTO
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}
public class ReservationUpdateDTOValidator : AbstractValidator<ReservationUpdateDTO>
{
    public ReservationUpdateDTOValidator()
    {
        RuleFor(e => e.Id).GreaterThan(0).WithMessage("Id must be natural");
        RuleFor(e => e.RoomId).GreaterThan(0).WithMessage("Id must be natural");
    }
}
