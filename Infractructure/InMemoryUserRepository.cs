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
        private readonly List<Users> users;
        public InMemoryUserRepository()
        {
            users = [];
        }
        public void Add(Users user)
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

        public List<Users> GetAll()
        {
            return users;
        }

        public Users? GetById(int userId)
        {
            return users.FirstOrDefault(u => u.Id == userId);
        }

        public void Update(Users user)
        {
            var usere = users.FirstOrDefault(u => u.Id == user.Id);
            if (usere != null)
            {
                usere.UserName = user.UserName;
                usere.Password = user.Password;
                usere.Email = user.Email;
                usere.FullName = user.FullName;
                usere.PhoneNumber = user.PhoneNumber;
                usere.Address = user.Address;
                usere.Role = user.Role;
            }
        }
    }
}
