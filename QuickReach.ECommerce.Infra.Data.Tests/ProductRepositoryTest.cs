using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using QuickReach.ECommerce.Infra.Data.Repositories;
using QuickReach.ECommerce.Domain.Models;
using System.Collections;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Infra.Data.Tests.Utilities;

namespace QuickReach.ECommerce.Infra.Data.Tests
{
    public class ProductRepositoryTest
    {
        [Fact]
        public void Create_WithValidProduct_ShouldAddProductToDatabase()
        {
            var options = ConnectionOptionHelper.SQLite();

            #region AddCategory
            var category = new Category
            {
                Name = "Sweets",
                Description = "Sweets category"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Categories.Add(category);
                context.SaveChanges();
            } 
            #endregion

            var product = new Product
            {
                Name = "Hany",
                Description = "Manufactured by Annie's Sweets",
                Price = 2,
                CategoryID = category.ID,
                ImageURL = "https://www.QuickReach.com/ECommerce/Products/Hany.jpg"
            };

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new ProductRepository(context);

                //act
                sut.Create(product);

                //assert
                var actual = context.Products.Find(product.ID);

                Assert.NotNull(actual);
                Assert.Equal(product.Name, actual.Name);
                Assert.Equal(product.Description, actual.Description);
                Assert.Equal(product.Price, actual.Price);
                Assert.Equal(product.CategoryID, actual.CategoryID);
                Assert.Equal(product.ImageURL, actual.ImageURL);
            }
        }

        [Fact]
        public void Create_ProductWithNoExistingCategory_ShouldThrowException()
        {
            var options = ConnectionOptionHelper.SQLite();

            //for testing with existing category
            #region AddCategory
            //var category = new Category
            //{
            //    Name = "Sweets",
            //    Description = "Sweets category"
            //};

            //using (var context = new ECommerceDbContext(options))
            //{
            //    context.Database.OpenConnection();
            //    context.Database.EnsureCreated();

            //    context.Categories.Add(category);
            //    context.SaveChanges();
            //}
            #endregion

            #region AddProduct
            var product = new Product
            {
                Name = "Hany",
                Description = "Manufactured by Annie's Sweets",
                Price = 2,
                CategoryID = 0,
                ImageURL = "https://www.QuickReach.com/ECommerce/Products/Hany.jpg"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
            }
            #endregion

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new ProductRepository(context);

                //act & assert
                Assert.Throws<DbUpdateException>(() => sut.Create(product));
            }
        }

        [Fact]
        public void Delete_WithValidProductID_RetrieveShouldReturnNull()
        {
            var options = ConnectionOptionHelper.SQLite();

            #region AddCategory
            var category = new Category
            {
                Name = "Sweets",
                Description = "Sweets category"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Categories.Add(category);
                context.SaveChanges();
            }
            #endregion

            #region AddProduct
            var product = new Product
            {
                Name = "Hany",
                Description = "Manufactured by Annie's Sweets",
                Price = 2,
                CategoryID = category.ID,
                ImageURL = "https://www.QuickReach.com/ECommerce/Products/Hany.jpg"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Products.Add(product);
                context.SaveChanges();
            } 
            #endregion

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new ProductRepository(context);

                //act
                sut.Delete(product.ID);

                //assert
                var actual = context.Products.Find(product.ID);
                Assert.Null(actual); 
            }
        }

        [Fact]
        public void Retrieve_WithSkipAndCount_ShouldReturnCorrectListOfProducts()
        {
            var options = ConnectionOptionHelper.SQLite();

            #region AddCategory
            var category = new Category
            {
                Name = "Sweets",
                Description = "Sweets category"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Categories.Add(category);
                context.SaveChanges();
            }
            #endregion

            using (var context = new ECommerceDbContext(options))
            {
                for (int i = 1; i <= 20; i += 1)
                {
                    context.Products.Add(new Product
                    {
                        Name = string.Format("Product{0}", i),
                        Description = string.Format("Description{0}", i),
                        Price = i,
                        CategoryID = category.ID,
                        ImageURL = string.Format("URL{0}", i)
                    });
                }
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new ProductRepository(context);

                //act
                var list = sut.Retrieve(4, 4);

                //assert
                Assert.True(list.Count() == 4); 
            }
        }
        
        [Fact]
        public void Retrieve_WithValidProductID_ShouldReturnTheProduct()
        {
            var options = ConnectionOptionHelper.SQLite();

            #region AddCategory
            var category = new Category
            {
                Name = "Sweets",
                Description = "Sweets category"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Categories.Add(category);
                context.SaveChanges();
            }
            #endregion

            #region AddProduct
            var product = new Product
            {
                Name = "Hany",
                Description = "Manufactured by Annie's Sweets",
                Price = 2,
                CategoryID = category.ID,
                ImageURL = "https://www.QuickReach.com/ECommerce/Products/Hany.jpg"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Products.Add(product);
                context.SaveChanges();
            }
            #endregion

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new ProductRepository(context);

                //act
                var actual = sut.Retrieve(product.ID);

                //assert
                Assert.NotNull(actual); 
            }
        }

        [Fact]
        public void Update_WithValidProduct_RetrieveShouldReturnUpdatedProduct()
        {
            var options = ConnectionOptionHelper.SQLite();

            #region AddCategory
            var category = new Category
            {
                Name = "Sweets",
                Description = "Sweets category"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Categories.Add(category);
                context.SaveChanges();
            }
            #endregion

            #region AddProduct
            var product = new Product
            {
                Name = "Hany",
                Description = "Manufactured by Annie's Sweets",
                Price = 2,
                CategoryID = category.ID,
                ImageURL = "https://www.QuickReach.com/ECommerce/Products/Hany.jpg"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Products.Add(product);
                context.SaveChanges();
            }
            #endregion

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new ProductRepository(context);

                var actual = context.Products.Find(product.ID);
                actual.Name = "Frutos";
                actual.Description = "Manufactured by ACS";
                actual.Price = 1;
                actual.ImageURL = "https://www.QuickReach.com/ECommerce/Products/Frutos.jpg";

                //act
                var expected = sut.Update(actual.ID, actual);

                //assert
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Description, actual.Description);
                Assert.Equal(expected.Price, actual.Price);
                Assert.Equal(expected.ImageURL, actual.ImageURL); 
            }
        }
    }
}
