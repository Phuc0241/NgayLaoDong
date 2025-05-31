using QuanLyNgayLaoDong.Areas.LopTruong.ViewModel;
using QuanLyNgayLaoDong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyNgayLaoDong.Areas.LopTruong.Views.UserInfoLopTruong
{
    public class InfoLayoutlopTruongController : Controller
    {
        // GET: LopTruong/InfoLayoutlopTruong
        private DB_QLNLD _contextdb = new DB_QLNLD();
        [ChildActionOnly]
        public ActionResult ThongTinDangNhap()
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
                             HoTen = sv.hoten,
                             VaiTro = vt.vaitro1,
                             AnhDaiDien = anhLeft != null
                                 ? anhLeft.duongdan
                                 : (sv.gioitinh.HasValue
                                     ? (sv.gioitinh.Value ? "~/Uploads/avatar/man.png" : "~/Uploads/avatar/woman.png")
                                     : "~/Uploads/avatar/default.png")
                         }).FirstOrDefault();

            return PartialView("~/Areas/SinhVien/Views/UserInfo/_UserInfoPartial.cshtml", model);
        }
    }
}