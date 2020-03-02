using System;
using SS.Template.Domain.Model;

namespace SS.Template.Application.ProductDetail
{
    public class ProductDetailsModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }

        public string Type { get; set; }

        public int Availability { get; set; }

        public decimal Price { get; set; }

        public EnabledStatus Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
