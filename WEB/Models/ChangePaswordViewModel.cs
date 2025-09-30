using System.ComponentModel.DataAnnotations;

namespace WEB.Models
{
    public class ChangePaswordViewModel
    {
        [Required(ErrorMessage = "Mật khẩu cũ không được để trống")]
        public string OldPassword { get; set; } = string.Empty;
        [Required(ErrorMessage = "Mật khẩu mới không được để trống")]
        public string NewPassword { get; set; } = string.Empty;
        [Required(ErrorMessage = "Nhập lại mật khẩu không được để trống")]
        public string ReEnterPassword { get; set; } = string.Empty;
    }
}
