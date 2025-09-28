using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.User_side
{
    public interface IUserProductControllerRepository
    {
        IEnumerable<Products> Filters(IEnumerable<Products> source, ProductFilterOptions options);
    }
}
