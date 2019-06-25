using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using QuickReach.ECommerce.Infra.Data.Repositories;
using QuickReach.ECommerce.Domain.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;

namespace QuickReach.ECommerce.Infra.Data.Tests
{
    public class SupplierRepositoryTest
    {
        [Fact]
        public void Create_WithValidSupplier_ShouldAddSupplierToDatabase()
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


            var supplier = new Supplier
            {
                Name = "Annie's Sweets",
                Description = "Manufacturing and Packaging",
                IsActive = true
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var sut = new SupplierRepository(context);

                //act
                sut.Create(supplier); 
            }

            using (var context = new ECommerceDbContext(options))
            {
                //assert
                var actual = context.Suppliers.Find(supplier.ID);

                Assert.NotNull(actual);
                Assert.Equal(supplier.Name, actual.Name);
                Assert.Equal(supplier.Description, actual.Description);
                Assert.Equal(supplier.IsActive, actual.IsActive);
            }
        }

        [Fact]
        public void Delete_WithValidSupplierID_RetrieveShouldReturnNull()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                .UseInMemoryDatabase($"SupplierForTesting{Guid.NewGuid()}")
                .Options;

            var supplier = new Supplier
            {
                Name = "Annie's Sweets",
                Description = "Manufacturing and Packaging",
                IsActive = true
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Suppliers.Add(supplier);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new SupplierRepository(context);

                //act
                sut.Delete(supplier.ID);

                var actual = context.Suppliers.Find(supplier.ID);

                //assert
                Assert.Null(actual);
            }
        }

        [Fact]
        public void Retrieve_WithValidSupplierID_ShouldReturnTheSupplier()
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


            var supplier = new Supplier
            {
                Name = "Annie's Sweets",
                Description = "Manufacturing and Packaging",
                IsActive = true
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Suppliers.Add(supplier);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new SupplierRepository(context);

                //act
                var actual = sut.Retrieve(supplier.ID);

                //assert
                Assert.NotNull(actual);
                Assert.Equal(supplier.Name, actual.Name);
                Assert.Equal(supplier.Description, actual.Description);
                Assert.Equal(supplier.IsActive, actual.IsActive);
            }
        }

        [Fact]
        public void Retrieve_WithSkipAndCount_ShouldReturnCorrectListOfSuppliers()
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
                    context.Suppliers.Add(new Supplier
                    {
                        Name = string.Format("Category {0}", i),
                        Description = string.Format("Description {0}", i),
                        IsActive = true
                    });
                }

                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new SupplierRepository(context);

                //act
                var list = sut.Retrieve(5, 5);

                //assert
                Assert.True(list.Count() == 5);  
            }
        }

        [Fact]
        public void Update_WithValidSupplier_RetrieveShouldReturnUpdatedSupplier()
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


            var oldSupplier = new Supplier
            {
                Name = "Rebisco",
                Description = "Food Supplier",
                IsActive = false
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Suppliers.Add(oldSupplier);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new SupplierRepository(context);

                var expected = context.Suppliers.Find(oldSupplier.ID);
                expected.Name = "Fibisco";
                expected.Description = "Food Supplier/Manufacturer";
                expected.IsActive = true;

                //act
                sut.Update(expected.ID, expected);

                //assert
                var actual = sut.Retrieve(oldSupplier.ID);

                Assert.Equal(actual.Name, expected.Name);
                Assert.Equal(actual.Description, expected.Description);
                Assert.Equal(actual.IsActive, expected.IsActive); 
            }
        }
    }
}
