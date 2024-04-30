using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Core.Dtos;
using FluentValidation;

namespace FinanceApp.Presentation.Validator
{
    public class ServiceValidator : AbstractValidator<ServiceDto>
    {
        public ServiceValidator()
        {
        //     RuleFor(service => service.ServiceImageUrl)
        //  .NotEmpty().WithMessage("The Service Image URL is required.");

            RuleFor(service => service.ServiceName)
                .NotEmpty().WithMessage("The Service Name is required.");

            RuleFor(service => service.ServiceCost)
           .NotEmpty().GreaterThan(0).WithMessage("The Service Cost is required.");

        }
    }
}