using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.User_side
{
    public class CartController
    {
        private readonly ICartControllerRepository repo;
        public CartController(ICartControllerRepository repo)
        {
            this.repo = repo;
        }
        public List<CartItems> GetAll(int? userId)
        {
            return repo.GetAll(userId);
        }
        public void AddToCart(Products product, int? userId)
        {
            repo.AddToCart(product, userId);
        }
        public void Delete(int id, int userId)
        {
            repo.Delete(id, userId);
        }
        public void UpdateQuantity(int id, int quantity, int userId)
        {
            repo.UpdateQuantity(id, quantity, userId);
        }
        public void Clear(int userId)
        {
            repo.Clear(userId);
        }
        public CartItems? GetById(int productId, int userId)
        {
            return repo.GetById(productId, userId);
        }
    }
}
