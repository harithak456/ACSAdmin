﻿using EcommerceAdmin.Models.Bal;
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
    [HandleError]
    public class LoginController : Controller
    {
        #region Declaration
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        Bal_Guest balGuest = new Bal_Guest();
        Bal_Order balOrder = new Bal_Order();
        #endregion

        // GET: Login
        public ActionResult Register(string msg="")
        {
            ViewBag.Msg =msg;
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
                result = balGuest.ActivateGuest(ent, id, trans);
                if (result > 0)
                {
                    trans.Commit();
                }
                else 
                {
                  
                    trans.Rollback();
                }
            }
          
            string message = "";
            if (result > 0)
            {
                message = "Your Email Has Been Confirmed ! Please Login.";

            }
            else if(result!=-2)
            {
                message = "Your Email Not Confirmed.";
            }
            return RedirectToAction("Register", new { msg = message });
            //ViewBag.Result = result;
            //return View();

        }

        public int RegisterGuest(Ent_Guest model)
        {
            int result = 0;
            SafeTransaction trans = new SafeTransaction();
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            DateTime indiTime = Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd h:m:s"));
            model.Created_Date = indiTime;
            string uniqueId = Guid.NewGuid().ToString();
            model.Unique_ID = uniqueId;
            result = balGuest.SaveGuest(model, trans);
            if (result > 0)
            {
                trans.Commit();                
                var lnkHref = "<a href='https://acsadmin.atintellilabs.live/" + @Url.Action("ActivateAccount", "Login", new { id = uniqueId }) + "' target='_blank' style='display: inline-block; padding: 16px 36px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;'>Confirm Your Mail</a>";
               
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(Server.MapPath("~/confirm.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{Url}", lnkHref);
                Email email = new Email();
                email.SendMail(body, model.Guest_Username, "Account Activation");
            }
            else
            {
                trans.Rollback();
            }
            return result;
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
                            myCookie.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(myCookie);
                        }

                        if (Request.Cookies["Guest_FirstName"] != null)
                        {
                            HttpCookie myCookie = new HttpCookie("Guest_FirstName");
                            myCookie.Expires = DateTime.Now.AddDays(-1);
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
                myCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(myCookie);
            }

            if (Request.Cookies["Guest_FirstName"] != null)
            {
                HttpCookie myCookie = new HttpCookie("Guest_FirstName");
                myCookie.Expires = DateTime.Now.AddDays(-1);
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
            int GuestID = Guest_ID != null ? Convert.ToInt32( Guest_ID.Value.Split('=')[1]) : 0;
            List<Ent_Order> list = new List<Ent_Order>();
            List<Ent_GuestAddress> AddressList = new List<Ent_GuestAddress>();
             Ent_Guest entGuest = new Ent_Guest();

            if (GuestID!=0)
            {
                list = balOrder.SelectGuestOrder(GuestID);
                entGuest = balGuest.SelectGuestDetails(GuestID);               
                AddressList = balGuest.SelectGuestAddressList(GuestID);
            }

            ViewBag.OrderList = list;
            Ent_GuestAddress entHome = new Ent_GuestAddress();
            Ent_GuestAddress entWork = new Ent_GuestAddress();
            Ent_GuestAddress entOther = new Ent_GuestAddress();
            var home= AddressList.Where(x => x.Address_Type == "Home").FirstOrDefault();
            var work = AddressList.Where(x => x.Address_Type == "Work").FirstOrDefault();
            var other = AddressList.Where(x => x.Address_Type == "Other").FirstOrDefault();

            if (home != null)
                entHome = home;
            if (work != null)
                entWork = work;
            if (other != null)
                entOther = other;


            ViewBag.HomeAddress = entHome;
            ViewBag.WorkAddress = entWork;
            ViewBag.OtherAddress = entOther;
                return View(entGuest);
        }

        public int UpdateAddress(Ent_GuestAddress model)
        {
            int result = 0;
            HttpCookie Guest_ID = Request.Cookies["Guest_ID"];
            int GuestID = Guest_ID != null ? Convert.ToInt32(Guest_ID.Value.Split('=')[1]) : 0;
            model.Guest_ID = GuestID;
            SafeTransaction trans = new SafeTransaction();        
            result = balGuest.UpdateAddress(model, trans);
            if (result > 0)
            {
                trans.Commit();

            }
            else
            {
                trans.Rollback();
            }
            return result;
        }

    }
}