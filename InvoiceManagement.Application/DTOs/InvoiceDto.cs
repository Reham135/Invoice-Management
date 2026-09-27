using InvoiceManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagement.Application.DTOs;

public class InvoiceDto
{
    public int Id { get; set; }

    public string InvoiceNumber { get; set; } = string.Empty;

    public DateTime InvoiceDate { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public InvoiceStatus Status { get; set; }

    public List<InvoiceLineItemDto> LineItems { get; set; } = [];

    public decimal Total => LineItems.Sum(x => x.LineTotal);
}
