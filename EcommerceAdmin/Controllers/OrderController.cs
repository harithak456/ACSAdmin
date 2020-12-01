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
    [HandleError]
    public class OrderController : Controller
    {
        #region Declaration
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        Bal_Category balCategory = new Bal_Category();
        Bal_Master balMaster = new Bal_Master();
        Bal_Product balProduct = new Bal_Product();
        Bal_Guest balguest = new Bal_Guest();
        Bal_Order balOrder = new Bal_Order();
        #endregion

        // GET: Order
        public int AddToCart(int Product_ID)
        {
            Ent_Product ent = new Ent_Product();
            ent = balProduct.SelectProduct(Product_ID);
            HttpCookie Guest_ID = Request.Cookies["Guest_ID"];
            int GuestID = Guest_ID != null ?Convert.ToInt32( Guest_ID.Value.Split('=')[1]) : 0;

            int qty = 1;
            int cartid = 0;
            if (Session["Cart"] == null)
            {
                List<Ent_OrderDetail> item = new List<Ent_OrderDetail>();
                item.Add(new Ent_OrderDetail()
                {
                    Product_ID = Product_ID,
                    Product_Name = ent.Product_Name,
                    Quantity = 1,
                    Product_Price =ent.Product_Price-ent.Product_Discount,
                    Product_Image = ent.Product_Image,
                    Product_Total = ent.Product_Price - ent.Product_Discount,
                });
                Session["Cart"] = item;
                Session["SubTotal"] = ent.Product_Price - ent.Product_Discount;
                Session["Total"] = ent.Product_Price - ent.Product_Discount;
            }
            else
            {
                List<Ent_OrderDetail> item = (List<Ent_OrderDetail>)Session["Cart"];
                bool has = item.Any(x => x.Product_ID == Product_ID);
                if (has == false)
                {
                    item.Add(new Ent_OrderDetail()
                    {
                        Product_ID = Product_ID,
                        Product_Name = ent.Product_Name,
                        Quantity = 1,
                        Product_Price = ent.Product_Price - ent.Product_Discount,
                        Product_Image = ent.Product_Image,
                        Product_Total = ent.Product_Price - ent.Product_Discount,
                    });
                }
                else
                {
                    item.Where(w => w.Product_ID == Product_ID).ToList().ForEach(i => { i.Quantity = i.Quantity + 1; i.Product_Total = ((i.Quantity) * i.Product_Price); });
                    qty = item.Where(l => l.Product_ID == Product_ID).FirstOrDefault().Quantity;
                    cartid = 1;

                }
                Session["Cart"] = item;
                Session["SubTotal"] = Convert.ToInt32(Session["SubTotal"]) + (ent.Product_Price - ent.Product_Discount);
                Session["Total"] = Convert.ToInt32(Session["Total"]) +( ent.Product_Price - ent.Product_Discount);
            }
            if (GuestID!=0)
            {
                Ent_OrderDetail entP = new Ent_OrderDetail();
                entP.Cart_ID = cartid;
                entP.Product_ID = Product_ID;
                entP.Quantity = qty;
                entP.Guest_ID = Convert.ToInt32(GuestID);
                SafeTransaction trans = new SafeTransaction();
                int result = balOrder.InsertCart(entP, trans);
                if (result > 0)
                {
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                }
            }


            return 1;
        }

        public ActionResult DeleteCart(int CartID)
        {
            List<Ent_OrderDetail> list = (List<Ent_OrderDetail>)Session["Cart"];
            Ent_Product ent = new Ent_Product();
            ent = balProduct.SelectProduct(CartID);
            var qty = list.Where(l => l.Product_ID == CartID).FirstOrDefault().Quantity;

            Session["Cart"] = list.Where(l => l.Product_ID != CartID).ToList<Ent_OrderDetail>();

            int count = list.Count - 1;
            Session["SubTotal"] = Convert.ToInt32(Session["SubTotal"]) - ((ent.Product_Price - ent.Product_Discount) * qty);
            Session["Total"] = Convert.ToInt32(Session["Total"]) - ((ent.Product_Price - ent.Product_Discount) * qty);
            if (list.Count == 1)
            {
                Session["Cart"] = null;
                Session["Total"] = "0.00";
                Session["SubTotal"] = "0.00";
                Session["Shipping"] = "0.00";
            }
                
                HttpCookie Guest_ID = Request.Cookies["Guest_ID"];
            string GuestID = Guest_ID != null ? Guest_ID.Value.Split('=')[1] : "";
            if (!string.IsNullOrEmpty(GuestID))
            {
                SafeTransaction trans = new SafeTransaction();
                int result = balOrder.DeleteCart(CartID, Convert.ToInt32(GuestID), trans);
                if (result > 0)
                {
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                }
            }
            string[] Result = { count.ToString() , Session["Total"].ToString(), Session["SubTotal"].ToString() };
        
            return Json(Result,JsonRequestBehavior.AllowGet) ;
        }

        public ActionResult ViewCart()
        {
            return View();
        }

        public int UpdateCart(List<Ent_Product> CartList)
        {
            float total = 0;
            List<Ent_OrderDetail> item = new List<Ent_OrderDetail>();
            for (int i = 0; i < CartList.Count; i++)
            {
                item.Add(new Ent_OrderDetail()
                {
                    Product_ID = CartList[i].Product_ID,
                    Product_Name = CartList[i].Product_Name,
                    Quantity = CartList[i].Quantity,
                    Product_Price = CartList[i].Product_Price,
                    Product_Image = CartList[i].Product_Image,
                    Product_Total = CartList[i].Quantity * CartList[i].Product_Price,
                });

                HttpCookie Guest_ID = Request.Cookies["Guest_ID"];
                string GuestID = Guest_ID != null ? Guest_ID.Value.Split('=')[1] : "";
                if (!string.IsNullOrEmpty(GuestID))
                {
                    Ent_OrderDetail entP = new Ent_OrderDetail();
                    entP.Cart_ID = 1;
                    entP.Product_ID = CartList[i].Product_ID;
                    entP.Quantity = CartList[i].Quantity;
                    entP.Guest_ID = Convert.ToInt32(GuestID);
                    SafeTransaction trans = new SafeTransaction();
                    int result = balOrder.InsertCart(entP, trans);
                    if (result > 0)
                    {
                        trans.Commit();
                    }
                    else
                    {
                        trans.Rollback();
                    }
                }

                total = total + (CartList[i].Quantity * CartList[i].Product_Price);
                Session["Cart"] = item;
                item = (List<Ent_OrderDetail>)Session["Cart"];
            }
            Session["SubTotal"] = Session["Total"] = total;

            return 1;
        }

        public ActionResult Checkout()
        {
            HttpCookie Guest_ID = Request.Cookies["Guest_ID"];
            int GuestID = Guest_ID != null ? Convert.ToInt32(Guest_ID.Value.Split('=')[1]) : 0;
            Ent_Guest ent = new Ent_Guest();
            ent = balguest.SelectGuestDetails(GuestID);
            return View(ent);
        }

        public int SaveOrder(Ent_Order model)
        {          
            SafeTransaction trans = new SafeTransaction();
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            DateTime indiTime = Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd h:m:s"));
            model.Created_Date = indiTime;

            HttpCookie Guest_ID = Request.Cookies["Guest_ID"];
            string GuestID = Guest_ID != null ? Guest_ID.Value.Split('=')[1] : "";
            if (!string.IsNullOrEmpty(GuestID))
            {
                model.Guest_ID = Convert.ToInt32(GuestID);
                Ent_GuestAddress entAddress = new Ent_GuestAddress();
                entAddress = balguest.SelectGuestAddress(Convert.ToInt32(GuestID));
                if (entAddress.Guest_Address1 != "")
                {
                    model.Billing_FirstName = entAddress.First_Name;
                    model.Billing_LastName = entAddress.Last_Name;
                    model.Billing_Address1 = entAddress.Guest_Address1;
                    model.Billing_Address2 = entAddress.Guest_Address2;
                    model.Billing_State = entAddress.Guest_State;
                    model.Billing_Town = entAddress.Guest_Town;
                    model.Billing_Country = entAddress.Guest_Country;
                }
                else
                {                   
                    model.Billing_FirstName = model.Shipping_FirstName;
                    model.Billing_LastName = model.Shipping_LastName;
                    model.Billing_Address1 = model.Shipping_Address1;
                    model.Billing_Address2 = model.Shipping_Address2;
                    model.Billing_State = model.Shipping_State;
                    model.Billing_Town = model.Shipping_Town;
                    model.Billing_Country = model.Shipping_Country;
                }
            }
            else
            {
                model.Guest_ID = 0;
                model.Billing_FirstName = model.Shipping_FirstName;
                model.Billing_LastName = model.Shipping_LastName;              
                model.Billing_Address1 = model.Shipping_Address1;
                model.Billing_Address2 = model.Shipping_Address2;
                model.Billing_State = model.Shipping_State;
                model.Billing_Town= model.Shipping_Town;
                model.Billing_Country = model.Shipping_Country;               
            }

            model.Billing_Email = model.Shipping_Email;
            model.Billing_Phone = model.Shipping_Phone;

            model.Order_SubTotal = Convert.ToDouble(Session["SubTotal"]);
            model.Order_Shipping = 0;
            model.Order_Total = Convert.ToDouble(Session["Total"]);
            model.OrderDetailsList = (List<Ent_OrderDetail>)Session["Cart"];
            int i = balOrder.SaveOrder(model, trans);
            if (i > 0)
            {
                trans.Commit();
                if (model.Payment_COD == 1)
                {                    
                        string body = string.Empty; var lnkHref = "";
                     if (string.IsNullOrEmpty(GuestID))
                     {
                            lnkHref = "<a href='https://acsadmin.atintellilabs.live/" + @Url.Action("TrackOrder", "Order", new { Order_ID = i }) + "' target = '_blank' style = 'color: #fc7ca0;' > here </ a >";
                        }
                        else
                        {
                            lnkHref = "<a href='https://acsadmin.atintellilabs.live/" + @Url.Action("Register", "Login") + "' target = '_blank' style = 'color: #fc7ca0;' > here </ a >";
                        }

                        using (StreamReader reader = new StreamReader(Server.MapPath("~/OrderConfirmation.html")))
                        {
                            body = reader.ReadToEnd();
                        }
                        body = body.Replace("{Url}", lnkHref);

                        Email em = new Email();
                        em.SendConfirmationMail(i, body, "Order Confirmation");
                        Session["Cart"] = null;
                        Session["Total"] = null;
                        Session["SubTotal"] = null;
                        Session["Shipping"] = null;                    
                }
            }
            else
            {
                trans.Rollback();
            }

            return i;
        }
        public int UpdatePayment(Ent_Order entOrder)
        {
            SafeTransaction trans = new SafeTransaction();
            HttpCookie Guest_ID = Request.Cookies["Guest_ID"];
            int ID = Guest_ID != null ? Convert.ToInt32(Guest_ID.Value.Split('=')[1]) : 0;
            entOrder.Guest_ID = ID;
            int i = balOrder.UpdatePayment(entOrder,   trans);
            if (i > 0)
            {
                trans.Commit();
                if (entOrder.Payment_Status == "CAPTURED")
                {
                    string body = string.Empty; var lnkHref = "";
                    if (ID == 0)
                    {
                         lnkHref = "<a href='https://acsadmin.atintellilabs.live/" + @Url.Action("TrackOrder", "Order", new { Order_ID = entOrder.Order_ID }) + "' target = '_blank' style = 'color: #fc7ca0;' > here </ a >";
                    }
                    else
                    {
                         lnkHref = "<a href='https://acsadmin.atintellilabs.live/" + @Url.Action("Register", "Login") + "' target = '_blank' style = 'color: #fc7ca0;' > here </ a >";
                    }

                    using (StreamReader reader = new StreamReader(Server.MapPath("~/OrderConfirmation.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    body = body.Replace("{Url}", lnkHref);

                    Email em = new Email();
                    em.SendConfirmationMail(entOrder.Order_ID, body, "Order Confirmation");
                    Session["Cart"] = null;
                    Session["Total"] = null;
                    Session["SubTotal"] = null;
                    Session["Shipping"] = null;
                }
            }
            else
            {
                trans.Rollback();
            }
            
            return i;
        }
    

        public ActionResult ViewOrder()
        {
            int Order_ID = Request.QueryString["Id"] != null ? Convert.ToInt32(Request.QueryString["Id"]) : 0;
            List<Ent_OrderDetail> list = new List<Ent_OrderDetail>();
            Ent_Order ent = new Ent_Order();
            if (Order_ID != 0)
            {
                list = balOrder.SelectOrderDetails(Order_ID);
                ViewBag.OrderDetails = list;
                if(list.Count>0)
                ent = list[0].entOrder;             
            }
            return View(ent);
        }

        public ActionResult Payment(int id)
        {           
            ViewBag.Order_ID = id;
            return View();
        }

        public ActionResult TrackOrder(int Order_ID)
        {
            List<Ent_OrderDetail> list = new List<Ent_OrderDetail>();
            Ent_Order ent = new Ent_Order();
            if (Order_ID != 0)
            {
                list = balOrder.SelectOrderDetails(Order_ID);
                ViewBag.OrderDetails = list;
                if (list.Count > 0)
                    ent = list[0].entOrder;
            }
            return View(ent);           
        }
    }
}