using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Repositories
{
    public class CustomerRepository
        : RepositoryBase<Customer>,
          ICustomerRepository
    {
        public CustomerRepository(
            ECommerceDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Customer> Retrieve(string search = "", int skip = 0, int count = 10)
        {
            var result = this.context.Customers
                .Where(c => c.LastName.Contains(search) ||
                            c.FirstName.Contains(search))
                .Skip(skip)
                .Take(count)
                .ToList();

            return result;
        }
    }
}
