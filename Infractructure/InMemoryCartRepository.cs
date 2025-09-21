using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.User_side;

namespace Infractructure
{
    public class InMemoryCartRepository : ICartControllerRepository
    {
        private readonly List<Cart> carts;
        public InMemoryCartRepository()
        {
            carts = [];
        }

        public void AddToCart(Product product, int? userId)
        {
            var cart = carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Items = new List<CartItem>()
                };
                carts.Add(cart);
            }

            var item = cart.Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                cart.Items.Add(new CartItem()
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Quantity = 1
                });
            }


        }

        public void Clear(int userId)
        {
            var cart = carts.FirstOrDefault(c => c.UserId == userId);
            if (cart != null)
            cart.Items.Clear();
        }


        public void Delete(int id, int userId)
        {
            var cart = carts.FirstOrDefault(c => c.UserId == userId);
            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(c => c.ProductId == id);
                if (item != null)
                {
                    cart.Items.Remove(item);
                }
            }

        }


        public List<CartItem> GetAll(int? userId)
        {
            var cart = carts.FirstOrDefault(c => c.UserId == userId);
            if (cart != null)
                return cart.Items;
            return [];
        }

        public void UpdateQuantity(int id, int quantity, int userId)
        {
            var cart = carts.FirstOrDefault(c => c.UserId == userId);
            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(c => c.ProductId == id);
                if (item != null)
                {
                    item.Quantity += quantity;
                }
            }
            
        }
    }
}
