using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Admin_side
{
    public class UserController
    {
        private readonly IUserControllerRepository repo;
        public UserController(IUserControllerRepository repo)
        {
            this.repo = repo;
        }
        public User? GetById(int userId)
        {
            return repo.GetById(userId);
        }
        public List<User> GetAll()
        {
            return repo.GetAll();
        }
        public void Delete(int userId)
        {
            repo.Delete(userId);
        }
        public void Add(User user)
        {
            repo.Add(user);
        }
    }
}
