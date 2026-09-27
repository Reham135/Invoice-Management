using InvoiceManagement.Application.DTOs;

namespace InvoiceManagement.Application.Interfaces;

public interface IInvoiceService
{
    Task<List<InvoiceDto>> GetAllAsync();
    Task<InvoiceDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(SaveInvoiceDto invoice);
    Task<bool> UpdateAsync(int id, SaveInvoiceDto invoice);
    Task<bool> DeleteAsync(int id);
}
