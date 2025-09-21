using Entities;

namespace WEB.Models
{
    public class ProductListViewModel
    {
        public required IEnumerable<ProductViewModel> Products { get; init; }
        public required IEnumerable<CategoryViewModel> Categories { get; init; }
        public int? SelectedCategoryId { get; set; }
    }
}
