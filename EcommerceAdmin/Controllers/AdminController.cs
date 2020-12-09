using EcommerceAdmin.Models.Bal;
using EcommerceAdmin.Models.Common;
using EcommerceAdmin.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceAdmin.Controllers
{
    [HandleError]
    public class AdminController : Controller
    {
        #region Declaration
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");       
        Bal_Master balMaster = new Bal_Master();
        Bal_Order balOrder = new Bal_Order();
        #endregion

        public ActionResult Dashboard()
        {
            DataTable dt = new DataTable();
            dt = balMaster.SelectDashboardData();
            if (dt.Rows.Count > 0)
            {
                ViewBag.TotalOrder = dt.Rows[0]["TotalOrder"];
                ViewBag.NewOrder = dt.Rows[0]["NewOrder"];
                ViewBag.NewOrder = dt.Rows[0]["NewOrder"];
                ViewBag.ShippedOrder = dt.Rows[0]["ShippedOrder"];
                ViewBag.DeliveredOrder = dt.Rows[0]["DeliveredOrder"];
                ViewBag.ReturnOrder = dt.Rows[0]["ReturnOrder"];
                ViewBag.UserRegistration = dt.Rows[0]["UserRegistration"];
                ViewBag.UniqueVisitors = dt.Rows[0]["UniqueVisitors"];
            }

            DataTable dtD = balMaster.SelectYesterdayCount();
            if (dtD.Rows.Count > 0)
            {
                ViewBag.RegisteredGuest = dtD.Rows[0]["RegisteredGuest"];
                ViewBag.UniqueGuest = dtD.Rows[0]["UniqueGuest"];
            }
            List<Ent_Order> OrderList = new List<Ent_Order>();

            OrderList = balOrder.SelectGuestOrder(0);
            ViewBag.OrderList = OrderList.Where(x=>x.Created_Date.ToString("dd/MM/yyyy")==DateTime.Now.ToString("dd/MM/yyyy")).ToList<Ent_Order>();

            return View();
        }

        public ActionResult NavigationPartial()
        {
            DataTable dt = new DataTable();
            dt = balMaster.SelectDashboardData();
            if (dt.Rows.Count > 0)
            {
                ViewBag.OrderToday = dt.Rows[0]["OrderToday"];
                ViewBag.RegistrationToday = dt.Rows[0]["RegistrationToday"];
            }
            return PartialView();

        }

        public ActionResult NotFound()
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
                        User_ID.Expires = DateTime.Now.AddMinutes(10);
                        Response.Cookies.Add(User_ID);                  

                        HttpCookie User_Name = new HttpCookie("User_Name");
                        User_Name.Values["User_Name"] = Convert.ToString(result[0].User_Name);
                        User_Name.Expires = DateTime.Now.AddMinutes(10);
                        Response.Cookies.Add(User_Name);

                        HttpCookie User_Type = new HttpCookie("User_Type");
                        User_Type.Values["User_Type"] = Convert.ToString(result[0].User_Type);
                        User_Type.Expires = DateTime.Now.AddMinutes(10);
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

        public ActionResult OrderList(string flag)
        {
            List<Ent_Order> OrderList = new List<Ent_Order>();
           
            OrderList = balOrder.SelectGuestOrder(0);
            ViewBag.OrderList = OrderList;
            ViewBag.flag = flag;
            return View();
        }

        public ActionResult Orders()
        {
            int OrderID = Request.QueryString["OrderID"] != null ? Convert.ToInt32(Request.QueryString["OrderID"]) : 0;
            Ent_Order entOrder = new Ent_Order();
            entOrder = balOrder.SelectOrder(OrderID);
            List<Ent_OrderDetail> OrderList = new List<Ent_OrderDetail>();
            OrderList = balOrder.SelectOrderDetails(OrderID);
            ViewBag.OrderList = OrderList;
            return View(entOrder);
        }

        public int UpdateNotification()
        {
            int i = balOrder.UpdateNotification(0);
            return i;
        }

        public RedirectToRouteResult UpdateOrderNotification()
        {
            int i = balOrder.UpdateNotification(1);
            return RedirectToAction("OrderList", "Admin", new { flag = "1" });
        }

        public int UpdateOrderStatus(Ent_Order ent)
        {
            int result = 0;
            SafeTransaction trans = new SafeTransaction();                     
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            DateTime indiTime = Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd h:m:s"));
            ent.Created_Date = indiTime;
            result = balOrder.UpdateOrderStatus(ent, trans);
            if (result > 0)
            {
                trans.Commit();
                string body = string.Empty; var lnkHref = "";
                if (ent.Is_Active == 2)
                {
                    if (ent.Guest_ID == 0)
                    {
                        lnkHref = "<a href='https://acsadmin.atintellilabs.live/" + @Url.Action("TrackOrder", "Order", new { Order_ID = ent.Order_ID }) + "' target = '_blank' style = 'color: #fc7ca0;' > here </a>";
                    }
                    else
                    {
                        lnkHref = "<a href='https://acsadmin.atintellilabs.live/" + @Url.Action("Register", "Login") + "' target = '_blank' style = 'color: #fc7ca0;' > here </a>";
                    }

                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Shipping.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    body = body.Replace("{Url}", lnkHref);

                    Email em = new Email();
                    em.SendConfirmationMail(ent.Order_ID, body, "Order Shipped");
                }
                else if (ent.Is_Active == 3)
                {
                    if (ent.Guest_ID == 0)
                    {
                        lnkHref = "<a href='https://acsadmin.atintellilabs.live/" + @Url.Action("TrackOrder", "Order", new { Order_ID = ent.Order_ID }) + "' target = '_blank' style = 'color: #fc7ca0;' > here </a>";
                    }
                    else
                    {
                        lnkHref = "<a href='https://acsadmin.atintellilabs.live/" + @Url.Action("Register", "Login") + "' target = '_blank' style = 'color: #fc7ca0;' > here </a>";
                    }

                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Delivered.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    body = body.Replace("{Url}", lnkHref);

                    Email eml = new Email();
                    eml.SendConfirmationMail(ent.Order_ID, body, "Order Delivered");
                }
                
            }
            else { trans.Rollback(); }
            return result;
        }
    }
}