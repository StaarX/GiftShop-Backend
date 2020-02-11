using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SS.Template.Core;
using SS.Template.Domain.Entities;

namespace SS.Template.Persistence.Configurations
{
    public sealed class ProductsConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p=>new {p.Id});
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(AppConstants.StandardValueLength);
            builder.Property(x => x.Description)
                   .HasMaxLength(AppConstants.StandardValueLength);
        }
    }
}
