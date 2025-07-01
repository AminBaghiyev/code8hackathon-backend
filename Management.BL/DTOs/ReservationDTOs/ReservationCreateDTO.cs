using FluentValidation;
using Management.BL.DTOs.ServiceDTOs;
using Management.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.BL.DTOs;

public record ReservationCreateDTO
{
    public string CustomerId { get; set; }
    public int RoomId { get; set; }
    public int ServiceId { get; set; }
    public DateTime CheckInDate { get; set; }
    public List<ServiceCreateDTO> Service { get; set; }
    public DateTime CheckOutDate { get; set; }
}
public class ReservationCreateDTOValidator : AbstractValidator<ReservationCreateDTO>
{
    public ReservationCreateDTOValidator()
    {
        RuleFor(e => e.RoomId).GreaterThan(0).WithMessage("Id must be natural");
        RuleFor(e => e.CheckInDate)
        .LessThan(e => e.CheckOutDate)
        .WithMessage("The entry date must be before the date");

        RuleFor(e => e.CheckInDate)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("The login date cannot be past");
    }
}
