using QuanLyNgayLaoDong.Areas.LopTruong.ViewModel;
using QuanLyNgayLaoDong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyNgayLaoDong.Areas.LopTruong.Controllers
{
    public class UserInfoLopTruongController : Controller
    {
        // GET: LopTruong/UserInfoLopTruong
        private readonly DB_QLNLD _contextdb = new DB_QLNLD();

        [ChildActionOnly] // Dùng trong layout
        public ActionResult GetUserInfo()
        {
            var username = User.Identity.Name;

            var model = (from tk in _contextdb.TaiKhoans
                         join sv in _contextdb.SinhViens on tk.taikhoan_id equals sv.taikhoan
                         join anh in _contextdb.Anhs on sv.anh_id equals anh.anh_id into anhJoin
                         from anhLeft in anhJoin.DefaultIfEmpty()
                         where tk.username == username
                         select new TaiKhoanLopTruongViewModel
                         {
                             HoTen = sv.hoten,
                             AnhDaiDien = anhLeft != null
                                 ? anhLeft.duongdan
                                 : (sv.gioitinh.HasValue
                                     ? (sv.gioitinh.Value ? "~/Uploads/avatar/man.png" : "~/Uploads/avatar/woman.png")
                                     : "~/Uploads/avatar/default.png")
                         }).FirstOrDefault();

            if (model == null)
            {
                model = new TaiKhoanLopTruongViewModel
                {
                    HoTen = "Khách",
                    AnhDaiDien = "~/Uploads/avatar/default.png"
                };
            }

            return PartialView("_UserInfoPartial", model);
        }
    }
}