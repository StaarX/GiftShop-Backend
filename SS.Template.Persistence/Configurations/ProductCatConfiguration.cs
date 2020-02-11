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
    public class ProductCatConfiguration : IEntityTypeConfiguration<ProductCat>
    {
        public void Configure(EntityTypeBuilder<ProductCat> builder)
        {
            builder.HasKey(x=> new { x.ProductId, x.CategoryId});

            builder.HasOne<Product>(pc => pc.Product)
                   .WithMany(p=>p.ProductCatRelation)
                   .HasForeignKey(pc=>pc.ProductId);

            builder.HasOne<Category>(pc => pc.Category)
                   .WithMany(c => c.ProductCatRelation)
                   .HasForeignKey(pc => pc.CategoryId);
        }
    }
}
