using Entities;

namespace WEB.Models
{
    public class PurchasedProductListViewModel
    {
        public required IEnumerable<PurchasedProductViewModel> PurchasedProducts { get; init; }
        public EOrderStatus? FilterStatus { get; set; }
    }
}
