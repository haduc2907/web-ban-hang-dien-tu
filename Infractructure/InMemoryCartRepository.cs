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
        private readonly List<CartItems> carts;
        public InMemoryCartRepository()
        {
            carts = [];
        }

        public void AddToCart(Products product, int? userId)
        {
            var item = carts.FirstOrDefault(c => c.ProductId == product.Id && c.UserId == userId);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                carts.Add(new CartItems()
                {
                    UserId = userId ?? 0,
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
            carts.RemoveAll(c => c.UserId == userId);
        }


        public void Delete(int id, int userId)
        {
            var cart = carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == id);
            if (cart != null)
                carts.Remove(cart);

        }


        public List<CartItems> GetAll(int? userId)
        {
            return carts.Where(c => c.UserId == userId).ToList();
        }

        public CartItems? GetById(int productId, int userId)
        {
            return carts.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);
        }

        public void UpdateQuantity(int id, int quantity, int userId)
        {
            var cart = carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == id);
            if (cart != null)
            {
                cart.Quantity += quantity;

                if (cart.Quantity <= 0)
                {
                    // Nếu số lượng <= 0 thì tự động xóa khỏi giỏ
                    carts.Remove(cart);
                }
            }
            
        }
    }
}
