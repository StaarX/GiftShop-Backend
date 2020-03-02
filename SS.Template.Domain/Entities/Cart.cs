using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS.Template.Domain.Model;

namespace SS.Template.Domain.Entities
{
    public class Cart : Entity, IStatus<EnabledStatus>, IHaveDateCreated, IHaveDateUpdated
    {
        public Guid UserId { get; set; }


        //Foreign Keys declaration
        public ICollection<CartItem> CartItems { get; set; }



        public EnabledStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
