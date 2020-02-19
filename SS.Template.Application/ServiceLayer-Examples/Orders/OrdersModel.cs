using System;
using System.Collections.Generic;
using SS.Template.Domain.Entities;
using SS.Template.Domain.Model;

namespace SS.Template.Application.Orders
{
    public class OrdersModel
    {
        public Guid Id { get; set; }

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
