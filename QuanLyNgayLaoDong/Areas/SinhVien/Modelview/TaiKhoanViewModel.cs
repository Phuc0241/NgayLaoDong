using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyNgayLaoDong.Areas.SinhVien.Modelview
{
    public class TaiKhoanViewModel
    {
        //public string MaSo { get; set; }          // MSSV hoặc mã quản lý
        //public string HoTen { get; set; }
        //public string Email { get; set; }
        //public DateTime NgaySinh { get; set; }
        //public string GioiTinh { get; set; }
        //public string DiaChi { get; set; }
        //public string SoDienThoai { get; set; }
        //public string VaiTro { get; set; }        // tên vai trò
        //public string AnhDaiDien { get; set; }    // đường dẫn ảnh đại diện
        //public int MSSV { get; set; }     // mã số sinh viên
        //public string HoTen { get; set; }
        //public string Email { get; set; }
        //public DateTime? NgaySinh { get; set; }
        //public string GioiTinh { get; set; }
        //public string DiaChi { get; set; }
        //public string SoDienThoai { get; set; }
        //public string VaiTro { get; set; }
        //public int? AnhDaiDien { get; set; }
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