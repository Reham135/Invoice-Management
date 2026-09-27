
namespace InvoiceManagement.Application.DTOs;
public class SaveInvoiceLineItemDto
{
    public int? Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
}
