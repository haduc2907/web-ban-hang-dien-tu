using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Admin_side
{
    public class AdminCategoryController
    {
        private readonly IAdminCategoryControllerRepository repo;
        public AdminCategoryController(IAdminCategoryControllerRepository repo)
        {
            this.repo = repo;
        }
        public void Add(Category category)
        {
            repo.Add(category);
        }
        public void Delete(int categoryId)
        {
            repo.Delete(categoryId);
        }
        public List<Category> GetAll()
        {
            return repo.GetAll();
        }
        public Category? GetById(int categoryId)
        {
            return repo.GetById(categoryId);
        }
    }
}
