using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SS.Template.Core;
using SS.Template.Domain.Entities;

namespace SS.Template.Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => new { x.Id,x.OrderID});

            builder.Property(x => x.Quantity)
                   .IsRequired()
                   .HasMaxLength(AppConstants.StandardValueLength);

            builder.Property(x => x.UnitPrice)
                   .IsRequired()
                   .HasMaxLength(AppConstants.StandardValueLength);

            builder.HasOne<Order>(oi => oi.Order)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(oi => oi.OrderID);
                       
        }
    }
}
