namespace WEB.Models
{
    public class CartItemListViewModel
    {
        public required IEnumerable<CartItemViewModel> CartItems { get; init; }
    }
}

