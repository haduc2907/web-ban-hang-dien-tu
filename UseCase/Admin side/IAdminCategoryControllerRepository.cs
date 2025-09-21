using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Admin_side
{
    public interface IAdminCategoryControllerRepository
    {
        void Add(Category category);
        void Delete(int categoryId);
        Category? GetById(int categoryId);
        List<Category> GetAll();
    }
}
