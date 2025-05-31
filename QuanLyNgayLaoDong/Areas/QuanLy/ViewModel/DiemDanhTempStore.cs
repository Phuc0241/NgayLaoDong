using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyNgayLaoDong.Areas.QuanLy.ViewModel
{
    public static class DiemDanhTempStore
    {
        // Mã điểm danh: string (6 số) => Danh sách MSSV đã điểm danh
        public static Dictionary<string, List<int>> MaDiemDanhDict = new Dictionary<string, List<int>>();

        // Mã điểm danh => Thời gian tạo (để có thể kiểm tra hạn dùng)
        public static Dictionary<string, DateTime> MaDiemDanhCreatedAt = new Dictionary<string, DateTime>();

        public static void TaoMaDiemDanh(string ma)
        {
            if (!MaDiemDanhDict.ContainsKey(ma))
            {
                MaDiemDanhDict[ma] = new List<int>();
                MaDiemDanhCreatedAt[ma] = DateTime.Now;
            }
        }

        public static bool KiemTraMa(string ma)
        {
            return MaDiemDanhDict.ContainsKey(ma) && (DateTime.Now - MaDiemDanhCreatedAt[ma]).TotalMinutes <= 30;
        }

        public static void DiemDanh(string ma, int mssv)
        {
            if (KiemTraMa(ma) && !MaDiemDanhDict[ma].Contains(mssv))
                MaDiemDanhDict[ma].Add(mssv);
        }

        public static List<int> LayDanhSachMSSV(string ma)
        {
            return MaDiemDanhDict.ContainsKey(ma) ? MaDiemDanhDict[ma] : new List<int>();
        }
    }
}