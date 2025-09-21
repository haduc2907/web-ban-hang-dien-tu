using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.User_side
{
    public interface ICartControllerRepository
    {
        List<CartItem> GetAll(int? userId);
        void AddToCart(Product product, int? userID);
        void Delete(int id, int userId);
        void UpdateQuantity(int id, int quantity, int userId);
        void Clear(int userId);
    }
}
