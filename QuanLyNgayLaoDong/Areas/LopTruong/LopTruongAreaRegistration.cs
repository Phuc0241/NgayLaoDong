using System.Web.Mvc;

namespace QuanLyNgayLaoDong.Areas.LopTruong
{
    public class LopTruongAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LopTruong";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LopTruong_default",
                "LopTruong/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}