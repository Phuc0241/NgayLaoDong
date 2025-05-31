using QuanLyNgayLaoDong.Areas.LopTruong.ViewModel;
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

namespace QuanLyNgayLaoDong.Areas.LopTruong.Controllers
{
    [Authorize(Roles = "LopTruong")]
    public class TrangChuLopTruongController : Controller
    {
        private DB_QLNLD _contextdb = new DB_QLNLD();
        // GET: LopTruong/TrangChu
        public ActionResult Index()
        {
            var username = User.Identity.Name;

            var model = (from tk in _contextdb.TaiKhoans
                         join vt in _contextdb.VaiTroes on tk.role_id equals vt.vaitro_id
                         join sv in _contextdb.SinhViens on tk.taikhoan_id equals sv.taikhoan
                         join anh in _contextdb.Anhs on sv.anh_id equals anh.anh_id into anhJoin
                         from anhLeft in anhJoin.DefaultIfEmpty()
                         where tk.username == username
                         select new TaiKhoanLopTruongViewModel
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
       public ActionResult TrangThongTinLopTruong()
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
                         select new TaiKhoanLopTruongViewModel
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
        public ActionResult ThongTinLopTruong()
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

                return new PhieuDangKyLopTruongViewModel
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


            return View("~/Areas/LopTruong/Views/TrangChuLopTruong/PhieuDangKy.cshtml", result);
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

            var model = new PhieuDangKyLopTruongViewModel
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

            return View("~/Areas/LopTruong/Views/TrangChuLopTruong/CreatePhieuDangKy.cshtml", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePhieuDangKy(PhieuDangKyLopTruongViewModel model)
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
                    DotId = model.DotId, // ✅ BỔ SUNG DÒNG NÀY
                    LaoDongTheoLop = laoDongLop,
                    LaoDongCaNhan = laoDongCaNhan,
                    ThoiGian = model.ThoiGian,
                };

                _contextdb.PhieuDangKies.Add(entity);
                _contextdb.SaveChanges();
                return RedirectToAction("Index", "TrangChuLopTruong", new { area = "LopTruong" });
            }

            return View("~/Areas/LopTruong/Views/TrangChuLopTruong/CreatePhieuDangKy.cshtml", model);
        }

        
        [HttpGet]
        public ActionResult CreatePhieuDangKyTheoLop(int dotId)
        {
            string username = User.Identity.Name;

            // Lấy MSSV từ tài khoản
            var mssv = (from tk in _contextdb.TaiKhoans
                        join sv in _contextdb.SinhViens on tk.taikhoan_id equals sv.taikhoan
                        where tk.username == username
                        select sv.MSSV).FirstOrDefault();

            // Lấy lớp của lớp trưởng
            var lopId = _contextdb.SinhViens.Where(sv => sv.MSSV == mssv).Select(sv => sv.lop_id).FirstOrDefault();

            // Lấy danh sách sinh viên trong lớp
            var sinhViens = _contextdb.SinhViens
                .Where(sv => sv.lop_id == lopId)
                .Select(sv => new SinhVienItem
                {
                    MSSV = sv.MSSV,
                    HoTen = sv.hoten,
                    DuocChon = true // mặc định tích chọn
                }).ToList();

            // 🟢 Tính thời gian mặc định dựa trên dotId
            var dot = _contextdb.TaoDotNgayLaoDongs.FirstOrDefault(d => d.ID == dotId);
            DateTime? thoiGianMacDinh = null;

            if (dot != null && dot.NgayLaoDong.HasValue && !string.IsNullOrWhiteSpace(dot.ThoiGian))
            {
                string gioRaw = dot.ThoiGian.Trim();
                TimeSpan gioBatDau;

                var matchKhoangGio = Regex.Match(gioRaw, @"^(\d{1,2})(h|h\d{1,2})?");
                if (matchKhoangGio.Success)
                {
                    string gioPhut = matchKhoangGio.Value.Replace("h", ":");

                    if (Regex.IsMatch(gioPhut, @"^\d{1,2}$"))
                        gioPhut += ":00";
                    else if (Regex.IsMatch(gioPhut, @"^\d{1,2}:$"))
                        gioPhut += "00";

                    if (TimeSpan.TryParse(gioPhut, out gioBatDau))
                    {
                        thoiGianMacDinh = dot.NgayLaoDong.Value.Date.Add(gioBatDau);
                    }
                    else
                    {
                        thoiGianMacDinh = dot.NgayLaoDong.Value.Date.Add(new TimeSpan(7, 0, 0));
                    }
                }
                else
                {
                    thoiGianMacDinh = dot.NgayLaoDong.Value.Date.Add(new TimeSpan(7, 0, 0));
                }
            }
            else if (dot?.NgayLaoDong != null)
            {
                thoiGianMacDinh = dot.NgayLaoDong.Value.Date.Add(new TimeSpan(7, 0, 0));
            }
            else
            {
                // fallback nếu không có dot
                thoiGianMacDinh = DateTime.Now;
            }

            var viewModel = new PhieuDangKyTheoLopViewModel
            {
                DotId = dotId,
                ThoiGian = thoiGianMacDinh,
                LaoDongTheoLop = true,
                SinhViensTrongLop = sinhViens
            };

            return View("~/Areas/LopTruong/Views/TrangChuLopTruong/CreatePhieuDangKyTheoLop.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePhieuDangKyTheoLop(PhieuDangKyTheoLopViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sinhVienChon = model.SinhViensTrongLop.Where(sv => sv.DuocChon).ToList();

                int maxId = _contextdb.PhieuDangKies.Any() ? _contextdb.PhieuDangKies.Max(p => p.id) + 1 : 1;

                foreach (var sv in sinhVienChon)
                {
                    var phieu = new PhieuDangKy
                    {
                        id = maxId++,
                        MSSV = sv.MSSV,
                        DotId = model.DotId,
                        LaoDongTheoLop = true,
                        LaoDongCaNhan = false,
                        ThoiGian = model.ThoiGian
                    };

                    _contextdb.PhieuDangKies.Add(phieu);
                }

                _contextdb.SaveChanges();
                return RedirectToAction("PhieuDangKy", "TrangChuLopTruong");
            }

            return View("~/Areas/LopTruong/Views/TrangChuLopTruong/CreatePhieuDangKyTheoLop.cshtml", model);
        }
     
        //thứ 6
        [HttpGet]
        public ActionResult EditPhieuDangKy(int id)
        {
            var phieu = _contextdb.PhieuDangKies.Find(id);
            if (phieu == null) return HttpNotFound();

            var dot = _contextdb.TaoDotNgayLaoDongs.FirstOrDefault(d => d.ID == phieu.DotId);
            var lopId = _contextdb.SinhViens.Where(sv => sv.MSSV == phieu.MSSV).Select(sv => sv.lop_id).FirstOrDefault();

            var sinhViens = _contextdb.SinhViens
                .Where(sv => sv.lop_id == lopId)
                .Select(sv => new SinhVienItem
                {
                    MSSV = sv.MSSV,
                    HoTen = sv.hoten,
                    DuocChon = _contextdb.PhieuDangKies.Any(p => p.DotId == phieu.DotId && p.MSSV == sv.MSSV && p.LaoDongTheoLop == true)
                }).ToList();

            var viewModel = new EditPhieuDangKyFullViewModel
            {
                ThongTinPhieu = new PhieuDangKyLopTruongViewModel
                {
                    Id = phieu.id,
                    DotId = (int)phieu.DotId,
                    ThoiGian = phieu.ThoiGian,
                    DotLaoDong = dot?.DotLaoDong,
                    NgayLaoDong = dot?.NgayLaoDong,
                    KhuVuc = dot?.KhuVuc,
                    Buoi = dot?.Buoi,
                    MoTa = dot?.MoTa,
                    GiaTri = dot?.GiaTri
                },
                ChinhSuaSinhVien = new PhieuDangKyTheoLopViewModel
                {
                    Id = phieu.id,
                    DotId = (int)phieu.DotId,
                    ThoiGian = phieu.ThoiGian,
                    SinhViensTrongLop = sinhViens
                }
            };

            return View("EditPhieuDangKy", viewModel);
        }
        //thứ 6
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPhieuDangKy(EditPhieuDangKyFullViewModel model)
        {
            var dotId = model.ChinhSuaSinhVien.DotId;
            var sinhVienChon = model.ChinhSuaSinhVien.SinhViensTrongLop
                                .Where(sv => sv.DuocChon)
                                .Select(sv => sv.MSSV)
                                .ToList();

            var phieuHienTai = _contextdb.PhieuDangKies
                .Where(p => p.DotId == dotId)
                .ToList();

            var mssvHienTai = phieuHienTai
                .Where(p => p.LaoDongTheoLop == true)
                .Select(p => p.MSSV.Value)
                .ToList();

            var sinhVienThem = sinhVienChon.Except(mssvHienTai).ToList();
            var sinhVienXoa = mssvHienTai.Except(sinhVienChon).ToList();

            // ✅ Cập nhật những MSSV mới được chọn
            foreach (var mssv in sinhVienThem)
            {
                var phieu = phieuHienTai.FirstOrDefault(p => p.MSSV == mssv && p.DotId == dotId);
                if (phieu != null)
                {
                    phieu.LaoDongTheoLop = true;
                }
                else
                {
                    // Nếu chưa có → tạo mới với id tự tăng
                    int nextId = _contextdb.PhieuDangKies.Any()
                        ? _contextdb.PhieuDangKies.Max(p => p.id) + 1
                        : 1;

                    _contextdb.PhieuDangKies.Add(new PhieuDangKy
                    {
                        id = nextId,
                        MSSV = mssv,
                        DotId = dotId,
                        LaoDongTheoLop = true,
                        LaoDongCaNhan = false,
                        ThoiGian = model.ChinhSuaSinhVien.ThoiGian
                    });
                }
            }

            // ✅ Xóa tick (bỏ chọn) → cập nhật LaoDongTheoLop = false
            foreach (var mssv in sinhVienXoa)
            {
                var phieu = phieuHienTai
                    .FirstOrDefault(p => p.DotId == dotId && p.MSSV == mssv && p.LaoDongTheoLop == true);

                if (phieu != null)
                {
                    phieu.LaoDongTheoLop = false;
                }
            }

            _contextdb.SaveChanges();
            return RedirectToAction("PhieuDangKy", "TrangChuLopTruong");
        }

        [HttpGet]
        public ActionResult DeletePhieuDangKy(int id)
        {
            var entity = _contextdb.PhieuDangKies.Find(id);
            if (entity == null) return HttpNotFound();

            _contextdb.PhieuDangKies.Remove(entity);
            _contextdb.SaveChanges();
            //return RedirectToAction("PhieuDangKy", "TrangChuLopTruong", new { area = "LopTruong" });
            return View("~/Areas/LopTruong/Views/TrangChuLopTruong/PhieuDangKy.cshtml");
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
                .Select(p => new PhieuDangKyLopTruongViewModel
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

            return View("~/Areas/LopTruong/Views/TrangChuLopTruong/LichLaoDong.cshtml", lichDangKy);
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
                return new PhieuDangKyLopTruongViewModel
                {
                    Id = p.id,
                    MSSV = p.MSSV,
                    LaoDongTheoLop = p.LaoDongTheoLop,
                    LaoDongCaNhan = p.LaoDongCaNhan,
                    ThoiGian = p.ThoiGian,
                    NgayLaoDong = dot?.NgayLaoDong, // dùng để kiểm tra ngày
                };
            }).ToList();

            return View("~/Areas/LopTruong/Views/TrangChuLopTruong/DiemDanh.cshtml", viewModel);
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
                .Select(d => new DotLaoDongLopTruongViewModel
                {
                    ID = d.ID,
                    TenDotLaoDong = d.TenDotLaoDong,
                    KhuVuc = d.KhuVuc,
                    LoaiLaoDong = d.LoaiLaoDong,
                    NgayLaoDong = d.NgayLaoDong ?? DateTime.MinValue,
                    Buoi = d.Buoi,
                    GiaTri = d.GiaTri ?? 0,
                    ThoiGian = d.ThoiGian,
                    MoTa = d.MoTa
                })
                .ToList();

            return View("~/Areas/LopTruong/Views/TrangChuLopTruong/ThongBao.cshtml", dsDotLaoDong);
        }
        public ActionResult DangKyTheoThongBao()
        {
       
                var dsDotLaoDong = _contextdb.TaoDotNgayLaoDongs
                .Select(d => new DotLaoDongLopTruongViewModel
                {
                    ID = d.ID,
                    DotLaoDong = d.DotLaoDong,
                    KhuVuc = d.KhuVuc,
                    LoaiLaoDong = d.LoaiLaoDong, // ✅ Đảm bảo có trường này, ví dụ: "Lop" hoặc "CaNhan"
                    NgayLaoDong = d.NgayLaoDong ?? DateTime.MinValue,
                    Buoi = d.Buoi,
                    GiaTri = d.GiaTri ?? 0,
                    ThoiGian = d.ThoiGian,
                    MoTa = d.MoTa
                })
                .ToList();


            return View("~/Areas/LopTruong/Views/TrangChuLopTruong/DangKyTheoThongBao.cshtml", dsDotLaoDong);
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
                          select new ThongKeLaoDongLopTruongViewModel
                          {
                              MSSV = sv.MSSV,
                              HoTen = sv.hoten,
                              TongSoNgay = soNgay != null ? soNgay.TongSoNgay : 0
                          }).FirstOrDefault();

            return View("~/Areas/LopTruong/Views/TrangChuLopTruong/ThongKe.cshtml", svInfo); // không cần List<>

        }
    }
}