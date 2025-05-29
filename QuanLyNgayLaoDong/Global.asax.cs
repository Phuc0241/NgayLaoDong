using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace QuanLyNgayLaoDong
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            // Đăng ký sự kiện này để chạy method xử lý sau khi xác thực
            this.PostAuthenticateRequest += Application_PostAuthenticateRequest;
        }
        //protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        //{
        //    HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
        //    if (authCookie != null)
        //    {
        //        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
        //        if (ticket != null && !ticket.Expired)
        //        {
        //            string[] roles = new string[] { };
        //            if (!string.IsNullOrEmpty(ticket.UserData))
        //            {
        //                roles = ticket.UserData.Split(',');
        //            }
        //            GenericPrincipal principal = new GenericPrincipal(new GenericIdentity(ticket.Name), roles);
        //            HttpContext.Current.User = principal;
        //        }
        //    }
        //}
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket != null && !ticket.Expired)
                {
                    string[] roles = ticket.UserData.Split(',');
                    GenericPrincipal principal = new GenericPrincipal(new GenericIdentity(ticket.Name), roles);
                    HttpContext.Current.User = principal;
                }
            }
        }
    }
}
