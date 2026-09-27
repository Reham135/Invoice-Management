
namespace InvoiceManagement.Application.DTOs;

public class InvoiceLineItemDto
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal LineTotal => Quantity * UnitPrice;
}
