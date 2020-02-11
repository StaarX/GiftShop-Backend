using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SS.Template.Core;
using SS.Template.Domain.Entities;

namespace SS.Template.Persistence.Configurations
{
    public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => new { c.Id});
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(AppConstants.StandardValueLength);
            builder.Property(x => x.Description)
                   .IsRequired()
                   .HasMaxLength(AppConstants.StandardValueLength);
        }
    }
}
