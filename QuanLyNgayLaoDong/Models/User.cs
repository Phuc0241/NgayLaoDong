using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyNgayLaoDong.Models
{
    public class User
    {
        public int TaiKhoanId { get; set; }    // ID tài khoản (từ bảng TaiKhoan)
        public string Username { get; set; }    // Tên đăng nhập (từ bảng TaiKhoan)
        public string Password { get; set; }    // Mật khẩu (từ bảng TaiKhoan)
        public string Email { get; set; }       // Email (từ bảng TaiKhoan)
        public string Role { get; set; }        // Vai trò (từ bảng VaiTro)
        public int? SinhVienId { get; set; }    // ID sinh viên nếu có
        public int? QuanLyId { get; set; }      // ID quản lý nếu có
        public string HoTen { get; set; }
        public string Avatar { get; set; }
        // lấy từ bảng SinhVien khi role == "SinhVien"

    }
}