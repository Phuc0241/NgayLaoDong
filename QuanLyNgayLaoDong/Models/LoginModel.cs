using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; // Quan trọng: Phải thêm namespace này!

namespace QuanLyNgayLaoDong.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Tài khoản không được để trống.")]
        [Display(Name = "Tài khoản")] // Tên hiển thị thân thiện cho trường này
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [DataType(DataType.Password)] // Đảm bảo rằng đây là trường mật khẩu
        [Display(Name = "Mật khẩu")] // Tên hiển thị thân thiện cho trường này
        public string Password { get; set; }

        // Trường Role này thường được thiết lập sau khi đăng nhập thành công,
        // không cần validation khi người dùng nhập vào.
        public string Role { get; set; }
    }
}