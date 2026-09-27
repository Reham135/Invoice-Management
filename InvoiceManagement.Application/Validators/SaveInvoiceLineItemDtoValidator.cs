using FluentValidation;
using InvoiceManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagement.Application.Validators;

public class SaveInvoiceLineItemDtoValidator : AbstractValidator<SaveInvoiceLineItemDto>
{
    public SaveInvoiceLineItemDtoValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
    }
}