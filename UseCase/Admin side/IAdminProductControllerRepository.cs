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
        void Add(Products product);
        void Update(Products product);
        void Delete(int Id);
        Products? GetById(int Id);
        IEnumerable<Products> GetAll();
        IEnumerable<Products> GetByCategoryId(int categoryId);
    }
}
