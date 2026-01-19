using Challenger.FCamara.Luxclusif.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Challenger.FCamara.Luxclusif.Infrastructure.Persistences.Mapping;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.AcquisitionCostInSupplierCurrency)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.AcquisitionCostInUSD)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.AcquireDate)
            .IsRequired();

        builder.Property(p => p.SoldDate);

        builder.Property(p => p.CancelDate);

        builder.Property(p => p.ReturnDate);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(p => p.WmsProductId)
            .HasMaxLength(100);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt);

        // Indexes
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.WmsProductId);
    }
}