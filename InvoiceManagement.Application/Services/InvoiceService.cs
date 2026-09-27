using FluentValidation;
using InvoiceManagement.Application.DTOs;
using InvoiceManagement.Application.Interfaces;
using InvoiceManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace InvoiceManagement.Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IApplicationDbContext _context;
        private readonly IValidator<SaveInvoiceDto> _validator;

        public InvoiceService(IApplicationDbContext context, IValidator<SaveInvoiceDto> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<List<InvoiceDto>> GetAllAsync()
        {
            return await _context.Invoices
                .AsNoTracking()
                .OrderByDescending(x => x.InvoiceDate)
                .Select(x => new InvoiceDto
                {
                    Id = x.Id,
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceDate = x.InvoiceDate,
                    CustomerName = x.CustomerName,
                    Status = x.Status,

                    LineItems = x.LineItems
                        .Select(item => new InvoiceLineItemDto
                        {
                            Id = item.Id,
                            Description = item.Description,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<InvoiceDto?> GetByIdAsync(int id)
        {
            return await _context.Invoices
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new InvoiceDto
                {
                    Id = x.Id,
                    InvoiceNumber = x.InvoiceNumber,
                    InvoiceDate = x.InvoiceDate,
                    CustomerName = x.CustomerName,
                    Status = x.Status,

                    LineItems = x.LineItems
                        .Select(item => new InvoiceLineItemDto
                        {
                            Id = item.Id,
                            Description = item.Description,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(SaveInvoiceDto invoice)
        {
            await _validator.ValidateAndThrowAsync(invoice);

            var invoiceNumber = invoice.InvoiceNumber.Trim();
            var invoiceNumberExists = await _context.Invoices.AnyAsync(x => x.InvoiceNumber == invoiceNumber);

            if (invoiceNumberExists)
            {
                throw new ValidationException("An invoice with the same invoice number already exists.");
            }

            var entity = new Invoice
            {
                InvoiceNumber = invoiceNumber,
                InvoiceDate = invoice.InvoiceDate,
                CustomerName = invoice.CustomerName.Trim(),
                Status = invoice.Status,
                LineItems = invoice.LineItems
                    .Select(item => new InvoiceLineItem
                    {
                        Description = item.Description.Trim(),
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    })
                    .ToList()
            };

            _context.Invoices.Add(entity);

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> UpdateAsync(int id, SaveInvoiceDto invoice)
        {
            await _validator.ValidateAndThrowAsync(invoice);

            var entity = await _context.Invoices
                .Include(x => x.LineItems)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;

            var invoiceNumber = invoice.InvoiceNumber.Trim();

            var invoiceNumberExists = await _context.Invoices
                .AnyAsync(x =>
                    x.InvoiceNumber == invoiceNumber &&
                    x.Id != id);

            if (invoiceNumberExists)
            {
                throw new ValidationException("An invoice with the same invoice number already exists.");
            }

            entity.InvoiceNumber = invoiceNumber;
            entity.InvoiceDate = invoice.InvoiceDate;
            entity.CustomerName = invoice.CustomerName.Trim();
            entity.Status = invoice.Status;

            var submittedItemIds = invoice.LineItems
                .Where(x => x.Id.HasValue)
                .Select(x => x.Id!.Value)
                .ToHashSet();

            var removedItems = entity.LineItems.Where(x => !submittedItemIds.Contains(x.Id)).ToList();

            _context.InvoiceLineItems.RemoveRange(removedItems);

            foreach (var itemDto in invoice.LineItems)
            {
                if (itemDto.Id.HasValue)
                {
                    var existingItem = entity.LineItems.FirstOrDefault(x => x.Id == itemDto.Id.Value);

                    if (existingItem == null)
                    {
                        throw new ValidationException($"Line item with ID {itemDto.Id.Value} was not found.");
                    }

                    existingItem.Description = itemDto.Description.Trim();
                    existingItem.Quantity = itemDto.Quantity;
                    existingItem.UnitPrice = itemDto.UnitPrice;
                }
                else
                {
                    entity.LineItems.Add(new InvoiceLineItem
                    {
                        Description = itemDto.Description.Trim(),
                        Quantity = itemDto.Quantity,
                        UnitPrice = itemDto.UnitPrice
                    });
                }
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var invoice = await _context.Invoices.FirstOrDefaultAsync(x => x.Id == id);

            if (invoice == null)
                return false;

            _context.Invoices.Remove(invoice);

            await _context.SaveChangesAsync();

            return true;
        }
    }
 }
