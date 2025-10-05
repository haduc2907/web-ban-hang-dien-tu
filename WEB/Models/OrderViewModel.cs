using Entities;

namespace WEB.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<PurchasedProducts> PurchasedProducts { get; set; } = [];
        public string UserName { get; set; } = string.Empty;
    }
}
