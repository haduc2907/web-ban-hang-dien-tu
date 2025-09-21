using Entities;
using UseCase.Admin_side;

namespace Infractructure
{
    public class InMemoryProductRepository : IAdminProductControllerRepository
    {
        private readonly List<Product> products;
        public InMemoryProductRepository()
        {
            products = [];
        }
        public void Add(Product product)
        {
            products.Add(product);
        }
        public void Delete(int id)
        {
            var product = products.FirstOrDefault(product => product.Id == id);
            if (product != null)
            {
                products.Remove(product);
            }
        }
        public IEnumerable<Product> GetAll()
        {
            return products;
        }

        public IEnumerable<Product> GetByCategoryId(int categoryId)
        {
            return products.Where(product => product.CategoryId == categoryId).ToList();
        }

        public Product? GetById(int id)
        {
            return products.FirstOrDefault(product => product.Id == id);
        }
        public void Update(Product product)
        {
            var p = products.FirstOrDefault(p => p.Id == product.Id);
            if (p != null)
            {
                product.Name = p.Name;
                product.Description = p.Description;
                product.Price = p.Price;
                product.ImageUrl = p.ImageUrl;
                product.Quantity = p.Quantity;
                product.Status = p.Status;
                product.UpdatedDate = DateTime.Now;
                product.Brand = p.Brand;
                product.CategoryId = p.CategoryId;
            }
        }
    }
}
