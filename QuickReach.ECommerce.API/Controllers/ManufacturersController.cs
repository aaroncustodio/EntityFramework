using Dapper;
using Microsoft.AspNetCore.Mvc;
using QuickReach.ECommerce.API.ViewModel;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace QuickReach.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturersController : ControllerBase
    {
        private readonly IManufacturerRepository repository;
        private readonly IProductRepository productRepository;

        public ManufacturersController(
            IManufacturerRepository repository,
            IProductRepository productRepository)
        {
            this.repository = repository;
            this.productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Get(string search = "", int skip = 0, int count = 10)
        {
            var manufacturers = repository.Retrieve(search, skip, count);
            return Ok(manufacturers);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var manufacturer = this.repository.Retrieve(id);
            return Ok(manufacturer);
        }

        [HttpGet("{id}/products")]
        public IActionResult GetProductsByCategory(int id)
        {
            var connString = "Server=.;Database=QuickReachDb;Integrated Security=true;";
            var conn = new SqlConnection(connString);

            var parameter = new DynamicParameters();
            parameter.Add("@manufacturerId", id);

            var result = conn.Query<SearchItemViewModel>(
                @"SELECT
                        pc.ProductID, 
                        pc.ManufacturerID, 
                        p.Name, 
                        p.Description, 
                        p.Price, 
                        p.ImageURL
                  FROM  Product p INNER JOIN ProductManufacturer pc
                  ON    p.ID = pc.ProductID
                  WHERE pc.ManufacturerID = @manufacturerId", parameter)
                  .ToList();

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Manufacturer newManufacturer)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Create(newManufacturer);

            return CreatedAtAction(nameof(this.Get), new { id = newManufacturer.ID }, newManufacturer);
        }

        [HttpPut("{id}/products")]
        public IActionResult PostManufacturerProduct(int id, [FromBody] ProductManufacturer entity)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }
            var manufacturer = repository.Retrieve(id);
            if (manufacturer == null)
            {
                return NotFound();
            }
            if (productRepository.Retrieve(entity.ProductID) == null)
            {
                return NotFound();
            }

            //adds the productmanufacturer to the Manufacturer's IEnumerable ProductManufacturer property
            manufacturer.AddProduct(entity);

            repository.Update(id, manufacturer);
            return Ok(manufacturer);
        }

        [HttpPut("{id}/products/{productId}")]
        public IActionResult DeleteManufacturerProduct(int id, int productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var manufacturer = repository.Retrieve(id);
            if (manufacturer == null)
            {
                return NotFound();
            }
            if (productRepository.Retrieve(productId) == null)
            {
                return NotFound();
            }
            manufacturer.RemoveProduct(productId);
            repository.Update(id, manufacturer);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Manufacturer manufacturer)
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

            this.repository.Update(id, manufacturer);

            //200
            return Ok(manufacturer);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.repository.Delete(id);

            return Ok();
        }
    }
}
