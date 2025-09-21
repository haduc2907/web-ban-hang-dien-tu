using Entities;

namespace WEB.Models
{
    public class CartViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }    // hoặc string UserName
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public List<CartItem> Items { get; set; } = new();
    }
}
