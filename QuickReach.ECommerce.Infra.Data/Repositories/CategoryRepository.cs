using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Repositories
{
    //the purpose of this class is that when there are specific methods
    //for this certain, e.g. 'CategoryRepository' class, it will be much easier
    //to add the methods here
    public class CategoryRepository
        : RepositoryBase<Category>,
          IRepository<Category>
    {
        public CategoryRepository(
            ECommerceDbContext context)
            : base(context)
        {

        }
    }
}
