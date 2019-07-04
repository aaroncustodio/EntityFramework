using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Domain.Models
{
    public class Order : EntityBase
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public List<OrderItem> Items { get; set; }

        public Order(int cartId, int customerId)
        {
            CartId = cartId;
            CustomerId = customerId;
            Items = new List<OrderItem>();
        }
    }
}
