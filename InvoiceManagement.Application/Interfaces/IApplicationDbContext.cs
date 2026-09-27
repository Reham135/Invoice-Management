using InvoiceManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagement.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Invoice> Invoices { get; }
        DbSet<InvoiceLineItem> InvoiceLineItems { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
