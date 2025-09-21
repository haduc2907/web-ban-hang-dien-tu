using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Admin_side
{
    public interface IUserControllerRepository
    {
        User? GetById(int userId);
        void Add(User user);
        List<User> GetAll();
        void Delete(int userId);
    }
}
