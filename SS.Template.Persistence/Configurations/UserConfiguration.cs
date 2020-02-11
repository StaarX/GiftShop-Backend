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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(AppConstants.StandardValueLength);
            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasMaxLength(AppConstants.StandardValueLength);
            builder.Property(x => x.Password)
                   .IsRequired()
                   .HasMaxLength(AppConstants.StandardValueLength);
            builder.Property(x => x.Role)
                   .IsRequired()
                   .HasMaxLength(AppConstants.StandardValueLength);
        }
    }
}
