using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyNgayLaoDong.Areas.SinhVien.Modelview
{
    public class DotLaoDongViewModel
    {
        //public int ID { get; set; }
        //public string TenDotLaoDong { get; set; }
        //public string MoTa { get; set; }
        //public DateTime NgayBatDau { get; set; }
        //public DateTime NgayKetThuc { get; set; }
        public int ID { get; set; }

        public string TenDotLaoDong { get; set; }
        public string DotLaoDong { get; set; }
        public string KhuVuc { get; set; }

        public string LoaiLaoDong { get; set; }

        public DateTime NgayLaoDong { get; set; }

        public string Buoi { get; set; } // Sáng / Chiều

        public int GiaTri { get; set; } // Ngày được cộng

        public string ThoiGian { get; set; }

        public string MoTa { get; set; }
    }
}