using InvoiceManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceManagement.Infrastructure.Configurations;

public class InvoiceLineItemConfiguration : IEntityTypeConfiguration<InvoiceLineItem>
{
    public void Configure(EntityTypeBuilder<InvoiceLineItem> builder)
    {
        builder.ToTable("InvoiceLineItems", table =>
        {
            table.HasCheckConstraint("CK_InvoiceLineItems_Quantity","Quantity > 0");

            table.HasCheckConstraint("CK_InvoiceLineItems_UnitPrice","UnitPrice >= 0");
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Quantity).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.UnitPrice).IsRequired().HasPrecision(18, 2);
    }
}
