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
        void Add(Categories category);
        void Delete(int categoryId);
        Categories? GetById(int categoryId);
        List<Categories> GetAll();
        void Update(Categories category);
    }
}
