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
    public class ProductDetailsConfiguration : IEntityTypeConfiguration<ProductDetails>
    {
        public void Configure(EntityTypeBuilder<ProductDetails> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne<Product>(pd => pd.Product)
                   .WithMany(p => p.ProductDetails)
                   .HasForeignKey(pd => pd.ProductId);

            builder.Property(x => x.Price)
                   .IsRequired()
                   .HasMaxLength(AppConstants.StandardDecimalValueLength);

            builder.Property(x => x.Type)
                   .IsRequired()
                   .HasMaxLength(AppConstants.StandardValueLength);

            builder.Property(x => x.Availability)
                   .IsRequired()
                   .HasMaxLength(AppConstants.StandardValueLength);
        }
    }
}
