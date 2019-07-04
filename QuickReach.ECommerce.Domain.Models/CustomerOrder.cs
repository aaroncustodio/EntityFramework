using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Domain.Models
{
    public class CustomerOrder
    {
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        public int OrderID { get; set; }
        public Order Order { get; set; }
    }
}
