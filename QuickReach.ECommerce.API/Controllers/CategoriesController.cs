using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;

namespace QuickReach.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository repository;

        private readonly IProductRepository productRepository;

        public CategoriesController(
            ICategoryRepository repository, 
            IProductRepository productRepository)
        {
            this.repository = repository;
            this.productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Get(string search ="", int skip = 0, int count = 10)
        {
            var categories = repository.Retrieve(search, skip, count);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = this.repository.Retrieve(id);
            return Ok(category);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Category newCategory)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Create(newCategory);

            return CreatedAtAction(nameof(this.Get), new {id=newCategory.ID}, newCategory);
        }

        [HttpPut("{id}/products")]
        public IActionResult PostCategoryProduct(int id, [FromBody] ProductCategory entity)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }
            var category = repository.Retrieve(id);
            if (category == null)
            {
                return NotFound();
            }
            if (productRepository.Retrieve(entity.ProductID)== null)
            {
                return NotFound();
            }

            //adds the productcategory to the categories IEnumerable ProductCategory property
            category.AddProduct(entity);

            repository.Update(id, category);
            return Ok(category);
        }

        [HttpPut("{id}/products/{productId}")]
        public IActionResult DeleteCategoryProduct(int id, int productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var category = repository.Retrieve(id);
            if (category == null)
            {
                return NotFound();
            }
            if (productRepository.Retrieve(productId) == null)
            {
                return NotFound();
            }
            category.RemoveProduct(productId);
            repository.Update(id, category);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Category category)
        {
            //400
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //404
            var entity = this.repository.Retrieve(id);
            if (entity == null)
            {
                return NotFound();
            }

            this.repository.Update(id, category);

            //200
            return Ok(category);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.repository.Delete(id);

            return Ok();
        }
    }
}