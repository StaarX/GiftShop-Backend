using System;
using System.Collections.Generic;
using SS.Template.Domain.Entities;
using SS.Template.Domain.Model;

namespace SS.Template.Application.ShopCart
{
    public class BoughtModel
    {
        public ICollection<CartItem> itemsThatApplied { get; set; }

        public ICollection<CartItem> itemsThatDidntApply { get; set; }
    }
}
