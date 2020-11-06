using EcommerceAdmin.Models.Bal;
using EcommerceAdmin.Models.Common;
using EcommerceAdmin.Models.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace EcommerceAdmin.Controllers
{
    public class LoginController : Controller
    {
        #region Declaration
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        Bal_Guest balGuest = new Bal_Guest();
        Bal_Order balOrder = new Bal_Order();
        #endregion

        // GET: Login
        public ActionResult Register()
        {
            return View();
        }


        
        public ActionResult ActivateAccount(string id)
        {
            SafeTransaction trans = new SafeTransaction();
            int result = 0;
            if (id != null)
            {
                Ent_Guest ent = new Ent_Guest();
                DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                DateTime indiTime = Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd h:m:s"));
                ent.Created_Date = indiTime;               
                result = balGuest.ActivateGuest(ent,id, trans);
                if (result > 0)
                {
                    trans.Commit();
                }
                else
                {
                    result = 0;
                    trans.Rollback();
                }
            }
            ViewBag.Result = result;
            return View();

        }

        public int RegisterGuest(Ent_Guest model)
        {
            int result = 0;
            SafeTransaction trans = new SafeTransaction();
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            DateTime indiTime = Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd h:m:s"));
            model.Created_Date = indiTime;
            model.Guest_FirstName = "";
            string uniqueId = Guid.NewGuid().ToString();
            model.Unique_ID = uniqueId;
            result = balGuest.SaveGuest(model, trans);
            if (result > 0)
            {
                trans.Commit();

                //var lnkHref = "href='https://acsadmin.atintellilabs.live/" + @Url.Action("ActivateAccount", "Login", new { id = uniqueId }) + "'  target='_blank'";

                var lnkHref = "<a href='https://acsadmin.atintellilabs.live/" + @Url.Action("ActivateAccount", "Login", new { id = uniqueId }) + "' target='_blank' style='display: inline-block; padding: 16px 36px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;'>Confirm Your Mail</a>";
                // var lnkHref = uniqueId;

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(Server.MapPath("~/confirm.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{Url}", lnkHref);
                SendMail(body,uniqueId, model.Guest_Username);
            }
            else
            {
                trans.Rollback();
            }
            return result;
        }

        public int SendMail(string Mailbody,string uniqueId, string MailTo)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    int port = 587;
                    string host = "smtp.yandex.com.tr";
                    string sendmail = "mailsupport@intellilabs.co.in";
                    string password = "admin@123";

                    mail.From = new MailAddress(sendmail, "ACSpareparts.com");
                    mail.To.Add(MailTo);
                    mail.Subject = "Account Activation";
                    mail.IsBodyHtml = true;
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Mailbody, null, "text/html");
                    mail.AlternateViews.Add(htmlView);                  
                    using (SmtpClient emailClient = new SmtpClient(host, port))
                    {
                        System.Net.NetworkCredential userInfo = new System.Net.NetworkCredential(sendmail, password);
                        emailClient.UseDefaultCredentials = false;
                        emailClient.EnableSsl = true;
                        emailClient.DeliveryFormat = SmtpDeliveryFormat.International;
                        emailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        if (!string.IsNullOrEmpty(userInfo.UserName.Trim()) && !string.IsNullOrEmpty(userInfo.Password.Trim()))
                        {
                            emailClient.Credentials = userInfo;
                        }
                        emailClient.Send(mail);
                    }

                }
                return 1;

            }
            catch (Exception e)
            {
                return 0;
            }
        }

        //Login      
        public int CreateLogin(string Username,string Password)
        {           
            int i = 0;
            if (Username != "" && Password != "")
            {                                   
                    Ent_Guest entu = new Ent_Guest();
                Ent_Guest result = new Ent_Guest();
                    entu.Guest_Username = Username;
                    entu.Guest_Password = Password;
                    result = balGuest.SelectLogin(entu);
                    if (result!=null)
                    {
                    if (result.Guest_ID > 0)
                    {

                        //Login User ID
                        HttpCookie Guest_ID = new HttpCookie("Guest_ID");
                        Guest_ID.Values["Guest_ID"] = Convert.ToString(result.Guest_ID);
                        Guest_ID.Expires = DateTime.Now.AddMinutes(10);
                        Response.Cookies.Add(Guest_ID);

                        //Login User Name
                        HttpCookie Guest_FirstName = new HttpCookie("Guest_FirstName");
                        if (result.Guest_FirstName != "")
                            Guest_FirstName.Values["Guest_FirstName"] = Convert.ToString(result.Guest_FirstName);
                        else
                            Guest_FirstName.Values["Guest_FirstName"] = Convert.ToString(result.Guest_Username);
                        Guest_FirstName.Expires = DateTime.Now.AddMinutes(10);
                        Response.Cookies.Add(Guest_FirstName);

                        List<Ent_OrderDetail> list = new List<Ent_OrderDetail>();
                        list = balOrder.SelectCart(result.Guest_ID);
                        if (list.Count == 0 && Session["Cart"] != null)
                        {
                            List<Ent_OrderDetail> item = (List<Ent_OrderDetail>)Session["Cart"];
                            SafeTransaction trans = new SafeTransaction();
                            int r = balOrder.InsertCartList(item, result.Guest_ID, trans);
                            if (r > 0)
                            {
                                trans.Commit();
                            }
                            else
                            {
                                trans.Rollback();
                            }
                        }
                        else {
                            Session["Cart"] = list;
                            Session["Total"] = Session["SubTotal"] =list.Sum(y=>y.Product_Total);
                        }
                        i = 1;
                    }
                    else
                    {
                        if (Request.Cookies["Guest_ID"] != null)
                        {
                            HttpCookie myCookie = new HttpCookie("Guest_ID");
                            myCookie.Expires = DateTime.Now.AddDays(-1d);
                            Response.Cookies.Add(myCookie);
                        }

                        if (Request.Cookies["Guest_FirstName"] != null)
                        {
                            HttpCookie myCookie = new HttpCookie("Guest_FirstName");
                            myCookie.Expires = DateTime.Now.AddDays(-1d);
                            Response.Cookies.Add(myCookie);
                        }

                        i = -1;
                    }
                    }
                    else { i = -1; }                
            }
            else
            {
                i = 0;
            }
            return i;
        }

        public ActionResult Logout()
        {
            if (Request.Cookies["Guest_ID"] != null)
            {
                HttpCookie myCookie = new HttpCookie("Guest_ID");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }

            if (Request.Cookies["Guest_FirstName"] != null)
            {
                HttpCookie myCookie = new HttpCookie("Guest_FirstName");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }
            Session["Cart"] = null;
            Session["Total"] = null;
            Session["SubTotal"] = null;
            return RedirectToAction("Index","Home");
        }

        public ActionResult MyAccount()
        {
            HttpCookie Guest_ID = Request.Cookies["Guest_ID"];
            string GuestID = Guest_ID != null ? Guest_ID.Value.Split('=')[1] : "";
            List<Ent_Order> list = new List<Ent_Order>();
            Ent_Guest entGuest = new Ent_Guest();
            if (!string.IsNullOrEmpty(GuestID))
            {
                list = balOrder.SelectGuestOrder(Convert.ToInt32(GuestID));

                entGuest = list[0].entGuest;
            }
            ViewBag.OrderList = list;
                return View(entGuest);
        }
    }
}