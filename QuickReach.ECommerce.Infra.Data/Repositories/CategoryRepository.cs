using Microsoft.EntityFrameworkCore;
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

        //an extended function added to the categoryrepository class
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

        public override Category Retrieve(int entityId)
        {
            var entity = this.context.Categories
                    .Include(c => c.ProductCategories)
                    .Include(c => c.ChildCategories)
                    .Include(c => c.ParentCategories)
                    .Where(c => c.ID == entityId)
                    .FirstOrDefault();

            return entity;
        }

        //includes the list of products when viewing a category
        //public override Category Retrieve(int entityId)
        //{
        //    var entity = this.context.Categories
        //        .AsNoTracking()
        //        .Include(c => c.Products)
        //        .Where(c => c.ID == entityId)
        //        .FirstOrDefault();

        //    return entity;
        //}

        //public override void Delete(int entityId)
        //{
        //    //checks if foreign key exists
        //    var CategoryHasProduct =
        //        this.context
        //            .Products
        //            .Where(c => c.CategoryID == entityId);

        //    if (CategoryHasProduct.Count() != 0)
        //    {
        //        throw new System.Exception("Category has products, it cannot be deleted.");
        //    }

        //    base.Delete(entityId);
        //}
    }
}
