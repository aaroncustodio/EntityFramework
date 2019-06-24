using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using QuickReach.ECommerce.Infra.Data.Repositories;
using QuickReach.ECommerce.Domain.Models;

namespace QuickReach.ECommerce.Infra.Data.Tests
{
    public class ProductRepositoryTest
    {
        [Fact]
        public void Create_WithValidProduct_ShouldAddProductToDatabase()
        {
            //arrange
            var context = new ECommerceDbContext();
            var sut = new ProductRepository(context);
            var categoryRepository = new CategoryRepository(context);

            var category = new Category
            {
                Name = "Sweets",
                Description = "Matamis yo."
            };
            categoryRepository.Create(category);

            var product = new Product
            {
                Name = "Hany",
                Description = "Rectangular sweety goodness.",
                Price = 2,
                CategoryID = category.ID,
                ImageURL = "https://www.QuickReach.com/ECommerce/Products/Hany.jpg"
            };

            //act
            sut.Create(product);

            //assert
            var retrievedProduct = sut.Retrieve(product.ID);

            Assert.True(product.ID != 0);
            Assert.Equal(retrievedProduct.CategoryID, category.ID);
            Assert.NotNull(retrievedProduct);

            //cleanup
            sut.Delete(product.ID);
            categoryRepository.Delete(category.ID);
        }
    }
}
