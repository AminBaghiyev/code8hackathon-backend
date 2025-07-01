using FluentValidation;
using Management.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.BL.DTOs.ServiceDTOs
{
    public record ServiceUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }
    }
    public class ServicesUpdateDTOValidator : AbstractValidator<ServiceUpdateDTO>
    {
        public ServicesUpdateDTOValidator()
        {
            RuleFor(e => e.Name).NotEmpty().NotNull().WithMessage("Service name can't be empty");
            RuleFor(e => e.Price).NotEmpty().NotNull().WithMessage("Service price can't be empty");
            RuleFor(e => e.ReservationId).GreaterThan(0).WithMessage("Reservation Id must be natural");
        }
    }
}
