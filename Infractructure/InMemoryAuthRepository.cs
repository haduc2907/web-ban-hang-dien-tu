using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase;

namespace Infractructure
{
    public class InMemoryAuthRepository : IAuthRepository
    {
        private readonly List<Users> users;
        public InMemoryAuthRepository()
        {
            users = [];
        }
        public Users? Login(string username, string password)
        {
            var user = users.FirstOrDefault(u => u.UserName == username && u.Password == password);
            return user;
        }

        public void Register(Users user)
        {
            users.Add(user);
        }
    }
}
