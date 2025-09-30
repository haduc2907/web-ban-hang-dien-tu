using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.User_side
{
    public interface IUserReviewControllerRepository
    {
        void Add(Reviews review);
        void Update(Reviews review);
        void Delete(int id);
        Reviews? GetById(int id);
        List<Reviews> GetAll();
        
    }
}
