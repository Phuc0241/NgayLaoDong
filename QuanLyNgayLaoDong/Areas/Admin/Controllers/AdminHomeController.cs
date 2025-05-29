using QuanLyNgayLaoDong.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;



namespace QuanLyNgayLaoDong.Areas.Admin.Controllers
{
    [Authorize]
    public class AdminHomeController : Controller
    {
        // GET: Admin/AdminHome
        public ActionResult Index()
        {
            return View();
        }

        /* Khu vực dành cho trang Tài Khoản */
        public ActionResult TrangTaiKhoan()
        {
            using (var db = new DB_QLNLD())
            {
                var list = db.TaiKhoans
                             .Where(t => t.deleted_at == null)
                             .Include(t => t.VaiTro)
                             .ToList();
                ViewBag.TaiKhoanEdit = TempData["TaiKhoanEdit"];
                ViewBag.TaiKhoanDetail = TempData["TaiKhoanDetail"];
                return View(list);
            }
        }

        // Thêm tài khoản 
        [HttpPost]
        public ActionResult AddTaiKhoan(TaiKhoan tk)
        {
            using (var db = new DB_QLNLD())
            {
                if (ModelState.IsValid)
                {
                    // Đặt mật khẩu mặc định nếu không có
                    if (string.IsNullOrEmpty(tk.password))
                    {
                        tk.password = "abc@123"; // Gợi ý mã hóa mật khẩu
                    }

                    db.TaiKhoans.Add(tk);
                    db.SaveChanges();

                    TempData["Message"] = "Thêm tài khoản thành công!";
                    return RedirectToAction("TrangTaiKhoan");
                }

                TempData["Error"] = "Dữ liệu không hợp lệ. Không thể thêm tài khoản.";
                return RedirectToAction("TrangTaiKhoan");
            }
        }

        // Xóa tài khoản (soft delete)
        public ActionResult Delete(int id)
        {
            using (var db = new DB_QLNLD())
            {
                var taiKhoan = db.TaiKhoans.Find(id);
                if (taiKhoan != null)
                {
                    taiKhoan.deleted_at = DateTime.Now;
                    db.SaveChanges();

                    TempData["Message"] = "Xóa tài khoản thành công!";
                }
                else
                {
                    TempData["Error"] = "Không tìm thấy tài khoản cần xóa.";
                }
            }
            return RedirectToAction("TrangTaiKhoan");
        }

        // Xem chi tiết tài khoản
        public ActionResult Details(int id)
        {
            using (var db = new DB_QLNLD())
            {
                var taiKhoan = db.TaiKhoans
                    .Include(t => t.VaiTro)
                    .FirstOrDefault(t => t.taikhoan_id == id && t.deleted_at == null);

                if (taiKhoan == null)
                {
                    TempData["Error"] = "Không tìm thấy tài khoản.";
                    return RedirectToAction("TrangTaiKhoan");
                }

                TempData["TaiKhoanDetail"] = taiKhoan;
                return RedirectToAction("TrangTaiKhoan");
            }
        }

        // Sửa tài khoản (GET)
        public ActionResult Edit(int id)
        {
            using (var db = new DB_QLNLD())
            {
                var taiKhoan = db.TaiKhoans
                    .Include(t => t.VaiTro)
                    .FirstOrDefault(t => t.taikhoan_id == id && t.deleted_at == null);

                if (taiKhoan == null)
                {
                    TempData["Error"] = "Không tìm thấy tài khoản để sửa.";
                    return RedirectToAction("TrangTaiKhoan");
                }

                TempData["TaiKhoanEdit"] = taiKhoan;
                return RedirectToAction("TrangTaiKhoan");
            }
        }

        // Sửa tài khoản (POST)
        [HttpPost]
        public ActionResult EditTaiKhoan(TaiKhoan tk)
        {
            if (ModelState.IsValid)
            {
                using (var db = new DB_QLNLD())
                {
                    var existingTaiKhoan = db.TaiKhoans.Find(tk.taikhoan_id);
                    if (existingTaiKhoan != null && existingTaiKhoan.deleted_at == null)
                    {
                        existingTaiKhoan.username = tk.username;

                        if (!string.IsNullOrEmpty(tk.password))
                        {
                            existingTaiKhoan.password = tk.password;
                        }

                        existingTaiKhoan.email = tk.email;
                        existingTaiKhoan.role_id = tk.role_id;

                        db.SaveChanges();

                        TempData["Message"] = "Cập nhật tài khoản thành công!";
                    }
                    else
                    {
                        TempData["Error"] = "Không tìm thấy tài khoản để cập nhật.";
                    }
                }
            }
            else
            {
                TempData["Error"] = "Dữ liệu không hợp lệ. Không thể cập nhật.";
            }

            return RedirectToAction("TrangTaiKhoan");
        }

        // Đặt lại mật khẩu
        [HttpPost]
        public ActionResult ResetPassword(int id)
        {
            using (var db = new DB_QLNLD())
            {
                var taiKhoan = db.TaiKhoans
                                 .FirstOrDefault(t => t.taikhoan_id == id && t.deleted_at == null);

                if (taiKhoan == null)
                {
                    TempData["Error"] = "Không tìm thấy tài khoản để đặt lại mật khẩu.";
                    return RedirectToAction("TrangTaiKhoan");
                }

                taiKhoan.password = "abc@123"; // Nên mã hóa mật khẩu
                db.SaveChanges();

                TempData["Message"] = "Đặt lại mật khẩu thành công!";
            }

            return RedirectToAction("TrangTaiKhoan");
        }









        // Hiển thị danh sách đợt lao động
        public ActionResult TrangTaoDotLaoDong()
        {
            using (var db = new DB_QLNLD())
            {
                // Lấy danh sách đợt lao động kèm thông tin tài khoản liên quan
                var list = db.TaoDotNgayLaoDongs
                             .Include(t => t.TaiKhoan)
                             .ToList();
                return View(list);
            }
        }





        //Hàm Tạo mới đợt lao động
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoMoiDotLaoDong(TaoDotNgayLaoDong model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra năm của NgayLaoDong
                if (model.NgayLaoDong.HasValue && model.NgayLaoDong.Value.Year < DateTime.Now.Year)
                {
                    ModelState.AddModelError("NgayLaoDong", $"Năm không được nhỏ hơn {DateTime.Now.Year}.");
                }
                else
                {
                    try
                    {
                        using (var db = new DB_QLNLD())
                        {
                            // Lấy thông tin người dùng đã đăng nhập
                            if (User.Identity.IsAuthenticated)
                            {
                                string username = User.Identity.Name;
                                var taiKhoan = db.TaiKhoans.FirstOrDefault(t => t.username == username);
                                if (taiKhoan != null)
                                {
                                    model.NguoiTao = taiKhoan.taikhoan_id;
                                }
                                else
                                {
                                    model.NguoiTao = null;
                                    System.Diagnostics.Debug.WriteLine($"Không tìm thấy tài khoản cho username: {username}");
                                }
                            }
                            else
                            {
                                model.NguoiTao = null;
                                System.Diagnostics.Debug.WriteLine("Người dùng chưa đăng nhập");
                            }

                            model.NgayTao = DateTime.Now;
                            model.NgayCapNhat = DateTime.Now;

                            db.TaoDotNgayLaoDongs.Add(model);
                            db.SaveChanges();
                            TempData["SuccessMessage"] = "Thêm đợt lao động thành công!";
                            return RedirectToAction("TrangTaoDotLaoDong");
                        }
                    }
                    catch (Exception ex)
                    {
                        var innerException = ex.InnerException?.Message ?? ex.Message;
                        System.Diagnostics.Debug.WriteLine($"Lỗi khi lưu dữ liệu: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"Inner Exception: {innerException}");
                        ModelState.AddModelError("", $"Lỗi khi lưu dữ liệu: {innerException}");
                    }
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            System.Diagnostics.Debug.WriteLine("Lỗi ModelState: " + string.Join(", ", errors));

            using (var db = new DB_QLNLD())
            {
                var list = db.TaoDotNgayLaoDongs
                             .Include(t => t.TaiKhoan)
                             .ToList();
                ViewBag.TaiKhoans = db.TaiKhoans.ToList();
                ViewBag.Errors = errors;
                return View("TrangTaoDotLaoDong", list);
            }
        }

        // Hàm sửa đọt lao động

        [HttpGet]
        public ActionResult ChinhSuaDotLaoDong(int id)
        {
            using (var db = new DB_QLNLD())
            {
                var dotLaoDong = db.TaoDotNgayLaoDongs.Find(id);
                if (dotLaoDong == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Không tìm thấy đợt lao động với ID: {id}");
                    return HttpNotFound();
                }

                // Log dữ liệu trả về để kiểm tra
                System.Diagnostics.Debug.WriteLine($"Dữ liệu trả về cho ID {id}: " +
                    $"DotLaoDong={dotLaoDong.DotLaoDong}, " +
                    $"NgayLaoDong={dotLaoDong.NgayLaoDong?.ToString("yyyy-MM-dd")}, " +
                    $"KhuVuc={dotLaoDong.KhuVuc}, " +
                    $"Buoi={dotLaoDong.Buoi}, " +
                    $"LoaiLaoDong={dotLaoDong.LoaiLaoDong}, " +
                    $"GiaTri={dotLaoDong.GiaTri}, " +
                    $"MoTa={dotLaoDong.MoTa}, " +
                    $"ThoiGian={dotLaoDong.ThoiGian}");

                // Trả về JSON cho AJAX
                return Json(new
                {
                    ID = dotLaoDong.ID,
                    DotLaoDong = dotLaoDong.DotLaoDong,
                    NgayLaoDong = dotLaoDong.NgayLaoDong?.ToString("yyyy-MM-dd"),
                    Buoi = dotLaoDong.Buoi,
                    LoaiLaoDong = dotLaoDong.LoaiLaoDong,
                    GiaTri = dotLaoDong.GiaTri,
                    ThoiGian = dotLaoDong.ThoiGian,
                    MoTa = dotLaoDong.MoTa,
                    KhuVuc = dotLaoDong.KhuVuc
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSuaDotLaoDong(TaoDotNgayLaoDong model)
        {
            if (ModelState.IsValid)
            {
                if (model.NgayLaoDong.HasValue && model.NgayLaoDong.Value.Year < DateTime.Now.Year)
                {
                    ModelState.AddModelError("NgayLaoDong", $"Năm không được nhỏ hơn {DateTime.Now.Year}.");
                }
                else
                {
                    try
                    {
                        using (var db = new DB_QLNLD())
                        {
                            var dotLaoDong = db.TaoDotNgayLaoDongs.Find(model.ID);
                            if (dotLaoDong == null)
                            {
                                return HttpNotFound();
                            }

                            dotLaoDong.DotLaoDong = model.DotLaoDong;
                            dotLaoDong.NgayLaoDong = model.NgayLaoDong;
                            dotLaoDong.Buoi = model.Buoi;
                            dotLaoDong.LoaiLaoDong = model.LoaiLaoDong;
                            dotLaoDong.GiaTri = model.GiaTri;
                            dotLaoDong.ThoiGian = model.ThoiGian;
                            dotLaoDong.KhuVuc = model.KhuVuc;
                            dotLaoDong.MoTa = model.MoTa;

                            db.SaveChanges();

                            TempData["SuccessMessage"] = "Cập nhật đợt lao động thành công!";
                            return RedirectToAction("TrangTaoDotLaoDong");
                        }
                    }
                    catch (Exception ex)
                    {
                        var inner = ex.InnerException?.Message ?? ex.Message;
                        ModelState.AddModelError("", $"Lỗi khi cập nhật dữ liệu: {inner}");
                    }
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            ViewBag.Errors = errors;
            using (var db = new DB_QLNLD())
            {
                ViewBag.TaiKhoans = db.TaiKhoans.ToList();
                return View("TrangTaoDotLaoDong", db.TaoDotNgayLaoDongs.ToList());
            }
        }
        // Khu Vực dành cho xem chi tiết

        [HttpGet]
        public ActionResult ChiTietDotLaoDong(int id)
        {
            using (var db = new DB_QLNLD())
            {
                var dotLaoDong = db.TaoDotNgayLaoDongs.Find(id);
                if (dotLaoDong == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Không tìm thấy đợt lao động với ID: {id}");
                    return HttpNotFound();
                }

                return Json(new
                {
                    ID = dotLaoDong.ID,
                    DotLaoDong = dotLaoDong.DotLaoDong,
                    NgayLaoDong = dotLaoDong.NgayLaoDong?.ToString("dd-MM-yyyy"),
                    KhuVuc = dotLaoDong.KhuVuc,
                    Buoi = dotLaoDong.Buoi,
                    LoaiLaoDong = dotLaoDong.LoaiLaoDong,
                    GiaTri = dotLaoDong.GiaTri,
                    MoTa = dotLaoDong.MoTa,
                    ThoiGian = dotLaoDong.ThoiGian
                }, JsonRequestBehavior.AllowGet);
            }
        }

        // Khu vực dành cho xóa đợt lao động

        [HttpPost]
        public ActionResult XoaDotLaoDong(int id)
        {
            try
            {
                using (var db = new DB_QLNLD())
                {
                    var dotLaoDong = db.TaoDotNgayLaoDongs.Find(id);
                    if (dotLaoDong == null)
                    {
                        return Json(new { success = false, message = "Không tìm thấy đợt lao động." });
                    }

                    db.TaoDotNgayLaoDongs.Remove(dotLaoDong);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Xóa đợt lao động thành công!" });
                }
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                System.Diagnostics.Debug.WriteLine($"Lỗi khi xóa đợt lao động: {inner}");
                return Json(new { success = false, message = $"Lỗi khi xóa: {inner}" });
            }
        }






    }
}