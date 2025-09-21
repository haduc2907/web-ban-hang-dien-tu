using Entities;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace WEB.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ và tên tối đa 100 ký tự")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại chỉ được chứa ký tự số")]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public ESRoleUser Role { get; set; } 
        public DateTime CreatedDate { get; set; }
    }
}
