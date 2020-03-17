using System;
using System.Collections.Generic;
using SS.Template.Domain.Entities;
using SS.Template.Domain.Model;

namespace SS.Template.Application.ShopCart
{
    public class CartItemModel
    {
        public Guid UserId { get; set; }
        public Guid CartId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public ProductDetails ProductDetail { get; set; }

        public EnabledStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
