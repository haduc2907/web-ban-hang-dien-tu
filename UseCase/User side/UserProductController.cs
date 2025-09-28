using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.User_side
{
    public class UserProductController
    {
        private readonly IUserProductControllerRepository repo;
        public UserProductController(IUserProductControllerRepository repo)
        {
            this.repo = repo;
        }

        public IEnumerable<Products> Filters(IEnumerable<Products> source, ProductFilterOptions options)
        {
            return repo.Filters(source, options);
        }
    }
}
