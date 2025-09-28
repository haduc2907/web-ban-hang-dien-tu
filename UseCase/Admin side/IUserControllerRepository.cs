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
        Users? GetById(int userId);
        void Add(Users user);
        List<Users> GetAll();
        void Delete(int userId);
        void Update(Users user);
    }
}
