using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using QuickReach.ECommerce.Infra.Data.Repositories;
using QuickReach.ECommerce.Domain.Models;
using System.Collections;

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
                Description = "Sweets category"
            };
            categoryRepository.Create(category);

            var product = new Product
            {
                Name = "Hany",
                Description = "Manufactured by Annie's Sweets",
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

        [Fact]
        public void Delete_WithValidProductID_RetrieveShouldReturnNull()
        {
            //arrange
            var context = new ECommerceDbContext();
            var sut = new ProductRepository(context);
            var categoryRepository = new CategoryRepository(context);

            var category = new Category
            {
                Name = "Sweets",
                Description = "Sweets category"
            };
            categoryRepository.Create(category);

            var product = new Product
            {
                Name = "Hany",
                Description = "Manufactured by Annie's Sweets",
                Price = 2,
                CategoryID = category.ID,
                ImageURL = "https://www.QuickReach.com/ECommerce/Products/Hany.jpg"
            };
            sut.Create(product);

            //act
            sut.Delete(product.ID);

            //assert
            var actual = sut.Retrieve(product.ID);
            Assert.Null(actual);

            //cleanup
            categoryRepository.Delete(category.ID);
        }

        [Fact]
        public void Retrieve_WithSkipAndCount_ShouldReturnCorrectListOfProducts()
        {
            //arrange
            var context = new ECommerceDbContext();
            var sut = new ProductRepository(context);
            var categoryRepository = new CategoryRepository(context);

            var category = new Category
            {
                Name = "Category1",
                Description = "Description1"
            };
            categoryRepository.Create(category);

            for (int i = 1; i <= 20; i += 1)
            {
                sut.Create(new Product
                {
                    Name = string.Format("Product{0}", i),
                    Description = string.Format("Description{0}", i),
                    Price = i,
                    CategoryID = category.ID,
                    ImageURL = string.Format("URL{0}", i)
                });
            }

            //act
            var list = sut.Retrieve(4, 4);

            //assert
            Assert.True(list.Count() == 4);

            //cleanup
            list = sut.Retrieve(0, 20);
            foreach (var i in list)
            {
                sut.Delete(i.ID);
            }
            categoryRepository.Delete(category.ID);
        }
        
        [Fact]
        public void Retrieve_WithValidProductID_ShouldReturnTheProduct()
        {
            //arrange
            var context = new ECommerceDbContext();
            var sut = new ProductRepository(context);
            var categoryRepository = new CategoryRepository(context);

            var category = new Category
            {
                Name = "Sweets",
                Description = "Sweets category"
            };
            categoryRepository.Create(category);

            var product = new Product
            {
                Name = "Hany",
                Description = "Manufactured by Annie's Sweets",
                Price = 2,
                CategoryID = category.ID,
                ImageURL = "https://www.QuickReach.com/ECommerce/Products/Hany.jpg"
            };
            sut.Create(product);

            //act
            var actual = sut.Retrieve(product.ID);

            //assert
            Assert.NotNull(actual);

            //cleanup
            sut.Delete(product.ID);
            categoryRepository.Delete(category.ID);
        }

        [Fact]
        public void Update_WithValidProduct_RetrieveShouldReturnUpdatedProduct()
        {
            //arrange
            var context = new ECommerceDbContext();
            var sut = new ProductRepository(context);
            var categoryRepository = new CategoryRepository(context);

            var category = new Category
            {
                Name = "Sweets",
                Description = "Sweets category"
            };
            categoryRepository.Create(category);

            var oldProduct = new Product
            {
                Name = "Hany",
                Description = "Manufactured by Annie's Sweets",
                Price = 2,
                CategoryID = category.ID,
                ImageURL = "https://www.QuickReach.com/ECommerce/Products/Hany.jpg"
            };
            sut.Create(oldProduct);

            var actual = sut.Retrieve(oldProduct.ID);
            actual.Name = "Hany2";
            actual.Description = "New desc.";
            actual.Price = 5;
            actual.ImageURL = "newURL";

            //act
            var expected = sut.Update(actual.ID, actual);

            //assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.Price, actual.Price);
            Assert.Equal(expected.ImageURL, actual.ImageURL);

            //cleanup
            sut.Delete(actual.ID);
            categoryRepository.Delete(category.ID);
        }
    }
}
