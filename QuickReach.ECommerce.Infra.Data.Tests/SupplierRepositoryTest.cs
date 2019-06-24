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
                Description = "We make your childhood sweeter!",
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
    }
}
