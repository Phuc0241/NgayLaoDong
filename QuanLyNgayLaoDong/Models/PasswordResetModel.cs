using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyNgayLaoDong.Models
{
    public class PasswordResetModel
    {
        public string Email { get; set; }

        // Có thể dùng thêm nếu bạn xác nhận qua token
        public string Token { get; set; }

        public string ResetLink { get; set; }
    }

}