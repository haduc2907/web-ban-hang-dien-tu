using Entities;

namespace Entities
{
    public class Product
    {
        private static int count = 0;
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public EStatusProduct Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; }
        public int CategoryId { get; set; } 
        public string Brand { get; set; } = string.Empty;
        public Product()
        {
            this.Id = ++count;
        }
    }
}
