using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using QuickReach.ECommerce.Infra.Data.Repositories;
using QuickReach.ECommerce.Domain.Models;

namespace QuickReach.ECommerce.Infra.Data.Tests
{
    public class SupplierRepositoryTest
    {
        [Fact]
        public void Create_WithValidSupplier_ShouldAddSupplierToDatabase()
        {
            //arrange
            var context = new ECommerceDbContext();
            var sut = new SupplierRepository(context);
            var supplier = new Supplier
            {
                Name = "Annie's Sweets",
                Description = "Manufacturing and Packaging",
                IsActive = true
            };

            //act
            sut.Create(supplier);

            //assert
            var actual = sut.Retrieve(supplier.ID);

            Assert.NotNull(actual);
            Assert.True(actual.ID != 0);

            //cleanup
            sut.Delete(actual.ID);
        }

        [Fact]
        public void Delete_WithValidSupplierID_RetrieveShouldReturnNull()
        {
            //arrange
            var context = new ECommerceDbContext();
            var sut = new SupplierRepository(context);
            var supplier = new Supplier
            {
                Name = "Annie's Sweets",
                Description = "Manufacturing and Packaging",
                IsActive = true
            };

            sut.Create(supplier);

            //act
            sut.Delete(supplier.ID);

            //assert
            Assert.Null(sut.Retrieve(supplier.ID));
        }

        [Fact]
        public void Retrieve_WithValidSupplierID_ShouldReturnTheSupplier()
        {
            //arrange
            var context = new ECommerceDbContext();
            var sut = new SupplierRepository(context);
            var supplier = new Supplier
            {
                Name = "Annie's Sweets",
                Description = "Manufacturing and Packaging",
                IsActive = true
            };
            sut.Create(supplier);

            //act
            var actual = sut.Retrieve(supplier.ID);

            //assert
            Assert.NotNull(actual);

            //cleanup
            sut.Delete(supplier.ID);
        }

        [Fact]
        public void Retrieve_WithSkipAndCount_ShouldReturnCorrectListOfSuppliers()
        {
            //arrange
            var context = new ECommerceDbContext();
            var sut = new SupplierRepository(context);
            for (int i = 1; i <= 20; i += 1)
            {
                sut.Create(new Supplier
                {
                    Name = string.Format("Category {0}", i),
                    Description = string.Format("Description {0}", i),
                    IsActive = true
                });
            }

            //act
            var list = sut.Retrieve(5, 5);

            //assert
            Assert.True(list.Count() == 5);

            //cleanup
            list = sut.Retrieve(0, 20);
            foreach (var i in list)
            {
                sut.Delete(i.ID);
            }
        }

        [Fact]
        public void Update_WithValidSupplier_RetrieveShouldReturnUpdatedSupplier()
        {
            //arrange
            var context = new ECommerceDbContext();
            var sut = new SupplierRepository(context);

            var oldSupplier = new Supplier
            {
                Name = "Rebisco",
                Description = "Food Supplier",
                IsActive = false
            };
            sut.Create(oldSupplier);

            var expected = sut.Retrieve(oldSupplier.ID);
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

            //cleanup
            sut.Delete(expected.ID);
        }
    }
}
