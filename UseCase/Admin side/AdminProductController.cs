using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Admin_side
{
    public class AdminProductController
    {
        private readonly IAdminProductControllerRepository repo;
        public AdminProductController(IAdminProductControllerRepository repo)
        {
            this.repo = repo;
        }
        public void Add(Products product)
        {
            repo.Add(product);
        }
        public void Delete(int id)
        {
            repo.Delete(id);
        }
        public Products? GetById(int id)
        {
            return repo.GetById(id);
        }
        public void Update(Products product)
        {
            repo.Update(product);
        }
        public IEnumerable<Products> GetAll()
        {
            return repo.GetAll();
        }
        public IEnumerable<Products> GetByCategoryId(int categoryId)
        {
            return repo.GetByCategoryId(categoryId);
        }
    }
}
