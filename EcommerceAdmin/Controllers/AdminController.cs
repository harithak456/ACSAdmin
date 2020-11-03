using EcommerceAdmin.Models.Bal;
using EcommerceAdmin.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceAdmin.Controllers
{
    public class AdminController : Controller
    {
        #region Declaration
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");       
        Bal_Master balMaster = new Bal_Master();
        #endregion

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        //Login
        public int CreateLogin(string Username, string Password)
        {
            int i = 0;
            if (Username != "" && Password != "")
            {
                List<Ent_User> result = new List<Ent_User>();
                Ent_User entu = new Ent_User();
                entu.User_Username = Username;
                entu.User_Password = Password;
                result = balMaster.SelectLogin(entu);
                if (result.Count > 0)
                {
                    if (result[0].User_ID > 0)
                    {
                        HttpCookie User_ID = new HttpCookie("User_ID");
                        User_ID.Values["User_ID"] = Convert.ToString(result[0].User_ID);
                        User_ID.Expires = DateTime.Now.AddDays(30);
                        Response.Cookies.Add(User_ID);                  

                        HttpCookie User_Name = new HttpCookie("User_Name");
                        User_Name.Values["User_Name"] = Convert.ToString(result[0].User_Name);
                        User_Name.Expires = DateTime.Now.AddDays(30);
                        Response.Cookies.Add(User_Name);

                        HttpCookie User_Type = new HttpCookie("User_Type");
                        User_Type.Values["User_Type"] = Convert.ToString(result[0].User_Type);
                        User_Type.Expires = DateTime.Now.AddDays(30);
                        Response.Cookies.Add(User_Type);
                    
                        i = 1;
                    }
                    else
                    {                       
                        i = -1;
                    }
                }
                else { i = -1; }
            }
            else { i = 0; }
            return i;
        }

        //LogOut
        public int CreateLogout()
        {
            HttpCookie User_ID = Request.Cookies["User_ID"];
            User_ID.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(User_ID);

            HttpCookie User_Name = Request.Cookies["User_Name"];
            User_Name.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(User_Name);

            HttpCookie User_Type = Request.Cookies["User_Type"];
            User_Type.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(User_Type);
            return 1;
        }
    }
}