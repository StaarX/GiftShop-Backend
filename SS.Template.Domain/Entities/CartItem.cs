using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS.Template.Domain.Model;

namespace SS.Template.Domain.Entities
{
    public class CartItem : Entity, IStatus<EnabledStatus>, IHaveDateCreated, IHaveDateUpdated
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
    }
}
