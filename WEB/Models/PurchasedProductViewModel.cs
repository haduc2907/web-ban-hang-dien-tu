using Entities;

namespace WEB.Models
{
    public class PurchasedProductViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime PurchasedDate { get; set; }
        public EOrderStatus Status { get; set; } = EOrderStatus.PendingConfirmation;
    }
}
