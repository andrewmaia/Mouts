using Mouts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Mouts.Infrastructure.PostgreSQL.Mappings;

internal class OrderMap : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Status)
            .HasColumnType("order_status")
            .IsRequired();
        builder.Property(x => x.TotalAmount)
            .IsRequired();
    }
}