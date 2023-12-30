using System.ComponentModel.DataAnnotations;

namespace CHERRY.BUS.ViewModels.User
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Tài khoản là bắt buộc.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        public string? PassWord { get; set; }
    }
}
