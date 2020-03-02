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
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(x => new {x.ProductDetailsId,x.CartID});

            builder.Property(x => x.Quantity)
                   .IsRequired()
                   .HasMaxLength(AppConstants.StandardValueLength);

            builder.Property(x => x.UnitPrice)
                   .IsRequired()
                   .HasMaxLength(AppConstants.StandardValueLength);

            builder.HasOne<Cart>(oi => oi.Cart)
                   .WithMany(o => o.CartItems)
                   .HasForeignKey(oi => oi.CartID);
                       
        }
    }
}
