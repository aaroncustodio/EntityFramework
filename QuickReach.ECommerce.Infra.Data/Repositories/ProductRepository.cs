using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Repositories
{
    public class ProductRepository
        : RepositoryBase<Product>,
          IProductRepository
    {
        public ProductRepository(
            ECommerceDbContext context)
            : base(context)
        {

        }

        public IEnumerable<Product> Retrieve(string search = "", int skip = 0, int count = 10)
        {
            var result = this.context.Products
                .Where(c => c.Name.Contains(search) ||
                            c.Description.Contains(search))
                .Skip(skip)
                .Take(count)
                .ToList();

            return result;
        }

        public override Product Create(Product newEntity)
        {
            var category = this.context
                .Categories.Find(newEntity.CategoryID);

            if (category == null)
            {
                throw new System.Exception(
                    "Category does not exist, Product cannot be created.");
            }

            return base.Create(newEntity);
        }
    }
}
