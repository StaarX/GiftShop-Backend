using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS.Template.Domain.Model;

namespace SS.Template.Domain.Entities
{
    public class User : Entity, IStatus<EnabledStatus>, IHaveDateCreated, IHaveDateUpdated
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        //ForeignKeys Declarations
        public ICollection<Order> Orders { get; set; }



        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public EnabledStatus Status { get; set; }
    }
}
