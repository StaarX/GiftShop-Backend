using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS.Template.Domain.Model;

namespace SS.Template.Domain.Entities
{
    public class ProductDetails : Entity, IStatus<EnabledStatus>, IHaveDateCreated, IHaveDateUpdated, IEquatable<ProductDetails>
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public decimal Price { get; set; }

        public string Type { get; set; }
        public int Availability { get; set; }



        public EnabledStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            ProductDetails objAsPart = obj as ProductDetails;
            if (objAsPart == null)
                return false;
            else
                return Equals(objAsPart);
        }

        public bool Equals(ProductDetails other)
        {
            if (other == null) return false;
            return (this.Id.Equals(other.Id));
        }
    }
}
