using FluentValidation;
using Management.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.BL.DTOs.ServiceDTOs
{
    public record ServiceCreateDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
     
    }
    public class ServicesCreateDTOValidator : AbstractValidator<ServiceCreateDTO>
    {
        public ServicesCreateDTOValidator()
        {
            RuleFor(e => e.Name).NotEmpty().NotNull().WithMessage("Service name can't be empty")
                .MaximumLength(100);
            RuleFor(e => e.Price).NotEmpty().GreaterThan(0).NotNull().WithMessage("Service price can't be empty");
        }
    }

}
