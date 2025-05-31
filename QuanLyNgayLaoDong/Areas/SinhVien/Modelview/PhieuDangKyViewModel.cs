using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuanLyNgayLaoDong.Areas.SinhVien.Modelview
{
    public class PhieuDangKyViewModel
    {
        public int Id { get; set; }
        public int? DotId { get; set; }

        [Required]
        [Display(Name = "Mã số sinh viên")]
        public int? MSSV { get; set; }

        [Display(Name = "Lao động theo lớp")]
        public bool? LaoDongTheoLop { get; set; }

        [Display(Name = "Lao động cá nhân")]
        public bool? LaoDongCaNhan { get; set; }

        [Display(Name = "Thời gian đăng ký")]
        [DataType(DataType.DateTime)]
        public DateTime? ThoiGian { get; set; }

        // ==== Các trường chỉ để hiển thị, không lưu DB ====
        public string TenDotLaoDong { get; set; }
        public string DotLaoDong { get; set; }
        public DateTime? NgayLaoDong { get; set; }
        public string KhuVuc { get; set; }
        public string Buoi { get; set; }
        public string LoaiLaoDong { get; set; }
        public string MoTa { get; set; }
        public string GioCuThe { get; set; } // nếu bạn cần tách riêng thời gian
        public int? GiaTri { get; set; } // số ngày được cộng

        //public int Id { get; set; }
        //public int? MSSV { get; set; }
        //public int? DotId { get; set; }
        //[Display(Name = "Lao động theo lớp")]
        //public bool? LaoDongTheoLop { get; set; }
        //[Display(Name = "Lao động cá nhân")]
        //public bool? LaoDongCaNhan { get; set; }

        //public DateTime? ThoiGian { get; set; }

        //// Thông tin đợt lao động
        //public string TenDotLaoDong { get; set; }
        //public DateTime? NgayLaoDong { get; set; }
        //public string KhuVuc { get; set; }
        //public string Buoi { get; set; }
        //public string MoTa { get; set; }
        //public string GioCuThe { get; set; }

        //public string LoaiLaoDong { get; set; }
    }
}