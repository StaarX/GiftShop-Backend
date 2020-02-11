using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using SS.Template.Domain.Model;

namespace SS.Template.Domain.Entities
{
    public class Product: Entity, IStatus<EnabledStatus>, IHaveDateCreated, IHaveDateUpdated
    {
        public string Name { get; set; }
        public string Description { get; set; }

        //Foreign Keys declaration
        public ICollection<ProductCat> ProductCatRelation { get; set; }
        public ICollection<ProductDetails> ProductDetails { get; set; }


        public EnabledStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
