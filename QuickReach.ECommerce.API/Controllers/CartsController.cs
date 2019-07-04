using Microsoft.AspNetCore.Mvc;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickReach.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository repository;
        private readonly IProductRepository productRepository;

        public CartsController(
            ICartRepository repository,
            IProductRepository productRepository)
        {
            this.repository = repository;
            this.productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Get(string search = "", int skip = 0, int count = 10)
        {
            var carts = repository.Retrieve(search, skip, count);
            return Ok(carts);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var cart = this.repository.Retrieve(id);
            return Ok(cart);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Cart newCart)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Create(newCart);

            return CreatedAtAction(nameof(this.Get), new { id = newCart.ID }, newCart);
        }

        [HttpPut("{id}/items/{qty}")]
        public IActionResult PostCartItem(int id, [FromBody] int productId, int qty)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }
            var cart = repository.Retrieve(id);
            if (cart == null)
            {
                return NotFound();
            }
            if (productRepository.Retrieve(productId) == null)
            {
                return NotFound();
            }

            var product = productRepository.Retrieve(productId);

            var cartitem = new CartItem
            {
                CartId = id,
                ProductId = Convert.ToInt32(product.ID),
                ProductName = product.Name,
                ImageUrl = product.ImageURL,
                Quantity = qty,
                OldUnitPrice = product.Price,
                UnitPrice = product.Price
            };

            cart.AddProductToCart(cartitem);
            repository.Update(id, cart);
            return Ok(cart);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Cart cart)
        {
            //400
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //404 check if exists
            var entity = this.repository.Retrieve(id);
            if (entity == null)
            {
                return NotFound();
            }

            this.repository.Update(id, cart);

            //200
            return Ok(cart);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.repository.Delete(id);

            return Ok();
        }
    }
}
