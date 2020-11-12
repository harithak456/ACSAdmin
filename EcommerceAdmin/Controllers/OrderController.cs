using EcommerceAdmin.Models.Bal;
using EcommerceAdmin.Models.Common;
using EcommerceAdmin.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceAdmin.Controllers
{
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
            string GuestID = Guest_ID != null ? Guest_ID.Value.Split('=')[1] : "";

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
                    Product_Price = ent.Product_Price,
                    Product_Image = ent.Product_Image,
                    Product_Total = ent.Product_Price,
                });
                Session["Cart"] = item;
                Session["SubTotal"] = ent.Product_Price;
                Session["Total"] = ent.Product_Price;
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
                        Product_Price = ent.Product_Price,
                        Product_Image = ent.Product_Image,
                        Product_Total = ent.Product_Price,
                    });
                }
                else
                {
                    item.Where(w => w.Product_ID == Product_ID).ToList().ForEach(i => { i.Quantity = i.Quantity + 1; i.Product_Total = ((i.Quantity) * i.Product_Price); });
                    qty = item.Where(l => l.Product_ID == Product_ID).FirstOrDefault().Quantity;
                    cartid = 1;

                }
                Session["Cart"] = item;
                Session["SubTotal"] = Convert.ToInt32(Session["SubTotal"]) + ent.Product_Price;
                Session["Total"] = Convert.ToInt32(Session["Total"]) + ent.Product_Price;
            }
            if (!string.IsNullOrEmpty(GuestID))
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

        public int DeleteCart(int CartID)
        {
            List<Ent_OrderDetail> list = (List<Ent_OrderDetail>)Session["Cart"];
            Ent_Product ent = new Ent_Product();
            ent = balProduct.SelectProduct(CartID);
            var qty = list.Where(l => l.Product_ID == CartID).FirstOrDefault().Quantity;
            Session["Cart"] = list.Where(l => l.Product_ID != CartID).ToList<Ent_OrderDetail>();

            int count = list.Count - 1;
            Session["SubTotal"] = Convert.ToInt32(Session["SubTotal"]) - (ent.Product_Price * qty);
            Session["Total"] = Convert.ToInt32(Session["Total"]) - (ent.Product_Price * qty);

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

            return count;
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
            model.Guest_ID = Convert.ToInt32(GuestID);

            model.Order_SubTotal = Convert.ToDouble(Session["SubTotal"]);
            model.Order_Shipping = 0;
            model.Order_Total = Convert.ToDouble(Session["Total"]);
            model.OrderDetailsList = (List<Ent_OrderDetail>)Session["Cart"];
            int i = balOrder.SaveOrder(model, trans);
            if (i > 0)
            {
                //Session["Cart"] = null;
                //Session["Total"] = "0.00";
                //Session["SubTotal"] = "0.00";
                //Session["Shipping"] = "0.00";
                trans.Commit();
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
            if (Order_ID != 0)
            {
                list = balOrder.SelectOrderDetails(Order_ID);
                ViewBag.OrderDetails = list;
                ViewBag.SubTotal = list[0].entOrder.Order_SubTotal;
                ViewBag.Shipping = list[0].entOrder.Order_Shipping;
                ViewBag.Total = list[0].entOrder.Order_Total;
            }
            return View();
        }

        public ActionResult Payment()
        {
            return View();
        }
    }
}