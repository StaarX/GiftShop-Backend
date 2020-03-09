using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS.Template.Domain.Model;

namespace SS.Template.Domain.Entities
{
    public class CartItem : IStatus<EnabledStatus>, IHaveDateCreated, IHaveDateUpdated
    {
        public Guid CartID { get; set; }
        public Cart Cart { get; set; }
        public Guid ProductDetailsId { get; set; }
        public ProductDetails ProductDetail { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }



        public EnabledStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            CartItem objAsPart = obj as CartItem;
            if (objAsPart == null)
                return false;
            else
                return Equals(objAsPart);
        }

        public bool Equals(CartItem other)
        {
            if (other == null)
                return false;
            return (this.ProductDetail.Id.Equals(other.ProductDetail.Id));
        }
    }
}
