using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Admin_side;

namespace Infractructure
{
    public class InMemoryCategoryRepository : IAdminCategoryControllerRepository
    {
        private readonly List<Category> categories;
        public InMemoryCategoryRepository()
        {
            categories = [];
        }
        public void Add(Category category)
        {
            categories.Add(category);
        }

        public void Delete(int categoryId)
        {
            var category = categories.FirstOrDefault(c => c.Id == categoryId);
            if (category != null)
            {
                categories.Remove(category);
            }
        }

        public List<Category> GetAll()
        {
            return categories;
        }

        public Category? GetById(int categoryId)
        {
            var category = categories.FirstOrDefault(c => c.Id == categoryId);
            return category;
        }
    }
}
