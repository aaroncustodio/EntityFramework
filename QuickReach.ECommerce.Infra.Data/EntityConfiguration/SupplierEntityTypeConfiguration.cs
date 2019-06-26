using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.EntityConfiguration
{
    //configuration of entity
    //you can configure the table names, required
    //properties in here, similar to what can be done
    //on the entities ([Required], [MaxLength()]
    public class SupplierEntityTypeConfiguration
        : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.Property(s => s.ID)
                   .IsRequired()
                   .ValueGeneratedOnAdd();
            builder.Property(s => s.Name)
                   .HasMaxLength(20);
        }
    }
}
