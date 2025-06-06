﻿using QuanLyNgayLaoDong.Areas.LopTruong.ViewModel;
using QuanLyNgayLaoDong.Areas.QuanLy.ViewModel;
using QuanLyNgayLaoDong.Areas.SinhVien.Modelview;
using QuanLyNgayLaoDong.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace QuanLyNgayLaoDong.Areas.SinhVien.Controllers
{
    [Authorize(Roles = "SinhVien")]
    public class TrangChuController : Controller
    {
        private DB_QLNLD _contextdb = new DB_QLNLD();
        // GET: SinhVien/TrangChu
        public ActionResult Index()
        {
            var username = User.Identity.Name;

            var model = (from tk in _contextdb.TaiKhoans
                         join vt in _contextdb.VaiTroes on tk.role_id equals vt.vaitro_id
                         join sv in _contextdb.SinhViens on tk.taikhoan_id equals sv.taikhoan
                         join anh in _contextdb.Anhs on sv.anh_id equals anh.anh_id into anhJoin
                         from anhLeft in anhJoin.DefaultIfEmpty()
                         where tk.username == username
                         select new TaiKhoanViewModel
                         {
                             MSSV = sv.MSSV,
                             HoTen = sv.hoten,
                             Email = tk.email,
                             NgaySinh = sv.ngaysinh,
                             GioiTinh = sv.gioitinh.HasValue ? (sv.gioitinh.Value ? "Nam" : "Nữ") : "Chưa cập nhật",
                             DiaChi = sv.quequan,
                             SoDienThoai = sv.SDT,
                             VaiTro = vt.vaitro1,
                             AnhDaiDien = anhLeft != null
                                        ? anhLeft.duongdan
                                        : (sv.gioitinh.HasValue
                                            ? (sv.gioitinh.Value ? "~/Uploads/avatar/man.png"
                                                            : "~/Uploads/avatar/woman.png")
                                            : "~/Uploads/avatar/default.png")
                         }).FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
            //return View();
        }
        public ActionResult TrangThongTinSinhVien()
        {
            var username = User.Identity.Name;

            var taiKhoan = _contextdb.TaiKhoans
                .Include("VaiTro")
                .FirstOrDefault(t => t.username == username);

            if (taiKhoan == null)
                return HttpNotFound();

            var model = (from tk in _contextdb.TaiKhoans
                         join vt in _contextdb.VaiTroes on tk.role_id equals vt.vaitro_id
                         join sv in _contextdb.SinhViens on tk.taikhoan_id equals sv.taikhoan
                         join anh in _contextdb.Anhs on sv.anh_id equals anh.anh_id into anhJoin
                         from anhLeft in anhJoin.DefaultIfEmpty()
                         where tk.username == username
                         select new TaiKhoanViewModel
                         {
                             MSSV = sv.MSSV,
                             HoTen = sv.hoten,
                             Email = tk.email,
                             NgaySinh = sv.ngaysinh,
                             GioiTinh = sv.gioitinh.HasValue ? (sv.gioitinh.Value ? "Nam" : "Nữ") : "Chưa cập nhật",
                             DiaChi = sv.quequan,
                             SoDienThoai = sv.SDT,
                             VaiTro = vt.vaitro1,
                             AnhDaiDien = anhLeft != null
                                        ? anhLeft.duongdan
                                        : (sv.gioitinh.HasValue
                                            ? (sv.gioitinh.Value ? "~/Uploads/avatar/man.png"
                                                            : "~/Uploads/avatar/woman.png")
                                                        : "~/Uploads/avatar/default.png")


                         }).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }
        public ActionResult ThongTinSinhVien()
        {
            // Giả sử bạn có DbContext
            var user = _contextdb.TaiKhoans.FirstOrDefault(t => t.username == "tên đăng nhập");

            if (user == null)
            {
                return HttpNotFound(); // Xử lý nếu không tìm thấy tài khoản
            }

            return View(user);
        }
        //Không xóa được bị trùng lập, hoặc lớp trưởng bỏ tích chọn
        [HttpGet]
        public ActionResult PhieuDangKy()
        {
            if (Session["MSSV"] == null)
                return RedirectToAction("Login", "Login"); // Nếu chưa đăng nhập

            int mssv = Convert.ToInt32(Session["MSSV"]);

            var danhSachPhieu = _contextdb.PhieuDangKies
            .Where(p => p.MSSV == mssv)
            .ToList();
            var danhSachDot = _contextdb.TaoDotNgayLaoDongs.ToList();
            var result = danhSachPhieu.Select((p, index) =>
            {
                var dot = danhSachDot.FirstOrDefault(d => d.ID == p.DotId); // ✅ sửa lại

                return new PhieuDangKyViewModel
                {
                    Id = p.id,
                    MSSV = p.MSSV,
                    DotId = p.id,
                    LaoDongCaNhan = p.LaoDongCaNhan,
                    LaoDongTheoLop = p.LaoDongTheoLop,
                    ThoiGian = p.ThoiGian,
                    DotLaoDong = dot?.DotLaoDong,
                    TenDotLaoDong = dot?.TenDotLaoDong,
                    NgayLaoDong = dot?.NgayLaoDong,
                    Buoi = dot?.Buoi,
                    KhuVuc = dot?.KhuVuc,
                    GioCuThe = dot?.ThoiGian,
                    MoTa = dot?.MoTa,
                    GiaTri = dot?.GiaTri,
                    LoaiLaoDong = p.LaoDongTheoLop == true ? "Theo lớp" :
                                  p.LaoDongCaNhan == true ? "Cá nhân" : "Chưa chọn"
                };
            }).ToList();


            return View("~/Areas/SinhVien/Views/TrangChu/PhieuDangKy.cshtml", result);
        }

        [HttpGet]
        public ActionResult CreatePhieuDangKy(int id)
        {
            var phieu = _contextdb.PhieuDangKies.FirstOrDefault(p => p.id == id);
            var dot = _contextdb.TaoDotNgayLaoDongs.FirstOrDefault(d => d.ID == id);
            if (dot == null) return HttpNotFound();

            // ✅ Lấy MSSV từ username đang đăng nhập
            string username = User.Identity.Name;

            var mssv = (from tk in _contextdb.TaiKhoans
                        join sv in _contextdb.SinhViens on tk.taikhoan_id equals sv.taikhoan
                        where tk.username == username
                        select sv.MSSV).FirstOrDefault();

       
            DateTime? thoiGianMacDinh = null;

            if (dot.NgayLaoDong.HasValue && !string.IsNullOrWhiteSpace(dot.ThoiGian))
            {
                string gioRaw = dot.ThoiGian.Trim();
                TimeSpan gioBatDau;

                // Trường hợp "14h-15h30", lấy số đầu
                var matchKhoangGio = Regex.Match(gioRaw, @"^(\d{1,2})(h|h\d{1,2})?");

                if (matchKhoangGio.Success)
                {
                    string gioPhut = matchKhoangGio.Value.Replace("h", ":");

                    // Nếu chỉ có số giờ (ví dụ "14"), thêm ":00"
                    if (Regex.IsMatch(gioPhut, @"^\d{1,2}$"))
                        gioPhut += ":00";
                    else if (Regex.IsMatch(gioPhut, @"^\d{1,2}:$")) // ví dụ "14:"
                        gioPhut += "00";

                    if (TimeSpan.TryParse(gioPhut, out gioBatDau))
                    {
                        thoiGianMacDinh = dot.NgayLaoDong.Value.Date.Add(gioBatDau);
                    }
                    else
                    {
                        // fallback: mặc định 07:00
                        thoiGianMacDinh = dot.NgayLaoDong.Value.Date.Add(new TimeSpan(7, 0, 0));
                    }
                }
                else
                {
                    // fallback nếu không match regex: dùng 07:00
                    thoiGianMacDinh = dot.NgayLaoDong.Value.Date.Add(new TimeSpan(7, 0, 0));
                }
            }
            else
            {
                // fallback nếu ngày hoặc giờ rỗng
                thoiGianMacDinh = dot.NgayLaoDong.Value.Date.Add(new TimeSpan(7, 0, 0));
            }

            var model = new PhieuDangKyViewModel
            {
                MSSV = mssv,
                DotId = dot.ID,
                TenDotLaoDong = dot.TenDotLaoDong,
                NgayLaoDong = dot.NgayLaoDong,
                KhuVuc = dot.KhuVuc,
                Buoi = dot.Buoi,
                //LoaiLaoDong = dot.LoaiLaoDong,
                LoaiLaoDong = phieu.LaoDongTheoLop == true ? "Lop" :
                              phieu.LaoDongCaNhan == true ? "CaNhan" : null,
                MoTa = dot.MoTa,
                GioCuThe = dot.ThoiGian,
                ThoiGian = thoiGianMacDinh
            };

            return View("~/Areas/SinhVien/Views/TrangChu/CreatePhieuDangKy.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePhieuDangKy(PhieuDangKyViewModel model)
        {
            if (ModelState.IsValid)
            {
                string loaiLaoDong = Request.Form["LoaiLaoDong"]; // "Lop" hoặc "CaNhan"

                bool laoDongLop = loaiLaoDong == "Lop";
                bool laoDongCaNhan = loaiLaoDong == "CaNhan";

                int newId = _contextdb.PhieuDangKies.Any()
                    ? _contextdb.PhieuDangKies.Max(p => p.id) + 1
                    : 1;

                var entity = new PhieuDangKy
                {
                    id = newId,
                    MSSV = model.MSSV,
                    LaoDongTheoLop = laoDongLop,
                    LaoDongCaNhan = laoDongCaNhan,
                    ThoiGian = model.ThoiGian,
                    DotId = model.DotId
                };

                _contextdb.PhieuDangKies.Add(entity);
                _contextdb.SaveChanges();
                return RedirectToAction("Index", "TrangChu", new { area = "SinhVien" });
            }

            return View("~/Areas/SinhVien/Views/TrangChu/CreatePhieuDangKy.cshtml", model);
        }


        [HttpGet]
        public ActionResult EditPhieuDangKy(int id)
        {
            var entity = _contextdb.PhieuDangKies.Find(id);
            if (entity == null) return HttpNotFound();

            var model = new PhieuDangKyViewModel
            {
                Id = entity.id,
                MSSV = entity.MSSV,
                LaoDongCaNhan = entity.LaoDongCaNhan,
                LaoDongTheoLop = entity.LaoDongTheoLop,
                ThoiGian = entity.ThoiGian
            };
            return View("~/Areas/SinhVien/Views/TrangChu/EditPhieuDangKy.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPhieuDangKy(PhieuDangKyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _contextdb.PhieuDangKies.Find(model.Id);
                if (entity == null) return HttpNotFound();

                string loaiLaoDong = Request.Form["LoaiLaoDong"];
                entity.MSSV = model.MSSV;
                entity.LaoDongTheoLop = (loaiLaoDong == "Lop");
                entity.LaoDongCaNhan = (loaiLaoDong == "CaNhan");
                entity.ThoiGian = model.ThoiGian;

                _contextdb.SaveChanges();
                return RedirectToAction("Index", "TrangChu", new { area = "SinhVien" });
            }
            return View("~/Areas/SinhVien/Views/TrangChu/EditPhieuDangKy.cshtml", model);
        }

        [HttpGet]
        public ActionResult DeletePhieuDangKy(int id)
        {
            var entity = _contextdb.PhieuDangKies.Find(id);
            if (entity == null) return HttpNotFound();

            _contextdb.PhieuDangKies.Remove(entity);
            _contextdb.SaveChanges();
            return RedirectToAction("PhieuDangKy", "TrangChu", new { area = "SinhVien" });
        }
     
        [HttpGet]
        public ActionResult LichLaoDong(DateTime? selectedDate)
        {
            var username = User.Identity.Name;

            // Lấy MSSV từ tài khoản đăng nhập
            var mssv = (from tk in _contextdb.TaiKhoans
                        join sv in _contextdb.SinhViens on tk.taikhoan_id equals sv.taikhoan
                        where tk.username == username
                        select sv.MSSV).FirstOrDefault();

            // Nếu không truyền ngày, mặc định là hôm nay
            DateTime startOfWeek = selectedDate ?? DateTime.Today;

            // Đưa về thứ Hai đầu tuần
            while (startOfWeek.DayOfWeek != DayOfWeek.Monday)
                startOfWeek = startOfWeek.AddDays(-1);

            // Ngày cuối tuần (Chủ nhật)
            DateTime endOfWeek = startOfWeek.AddDays(6);

            // Truy vấn các phiếu đăng ký trong tuần đó
            var lichDangKy = _contextdb.PhieuDangKies
                .Where(p => p.MSSV == mssv &&
                            DbFunctions.TruncateTime(p.ThoiGian) >= startOfWeek.Date &&
                            DbFunctions.TruncateTime(p.ThoiGian) <= endOfWeek.Date)
                .Select(p => new PhieuDangKyViewModel
                {
                    Id = p.id,
                    MSSV = p.MSSV,
                    ThoiGian = p.ThoiGian,
                    LaoDongTheoLop = p.LaoDongTheoLop,
                    LaoDongCaNhan = p.LaoDongCaNhan
                })
                .ToList();

            ViewBag.StartOfWeek = startOfWeek;
            ViewBag.EndOfWeek = endOfWeek;

            return View("~/Areas/SinhVien/Views/TrangChu/LichLaoDong.cshtml", lichDangKy);
        }

        public ActionResult DoiMatKhau()
        {
            return View();
        }
      
        [HttpGet]
        public ActionResult DiemDanh()
        {
            var username = User.Identity.Name;

            var mssv = (from tk in _contextdb.TaiKhoans
                        join sv in _contextdb.SinhViens on tk.taikhoan_id equals sv.taikhoan
                        where tk.username == username
                        select sv.MSSV).FirstOrDefault();

            var danhSachPhieu = _contextdb.PhieuDangKies
                .Where(p => p.MSSV == mssv)
                .ToList();

            var danhSachDot = _contextdb.TaoDotNgayLaoDongs.ToList();

            var viewModel = danhSachPhieu.Select(p =>
            {
                var dot = danhSachDot.FirstOrDefault(d => d.ID == p.DotId);
                return new PhieuDangKyViewModel
                {
                    Id = p.id,
                    MSSV = p.MSSV,
                    LaoDongTheoLop = p.LaoDongTheoLop,
                    LaoDongCaNhan = p.LaoDongCaNhan,
                    ThoiGian = p.ThoiGian,
                    NgayLaoDong = dot?.NgayLaoDong, // dùng để kiểm tra ngày
                };
            }).ToList();

            return View("~/Areas/SinhVien/Views/TrangChu/DiemDanh.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult NhapMa(string maDiemDanh, int? id) // Cho phép id null
        {
            if (string.IsNullOrWhiteSpace(maDiemDanh))
            {
                TempData["DiemDanhStatus"] = "empty";
                TempData["DiemDanhId"] = id;
                return RedirectToAction("DiemDanh");
            }

            if (!DiemDanhTempStore.KiemTraMa(maDiemDanh))
            {
                TempData["DiemDanhStatus"] = "wrong";
                TempData["DiemDanhId"] = id;
                return RedirectToAction("DiemDanh");
            }

            var username = User.Identity.Name;
            var mssv = (from tk in _contextdb.TaiKhoans
                        join sv in _contextdb.SinhViens on tk.taikhoan_id equals sv.taikhoan
                        where tk.username == username
                        select sv.MSSV).FirstOrDefault();

            DiemDanhTempStore.DiemDanh(maDiemDanh, mssv);

            TempData["DiemDanhStatus"] = "success";
            TempData["DiemDanhId"] = id;
            return RedirectToAction("DiemDanh");
        }


        public ActionResult ThongBao()
        {
            var dsDotLaoDong = _contextdb.TaoDotNgayLaoDongs
                .Select(d => new DotLaoDongViewModel
                {
                    ID = d.ID,
                    DotLaoDong = d.DotLaoDong,
                    KhuVuc = d.KhuVuc,
                    LoaiLaoDong = d.LoaiLaoDong,
                    NgayLaoDong = d.NgayLaoDong ?? DateTime.MinValue,
                    Buoi = d.Buoi,
                    GiaTri = d.GiaTri ?? 0,
                    ThoiGian = d.ThoiGian,
                    MoTa = d.MoTa
                })
                .ToList();

            return View("~/Areas/SinhVien/Views/TrangChu/ThongBao.cshtml", dsDotLaoDong);
        }
        public ActionResult DangKyTheoThongBao()
        {
            var userRole = (string)Session["Role"]; // hoặc dùng User.IsInRole nếu có sẵn

            var query = _contextdb.TaoDotNgayLaoDongs.AsQueryable();

            if (userRole == "SinhVien")
            {
                query = query.Where(d => d.LoaiLaoDong == "Cá nhân");
            }
            var dsDotLaoDong = _contextdb.TaoDotNgayLaoDongs
                .Where(d => d.LoaiLaoDong == "Cá nhân") // chỉ lấy đợt lao động cá nhân
                .Select(d => new DotLaoDongViewModel
                {
                    ID = d.ID,
                    DotLaoDong = d.DotLaoDong,
                    KhuVuc = d.KhuVuc,
                    LoaiLaoDong = d.LoaiLaoDong,
                    NgayLaoDong = d.NgayLaoDong ?? DateTime.MinValue,
                    Buoi = d.Buoi,
                    GiaTri = d.GiaTri ?? 0,
                    ThoiGian = d.ThoiGian,
                    MoTa = d.MoTa
                })
                .ToList();

            return View("~/Areas/SinhVien/Views/TrangChu/DangKyTheoThongBao.cshtml", dsDotLaoDong);
        }
        public ActionResult ThongKe()
        {
            string username = User.Identity.Name;

            var mssv = (from tk in _contextdb.TaiKhoans
                        join sv in _contextdb.SinhViens on tk.taikhoan_id equals sv.taikhoan
                        where tk.username == username
                        select sv.MSSV).FirstOrDefault();

            var svInfo = (from sv in _contextdb.SinhViens
                          where sv.MSSV == mssv
                          join songay in _contextdb.SoNgayLaoDongs on sv.MSSV equals songay.MSSV into soNgayJoin
                          from soNgay in soNgayJoin.DefaultIfEmpty()
                          select new ThongKeLaoDongViewModel
                          {
                              MSSV = sv.MSSV,
                              HoTen = sv.hoten,
                              TongSoNgay = soNgay != null ? soNgay.TongSoNgay : 0
                          }).FirstOrDefault();

            return View("~/Areas/SinhVien/Views/TrangChu/ThongKe.cshtml", svInfo); // không cần List<>

        }
        public ActionResult PhieuXacNhanHoanThanh()
        {
            return View();
        }
    }
}