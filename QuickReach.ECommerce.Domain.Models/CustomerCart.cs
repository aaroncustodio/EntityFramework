using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Domain.Models
{
    public class CustomerCart
    {
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        public int CartID { get; set; }
        public Cart Cart { get; set; }
    }
}
