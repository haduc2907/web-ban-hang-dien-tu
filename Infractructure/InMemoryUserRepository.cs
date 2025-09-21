using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Admin_side;

namespace Infractructure
{
    public class InMemoryUserRepository : IUserControllerRepository
    {
        private readonly List<User> users;
        public InMemoryUserRepository()
        {
            users = [];
        }
        public void Add(User user)
        {
            users.Add(user);
        }

        public void Delete(int userId)
        {
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                users.Remove(user);
            }
        }

        public List<User> GetAll()
        {
            return users;
        }

        public User? GetById(int userId)
        {
            return users.FirstOrDefault(u => u.Id == userId);
        }
    }
}
