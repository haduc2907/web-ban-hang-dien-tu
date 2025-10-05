using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Admin_side
{
    public class OrderController
    {
        private readonly IOrderControllerRepository _orderRepository;
        public OrderController(IOrderControllerRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public void Add(Orders order)
        {
            _orderRepository.Add(order);
        }
        public List<Orders> GetAllOrdersByUserId(int userId)
        {
            return _orderRepository.GetAllOrdersByUserId(userId);
        }
        public IEnumerable<Orders> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }
    }
}
