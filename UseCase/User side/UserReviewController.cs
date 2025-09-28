using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.User_side
{
    public class UserReviewController
    {
        private readonly IUserReviewControllerRepository repo;
        public UserReviewController(IUserReviewControllerRepository repo)
        {
            this.repo = repo;
        }
        public void Add(Reviews review)
        {
            repo.Add(review);
        }
        public void Update(Reviews review)
        {
            repo.Update(review);
        }
        public void Delete(int id)
        {
            repo.Delete(id);
        }
        public Reviews? GetById(int id)
        {
            return repo.GetById(id);
        }
        public List<Reviews> GetAll()
        {
            return repo.GetAll();
        }
    }
}
