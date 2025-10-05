using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Admin_side
{
    public interface IPurchasedProductControllerRepository
    {
        void Add(PurchasedProducts product);
        void Delete(int id);
        PurchasedProducts? GetById(int id);
        void Update(PurchasedProducts product);
        List<PurchasedProducts> GetAll();
        List<PurchasedProducts> GetAllByOrderId(int orderId);

    }
}
