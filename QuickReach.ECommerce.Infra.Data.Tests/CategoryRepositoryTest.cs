using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repositories;
using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace QuickReach.ECommerce.Infra.Data.Tests
{
    public class CategoryRepositoryTest
    {
        [Fact]
        public void Create_WithValidEntity_ShouldCreateDatabaseRecord()
        {
            //arrange
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);

            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                    .UseSqlite(connection)
                    .Options;


            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var sut = new CategoryRepository(context);

                //act
                sut.Create(category);
            }

            using (var context = new ECommerceDbContext(options))
            {
                //assert
                var actual = context.Categories.Find(category.ID);

                Assert.NotNull(actual);
                Assert.Equal(category.Name, actual.Name);
                Assert.Equal(category.Description, actual.Description);

            }
        }

        [Fact]
        public void Retrieve_WithValidEntityID_ReturnsAValidEntity()
        {
            //arrange
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);

            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                    .UseSqlite(connection)
                    .Options;


            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Categories.Add(category);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new CategoryRepository(context);

                //act
                var actual = sut.Retrieve(category.ID);

                //assert
                Assert.NotNull(actual); 
            }
        }

        [Fact]
        public void Retrieve_WithNonExistingEntityID_ReturnsNull()
        {
            //arrange
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);

            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                    .UseSqlite(connection)
                    .Options;

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var sut = new CategoryRepository(context);

                //act
                var actual = sut.Retrieve(-1);

                //assert
                Assert.Null(actual); 
            }
        }

        [Fact]
        public void Retrieve_WithSkipAndCount_ReturnsTheCorrectPage()
        {
            //arrange
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);

            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                    .UseSqlite(connection)
                    .Options;


            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                for (int i = 1; i <= 20; i += 1)
                {
                    context.Categories.Add(new Category
                    {
                        Name = string.Format("Category {0}", i),
                        Description = string.Format("Description {0}", i)
                    });
                }
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new CategoryRepository(context);

                //act
                var list = sut.Retrieve(5, 5);

                //assert
                Assert.True(list.Count() == 5); 
            }
        }

        [Fact]
        public void Delete_WithValidID_RetrieveShouldReturnNull()
        {
            //arrange
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);

            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                    .UseSqlite(connection)
                    .Options;


            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Categories.Add(category);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new CategoryRepository(context);

                //act
                sut.Delete(category.ID);

                //assert
                var actual = context.Categories.Find(category.ID);

                Assert.Null(actual); 
            }
        }

        [Fact]
        public void Update_WithValidEntity_RetrieveShouldReturnUpdatedEntity()
        {
            //arrange
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);

            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                    .UseSqlite(connection)
                    .Options;

            var oldCategory = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Categories.Add(oldCategory);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new CategoryRepository(context);

                var actual = context.Categories.Find(oldCategory.ID);
                actual.Name = "Shirts";
                actual.Description = "Shirts Department";

                //act
                sut.Update(actual.ID, actual);

                //assert
                var UpdatedCategory = context.Categories.Find(oldCategory.ID);

                Assert.Equal(UpdatedCategory.Name, actual.Name);
                Assert.Equal(UpdatedCategory.Description, actual.Description); 
            }
        }
    }
}
