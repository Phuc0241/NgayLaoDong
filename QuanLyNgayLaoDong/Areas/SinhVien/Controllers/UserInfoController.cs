using QuanLyNgayLaoDong.Areas.SinhVien.Modelview;
using QuanLyNgayLaoDong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyNgayLaoDong.Areas.SinhVien.Controllers
{
    public class UserInfoController : Controller
    {
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
                         select new TaiKhoanViewModel
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
                model = new TaiKhoanViewModel
                {
                    HoTen = "Khách",
                    AnhDaiDien = "~/Uploads/avatar/default.png"
                };
            }

            return PartialView("_UserInfoPartial", model);
        }
        //private DB_QLNLD _contextdb = new DB_QLNLD();

        //[ChildActionOnly]
        //public ActionResult GetUserInfo()
        //{
        //    try
        //    {
        //        var username = User.Identity.Name;
        //        if (string.IsNullOrEmpty(username))
        //        {
        //            return PartialView("_UserInfoPartial", new TaiKhoanViewModel
        //            {
        //                HoTen = "Khách",
        //                AnhDaiDien = "~/Uploads/avatar/default.png",
        //                VaiTro = "Khách"
        //            });
        //        }

        //        var model = (from tk in _contextdb.TaiKhoans
        //                     join vt in _contextdb.VaiTroes on tk.role_id equals vt.vaitro_id
        //                     join sv in _contextdb.SinhViens on tk.taikhoan_id equals sv.taikhoan
        //                     join anh in _contextdb.Anhs on sv.anh_id equals anh.anh_id into anhJoin
        //                     from anhLeft in anhJoin.DefaultIfEmpty()
        //                     where tk.username == username
        //                     select new TaiKhoanViewModel
        //                     {
        //                         MSSV = sv.MSSV,
        //                         HoTen = sv.hoten,
        //                         Email = tk.email,
        //                         NgaySinh = sv.ngaysinh,
        //                         GioiTinh = sv.gioitinh.HasValue ? (sv.gioitinh.Value ? "Nam" : "Nữ") : "Chưa cập nhật",
        //                         DiaChi = sv.quequan,
        //                         SoDienThoai = sv.SDT,
        //                         VaiTro = vt.vaitro1,
        //                         AnhDaiDien = anhLeft != null
        //                             ? anhLeft.duongdan
        //                             : (sv.gioitinh.HasValue
        //                                 ? (sv.gioitinh.Value ? "~/Uploads/avatar/man.png"
        //                                                      : "~/Uploads/avatar/woman.png")
        //                                 : "~/Uploads/avatar/default.png")
        //                     }).FirstOrDefault();

        //        if (model == null)
        //        {
        //            model = new TaiKhoanViewModel
        //            {
        //                HoTen = "Khách",
        //                AnhDaiDien = "~/Uploads/avatar/default.png",
        //                VaiTro = "Khách"
        //            };
        //        }

        //        return PartialView("_UserInfoPartial", model);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Ghi log lỗi, hoặc trả về partial view mặc định
        //        return PartialView("_UserInfoPartial", new TaiKhoanViewModel
        //        {
        //            HoTen = "Lỗi",
        //            AnhDaiDien = "~/Uploads/avatar/default.png",
        //            VaiTro = "Lỗi"
        //        });
        //    }
        //}
    }
}