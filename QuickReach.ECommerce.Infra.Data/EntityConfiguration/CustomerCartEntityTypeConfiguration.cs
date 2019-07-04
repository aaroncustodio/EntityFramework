using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.EntityConfiguration
{
    public class CustomerCartEntityTypeConfiguration
        : IEntityTypeConfiguration<CustomerCart>
    {
        public void Configure(EntityTypeBuilder<CustomerCart> builder)
        {
            builder.ToTable("CustomerCart");
            builder.HasKey(cc => new { cc.CustomerID, cc.CartID });
            builder.HasOne(cc => cc.Customer)
                   .WithMany(c => c.CustomerCarts)
                   .HasForeignKey("CustomerID");
        }
    }
}
