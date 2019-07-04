using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace QuickReach.ECommerce.Domain.Models
{
    [Table("Cart")]
    public class Cart : EntityBase
    {
        //[Required]
        public int CustomerId { get; set; }
        public List<CartItem> Items { get; set; }

        public Cart(int customerId)
        {
            CustomerId = customerId;
            Items = new List<CartItem>();
        }

        public void AddProductToCart(CartItem cartitem)
        {
            ((ICollection<CartItem>)this.Items).Add(cartitem);
        }

        public void RemoveProductFromCart(int productId)
        {
            var cartitem = this.GetCartItems(productId);

            ((ICollection<CartItem>)this.Items).Remove(cartitem);
        }

        public CartItem GetCartItems(int cartId)
        {
            return ((ICollection<CartItem>)this.Items)
                    .FirstOrDefault(pc => pc.CartId == cartId);
        }
    }
}
