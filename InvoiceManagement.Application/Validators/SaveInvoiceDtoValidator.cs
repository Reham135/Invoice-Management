using FluentValidation;
using InvoiceManagement.Application.DTOs;
namespace InvoiceManagement.Application.Validators;

public class SaveInvoiceDtoValidator: AbstractValidator<SaveInvoiceDto>
{
    public SaveInvoiceDtoValidator()
    {
        RuleFor(x => x.InvoiceNumber).NotEmpty().MaximumLength(50);

        RuleFor(x => x.CustomerName).NotEmpty().MaximumLength(150);

        RuleFor(x => x.InvoiceDate).NotEmpty();

        RuleFor(x => x.Status).IsInEnum();

        RuleFor(x => x.LineItems).NotEmpty().WithMessage("Invoice must contain at least one line item.");

        RuleForEach(x => x.LineItems).SetValidator(new SaveInvoiceLineItemDtoValidator());
    }
}