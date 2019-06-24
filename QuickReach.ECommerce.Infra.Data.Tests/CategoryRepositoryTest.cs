using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repositories;
using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace QuickReach.ECommerce.Infra.Data.Tests
{
    public class CategoryRepositoryTest
    {
        [Fact]
        public void Create_WithValidEntity_ShouldCreateDatabaseRecord()
        {
            //arrange
            var context = new ECommerceDbContext();

            var sut = new CategoryRepository(context);
            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };

            //act
            sut.Create(category);

            //assert
            Assert.True(category.ID != 0);

            var entity = sut.Retrieve(category.ID);
            Assert.NotNull(entity);

            //cleanup
            sut.Delete(category.ID);
        }

        [Fact]
        public void Retrieve_WithValidEntityID_ReturnsAValidEntity()
        {
            //arrange
            var context = new ECommerceDbContext();
            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };
            var sut = new CategoryRepository(context);
            sut.Create(category);

            //act
            var actual = sut.Retrieve(category.ID);

            //assert
            Assert.NotNull(actual);


            //cleanup
            sut.Delete(actual.ID);
        }

        [Fact]
        public void Retrieve_WithNonExistingEntityID_ReturnsNull()
        {
            //arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);

            //act
            var actual = sut.Retrieve(-1);

            //assert
            Assert.Null(actual);
        }

        [Fact]
        public void Retrieve_WithSkipAndCount_ReturnsTheCorrectPage()
        {
            //arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);
            for (int i = 1; i <= 20; i += 1)
            {
                sut.Create(new Category
                {
                    Name = string.Format("Category {0}", i),
                    Description = string.Format("Description {0}", i)
                });
            }

            //act
            var list = sut.Retrieve(5, 5);

            //assert
            Assert.True(list.Count() == 5);

            //cleanup
            list = sut.Retrieve(0, 20);
            foreach(var i in list)
            {
                sut.Delete(i.ID);
            }
        }

        [Fact]
        public void Delete_WithValidID_RetrieveShouldReturnNull()
        {
            //arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);
            var category = new Category
            {
                Name = "#1",
                Description = "Love Radio"
            };
            sut.Create(category);

            //act
            sut.Delete(category.ID);
            var actual = sut.Retrieve(category.ID);

            //assert
            Assert.Null(actual);
        }

        [Fact]
        public void Update_WithValidEntity_RetrieveShouldReturnUpdatedEntity()
        {
            //arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);

            var oldCategory1 = new Category
            {
                Name = "Sweets",
                Description = "Matamis yo."
            };
            sut.Create(oldCategory1);

            var oldCategory = new Category
            {
                Name = "#1",
                Description = "Love Radio"
            };
            sut.Create(oldCategory);

            var actual = sut.Retrieve(oldCategory.ID);
            actual.Name = "#2";
            actual.Description = "Yes FM";

            //act
            sut.Update(actual.ID, actual);

            //assert
            var UpdatedCategory = sut.Retrieve(oldCategory.ID);

            Assert.Equal(UpdatedCategory.Name, actual.Name);
            Assert.Equal(UpdatedCategory.Description, actual.Description);

            //cleanup
            sut.Delete(actual.ID);
        }
    }
}
