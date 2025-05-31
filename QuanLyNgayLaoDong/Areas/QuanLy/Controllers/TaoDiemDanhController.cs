using QuanLyNgayLaoDong.Areas.QuanLy.ViewModel;
using QuanLyNgayLaoDong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyNgayLaoDong.Areas.QuanLy.Controllers
{
    [Authorize]
    public class TaoDiemDanhController : Controller
    {
        private DB_QLNLD _contextdb = new DB_QLNLD();
        // GET: QuanLy/TaoDiemDanh
        public ActionResult Index()
        {
            return View();
        }
        // GET: Tạo mã điểm danh mới
        public ActionResult TaoMa()
        {
            var random = new Random();
            string ma = random.Next(100000, 999999).ToString();

            DiemDanhTempStore.TaoMaDiemDanh(ma);
            ViewBag.MaMoi = ma;

            return View();
        }

        // GET: Danh sách sinh viên đã điểm danh theo mã
        public ActionResult DanhSach(string ma)
        {
            if (!DiemDanhTempStore.KiemTraMa(ma))
            {
                TempData["Error"] = "Mã điểm danh không hợp lệ hoặc đã hết hạn.";
                return RedirectToAction("TaoMa");
            }

            var mssvs = DiemDanhTempStore.LayDanhSachMSSV(ma);

            var danhSach = _contextdb.SinhViens.Where(sv => mssvs.Contains(sv.MSSV)).ToList();
            return View( danhSach); // Trả về view danh sách sinh viên đã điểm danh
        }
    }
}