using EcommerceAdmin.Models.Bal;
using EcommerceAdmin.Models.Common;
using EcommerceAdmin.Models.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceAdmin.Controllers
{
    [HandleError]
    public class ProductController : Controller
    {
        #region Declaration
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        Bal_Category balCategory = new Bal_Category();
        Bal_Master balMaster = new Bal_Master();
        Bal_Product balProduct = new Bal_Product();
        #endregion

        public ActionResult AddProduct()
        {
            int ProductId = Request.QueryString["ProductId"] != null ? Convert.ToInt32(Request.QueryString["ProductId"]) : 0;           
            Ent_Product ent = new Ent_Product();

            List<Ent_Category> listCategory = new List<Ent_Category>();
            listCategory = balCategory.SelectCategoryList(0,"");
            ViewBag.listCategory = listCategory;

            if (ProductId != 0)
            {
                ent = balProduct.SelectProduct(ProductId);

                List<Ent_SubCategory> listSubCategory = new List<Ent_SubCategory>();
                listSubCategory = balCategory.SelectSubCategoryList(ent.Category_ID);
                ViewBag.listSubCategory = listSubCategory;

            }
            else
            {
                List<Ent_SubCategory> listSubCategory = new List<Ent_SubCategory>();
                listSubCategory = balCategory.SelectSubCategoryList(listCategory[0].Category_ID);
                ViewBag.listSubCategory = listSubCategory;
            }                     

            List<Ent_Brand> listBrand = new List<Ent_Brand>();
            listBrand = balMaster.SelectBrandList(0);
            ViewBag.listBrand = listBrand;

            return View(ent);
        }

        // GET: Category
        public int SaveProduct(Ent_Product model)
        {
            SafeTransaction trans = new SafeTransaction();
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            DateTime indiTime = Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd h:m:s"));
            model.Created_Date = indiTime;
            //HttpCookie C_UserID = Request.Cookies["User_ID"];
            //string User_ID = C_UserID != null ? C_UserID.Value.Split('=')[1] : "";
            //model.Created_By = Convert.ToInt32(User_ID);
            model.Created_By = 1;

            HttpFileCollectionBase files = Request.Files;
            if (files.Count > 0)
            {
                HttpPostedFileBase file = files["Product_ImageFile"];
                if (file != null)
                {
                    file.SaveAs(Server.MapPath("~/ProductImages/" + model.Product_Image));
                    model.Product_Image = model.Product_Image;
                }
            }
            else if (!System.IO.File.Exists(Server.MapPath("~/ProductImages/" + model.Product_Image)))
            {
                model.Product_Image = "";
            }
            int i = balProduct.SaveProduct(model, trans);
            if (i > 0)
            {
                trans.Commit();
            }
            else
            {
                trans.Rollback();
            }

            return i;
        }

        public int DeleteProduct(int Product_ID)
        {
            Ent_Product ent = new Ent_Product();

            ent.Product_ID = Product_ID;

            //HttpCookie UserID = Request.Cookies["User_ID"];
            //var UserId = UserID != null ? UserID.Value.Split('=')[1] : "";
            //ent.Modified_By = Convert.ToInt32(UserId);
            ent.Modified_By = 1;
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            DateTime indiTime = Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd h:m:s"));
            ent.Modified_Date = indiTime;
            SafeTransaction trans = new SafeTransaction();
            int i = balProduct.DeleteProduct(ent, trans);
            if (i > 0)
            {
                trans.Commit();
            }
            else
            {
                trans.Rollback();
            }

            return i;
        }

        public JsonResult FilterProduct(int Category, int SubCategory, int Brand)
        {
            List<Ent_Product> listProduct = new List<Ent_Product>();
            if (Session["listProduct"] == null)
            {
                listProduct = balProduct.SelectProductList(0);
            }
            else
            {
                listProduct = (List<Ent_Product>)Session["listProduct"];
            }
            if (Category > 0)
            {
                listProduct = listProduct.Where(x => x.Category_ID == Category).ToList<Ent_Product>();
            }

            if (SubCategory > 0)
            {
                listProduct = listProduct.Where(x => x.SubCategory_ID == SubCategory).ToList<Ent_Product>();
            }

            if (Brand > 0)
            {
                listProduct = listProduct.Where(x => x.Brand_ID == Brand).ToList<Ent_Product>();
            }


            return Json(listProduct, JsonRequestBehavior.AllowGet);
        }
    }
}