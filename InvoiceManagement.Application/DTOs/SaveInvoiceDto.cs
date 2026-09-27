using InvoiceManagement.Domain.Enums;

namespace InvoiceManagement.Application.DTOs;

public class SaveInvoiceDto
{
    public string InvoiceNumber { get; set; } = string.Empty;

    public DateTime InvoiceDate { get; set; } = DateTime.Today;

    public string CustomerName { get; set; } = string.Empty;

    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

    public List<SaveInvoiceLineItemDto> LineItems { get; set; } = [];
}
