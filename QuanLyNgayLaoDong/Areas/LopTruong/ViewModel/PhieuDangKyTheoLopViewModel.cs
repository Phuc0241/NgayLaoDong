using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyNgayLaoDong.Areas.LopTruong.ViewModel
{
    public class PhieuDangKyTheoLopViewModel
    {
        public int Id { get; set; }
        public int DotId { get; set; } // ID đợt lao động
        public DateTime? ThoiGian { get; set; }
        public string MSSV { get; set; }
        public bool LaoDongTheoLop { get; set; }
        public bool LaoDongCaNhan { get; set; }

        // Danh sách sinh viên trong lớp (checkbox)
        public List<SinhVienItem> SinhViensTrongLop { get; set; } = new List<SinhVienItem>();
    }
    public class SinhVienItem
    {
        public int MSSV { get; set; }
        public string HoTen { get; set; }
        public bool DuocChon { get; set; }
    }
}