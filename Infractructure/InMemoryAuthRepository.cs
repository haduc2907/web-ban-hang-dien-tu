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
        private readonly List<User> users;
        public InMemoryAuthRepository()
        {
            users = [];
        }
        public User? Login(string username, string password)
        {
            var user = users.FirstOrDefault(u => u.UserName == username && u.Password == password);
            return user;
        }

        public void Register(User user)
        {
            users.Add(user);
        }
    }
}
