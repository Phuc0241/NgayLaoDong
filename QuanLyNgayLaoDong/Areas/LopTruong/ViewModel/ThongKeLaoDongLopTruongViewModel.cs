using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyNgayLaoDong.Areas.LopTruong.ViewModel
{
    public class ThongKeLaoDongLopTruongViewModel
    {
        public int MSSV { get; set; }
        public string HoTen { get; set; }
        public int? TongSoNgay { get; set; }
        public bool Du18Ngay => TongSoNgay >= 18;
    }
}