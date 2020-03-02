using System;
using System.Collections.Generic;
using SS.Template.Domain.Entities;
using SS.Template.Domain.Model;

namespace SS.Template.Application.ShopCart
{
    public class CartModel
    {

        public Guid UserId { get; set; }

        public ICollection<CartItem> CartItems { get; set; }

        public EnabledStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
