using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mouts.Domain.Entities;

namespace Mouts.Infrastructure.PostgreSQL.Mappings;

internal class SaleMap : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sale");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.SaleNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.SaleDate)
            .IsRequired();

        builder.Property(x => x.CustomerId)
            .IsRequired();

        builder.Property(x => x.CustomerName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.BranchId)
            .IsRequired();

        builder.Property(x => x.BranchName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnType("sale_status")
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.OwnsMany(x => x.Items, itemBuilder =>
        {
            itemBuilder.ToTable("SaleItem");
            itemBuilder.WithOwner().HasForeignKey("SaleId");
            itemBuilder.HasKey(x => x.Id);
            itemBuilder.Property(x => x.Id).ValueGeneratedNever();

            itemBuilder.Property<Guid>("SaleId");

            itemBuilder.Property(x => x.ProductId)
                .IsRequired();

            itemBuilder.Property(x => x.ProductName)
                .HasMaxLength(200)
                .IsRequired();

            itemBuilder.Property(x => x.Quantity)
                .IsRequired();

            itemBuilder.Property(x => x.UnitPrice)
                .HasPrecision(18, 2)
                .IsRequired();

            itemBuilder.Property(x => x.Discount)
                .HasPrecision(18, 2)
                .IsRequired();

            itemBuilder.Property(x => x.TotalAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            itemBuilder.Property(x => x.IsCancelled)
                .IsRequired();
        });

        builder.Navigation(x => x.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
