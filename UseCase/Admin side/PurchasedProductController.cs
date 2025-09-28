using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Admin_side
{
    public class PurchasedProductController
    {
        private readonly IPurchasedProductControllerRepository repo;
        public PurchasedProductController(IPurchasedProductControllerRepository repo)
        {
            this.repo = repo;
        }
        public void Add(PurchasedProducts product)
        {
            repo.Add(product);
        }
        public void Delete(int id)
        {
            repo.Delete(id);
        }
        public PurchasedProducts? GetById(int id)
        {
            return repo.GetById(id);
        }
        public void Update(PurchasedProducts product)
        {
            repo.Update(product);
        }
        public List<PurchasedProducts> GetAll()
        {
            return repo.GetAll();
        }
    }
}
