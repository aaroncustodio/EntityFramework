using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Repositories
{
    //the purpose of this class is that when there are specific methods
    //for this certain, e.g. 'CategoryRepository' class, it will be much easier
    //to create an ICategoryRepository interface and then add the methods here
    public class CategoryRepository
        : RepositoryBase<Category>,
          ICategoryRepository
    {
        public CategoryRepository(
            ECommerceDbContext context)
            : base(context)
        {

        }

        public IEnumerable<Category> Retrieve(string search = "", int skip = 0, int count = 10)
        {
            var result = this.context.Categories
                .Where(c => c.Name.Contains(search) ||
                            c.Description.Contains(search))
                .Skip(skip)
                .Take(count)
                .ToList();

            return result;
        }
    }
}
