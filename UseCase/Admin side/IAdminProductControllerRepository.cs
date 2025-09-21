using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Admin_side
{
    public interface IAdminProductControllerRepository
    {
        void Add(Product product);
        void Update(Product product);
        void Delete(int Id);
        Product? GetById(int Id);
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetByCategoryId(int categoryId);
    }
}
