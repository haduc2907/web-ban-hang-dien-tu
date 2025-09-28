using Entities;
using UseCase.Admin_side;
using UseCase.User_side;

namespace Infractructure
{
    public class InMemoryProductRepository : IAdminProductControllerRepository, IUserProductControllerRepository
    {
        private readonly List<Products> products;
        public InMemoryProductRepository()
        {
            products = [];
        }
        public void Add(Products product)
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
        public IEnumerable<Products> GetAll()
        {
            return products;
        }

        public IEnumerable<Products> GetByCategoryId(int categoryId)
        {
            return products.Where(product => product.CategoryId == categoryId).ToList();
        }

        public Products? GetById(int id)
        {
            return products.FirstOrDefault(product => product.Id == id);
        }
        public void Update(Products product)
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
        public IEnumerable<Products> Filters(IEnumerable<Products> source, ProductFilterOptions options)
        {
            var query = source.AsQueryable();

            // 1. Keyword
            if (!string.IsNullOrWhiteSpace(options.Keyword))
            {
                query = query.Where(p =>
                    p.Name.Contains(options.Keyword, StringComparison.OrdinalIgnoreCase));
            }

            // 2. Price
            if (options.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= options.MinPrice.Value);
            }
            if (options.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= options.MaxPrice.Value);
            }

            // 3. Brand
            if (!string.IsNullOrWhiteSpace(options.Brand))
            {
                query = query.Where(p =>
                    p.Brand.Equals(options.Brand, StringComparison.OrdinalIgnoreCase));
            }

            // 4. Status
            if (options.Status.HasValue)
            {
                query = query.Where(p => p.Status == options.Status.Value);
            }

            return query.ToList();
        }

        public IEnumerable<Products> Find(IEnumerable<Products> source, string? keyword)
        {
            if (keyword == null)
                return source;
            return source.Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
        }

    }
}
