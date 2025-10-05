using Entities;

using UseCase.Admin_side;

namespace Infractructure
{
    public class InMemoryPurchasedProductRepostiory : IPurchasedProductControllerRepository
    {
        private readonly List<PurchasedProducts> products;
        public InMemoryPurchasedProductRepostiory()
        {
            products = [];
        }
        public void Add(PurchasedProducts product)
        {
            products.Add(product);
        }

        public void Delete(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                products.Remove(product);
            }
        }

        public List<PurchasedProducts> GetAll()
        {
            return products;
        }

        public List<PurchasedProducts> GetAllByOrderId(int orderId)
        {
            throw new NotImplementedException();
        }

        public PurchasedProducts? GetById(int id)
        {
            return products.FirstOrDefault(p => p.Id == id);
        }

        public void Update(PurchasedProducts pProduct)
        {
            var product = products.FirstOrDefault(p => p.Id == pProduct.Id);
            if (product != null)
            {
                product.UserId = pProduct.UserId;
                product.ProductId = pProduct.ProductId;
                product.Name = pProduct.Name;
                product.ImageUrl = pProduct.ImageUrl;
                product.Price = pProduct.Price;
                product.Quantity = pProduct.Quantity;
                product.PurchasedDate = pProduct.PurchasedDate;
                product.Status = pProduct.Status;
            }
        }
            
    }
}
