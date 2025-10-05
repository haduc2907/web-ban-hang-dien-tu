using Entities;

namespace WEB.Models
{
    public class OrderListViewModel
    {
        public required IEnumerable<OrderViewModel> Orders { get; init; }
    }
}
