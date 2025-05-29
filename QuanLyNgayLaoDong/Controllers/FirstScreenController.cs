using QuanLyNgayLaoDong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyNgayLaoDong.Controllers
{
    public class FirstScreenController : Controller
    {
        // GET: FirstScreen
        public ActionResult Index()
        {
            var user = Session["User"] as User;
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
    }
}