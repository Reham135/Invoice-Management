using FluentValidation;
using InvoiceManagement.Application.Interfaces;
using InvoiceManagement.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IInvoiceService, InvoiceService>();

        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly);

        return services;
    }
}