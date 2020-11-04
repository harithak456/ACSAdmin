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
                List<Ent_Product> item = new List<Ent_Product>();
                item.Add(new Ent_Product()
                {
                    Product_ID = Product_ID,
                    Product_Name=ent.Product_Name,
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
                List<Ent_Product> item = (List<Ent_Product>)Session["Cart"];
                bool has  = item.Any(x => x.Product_ID == Product_ID);
                if (has == false)
                {
                    item.Add(new Ent_Product()
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
                Ent_Product entP = new Ent_Product();
                entP.Cart_ID = cartid;
                entP.Product_ID = Product_ID;
                entP.Quantity = qty;
                entP.Guest_ID = Convert.ToInt32(GuestID);
                SafeTransaction trans = new SafeTransaction();
                int result = balguest.InsertCart(entP, trans);
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
            List<Ent_Product> list = (List<Ent_Product>)Session["Cart"];
            Ent_Product ent = new Ent_Product();
            ent = balProduct.SelectProduct(CartID);
            var qty = list.Where(l => l.Product_ID == CartID).FirstOrDefault().Quantity;
            Session["Cart"] = list.Where(l => l.Product_ID != CartID).ToList<Ent_Product>();
          
            int count = list.Count - 1;
            Session["SubTotal"] = Convert.ToInt32(Session["SubTotal"]) - (ent.Product_Price*qty);
            Session["Total"] = Convert.ToInt32(Session["Total"]) - (ent.Product_Price * qty);
            return count;
        }

        public ActionResult ViewCart()
        {
            return View();
        }

        public int UpdateCart(List<Ent_Product> CartList)
        {
            float total = 0;
           List <Ent_Product> item = new List<Ent_Product>();
            for (int i = 0; i < CartList.Count; i++)
            {
                item.Add(new Ent_Product()
                {
                    Product_ID = CartList[i].Product_ID,
                    Product_Name = CartList[i].Product_Name,
                    Quantity = CartList[i].Quantity,
                    Product_Price = CartList[i].Product_Price,
                    Product_Image = CartList[i].Product_Image,
                    Product_Total = CartList[i].Quantity * CartList[i].Product_Price,                 
                });
                total = total + (CartList[i].Quantity * CartList[i].Product_Price);
                Session["Cart"] = item;
                item = (List<Ent_Product>)Session["Cart"];             
            }
            Session["SubTotal"] = Session["Total"] =  total;
           
            return 1;
        }

    }
}