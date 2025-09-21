namespace WEB.Models
{
    public class UserListViewModel
    {
        public required IEnumerable<UserViewModel> Users { get; init; }
    }
}
