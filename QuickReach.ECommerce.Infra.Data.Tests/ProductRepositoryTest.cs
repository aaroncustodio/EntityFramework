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

namespace QuickReach.ECommerce.Infra.Data.Tests
{
    public class ProductRepositoryTest
    {
        [Fact]
        public void Create_WithValidProduct_ShouldAddProductToDatabase()
        {
            #region Options
            //arrange
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);

            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                    .UseSqlite(connection)
                    .Options; 
            #endregion


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
        public void Delete_WithValidProductID_RetrieveShouldReturnNull()
        {
            #region Options
            //arrange
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);

            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                    .UseSqlite(connection)
                    .Options;
            #endregion


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
            #region Options
            //arrange
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);

            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                    .UseSqlite(connection)
                    .Options;
            #endregion


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
            #region Options
            //arrange
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);

            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                    .UseSqlite(connection)
                    .Options;
            #endregion


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
            #region Options
            //arrange
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);

            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                    .UseSqlite(connection)
                    .Options;
            #endregion


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

                var actual = sut.Retrieve(product.ID);
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
            }
        }
    }
}
