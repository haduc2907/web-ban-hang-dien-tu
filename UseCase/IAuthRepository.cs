using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase
{
    public interface IAuthRepository
    {
        void Register(User user);
        User? Login(string username, string password);
    }
}
