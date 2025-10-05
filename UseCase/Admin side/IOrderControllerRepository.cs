using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Admin_side
{
    public interface IOrderControllerRepository
    {
        void Add(Orders order);
        IEnumerable<Orders> GetAllOrders();
        List<Orders> GetAllOrdersByUserId(int userId);

    }
}
