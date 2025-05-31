using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyNgayLaoDong.Areas.LopTruong.ViewModel
{
    public class TaiKhoanLopTruongViewModel
    {
        public int MSSV { get; set; }     // mã số sinh viên
        public string HoTen { get; set; }
        public string Email { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string GioiTinh { get; set; }  // đổi từ bool? thành string
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string VaiTro { get; set; }
        public string AnhDaiDien { get; set; }  // đổi từ int? thành string (đường dẫn ảnh)
    }
}