using System;
using System.Collections.Generic;
using System.Text;
using SS.Template.Domain.Model;

namespace SS.Template.Domain.Entities
{
    public class Category:Entity, IStatus<EnabledStatus>, IHaveDateCreated, IHaveDateUpdated
    {
        public string Name { get; set; }
        public string Description { get; set; }

        //Foreign Key Declaration
        public ICollection<ProductCat> ProductCatRelation { get; set; }


        public EnabledStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
