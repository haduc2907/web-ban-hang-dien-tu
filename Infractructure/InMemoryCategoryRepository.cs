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
        private readonly List<Categories> categories;
        public InMemoryCategoryRepository()
        {
            categories = [];
        }
        public void Add(Categories category)
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

        public List<Categories> GetAll()
        {
            return categories;
        }

        public Categories? GetById(int categoryId)
        {
            var category = categories.FirstOrDefault(c => c.Id == categoryId);
            return category;
        }

        public void Update(Categories category)
        {
            var cat = categories.FirstOrDefault(c => c.Id == category.Id);
            if (cat != null)
            {
                cat.Name = category.Name;
            }
        }
    }
}
