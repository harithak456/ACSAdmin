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
    [HandleError]
    public class MasterController : Controller
    {
        #region Declaration
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        Bal_Category balCategory = new Bal_Category();
        Bal_Master balMaster = new Bal_Master();
        Bal_Product balProduct=new Bal_Product();
        #endregion

        #region Product

        public ActionResult Products()
        {
            List<Ent_Product> listProduct = new List<Ent_Product>();
            listProduct = balProduct.SelectProductList(0);

            List<Ent_Category> listCategory = new List<Ent_Category>();
            listCategory = balCategory.SelectCategoryList(0, "");
         

            List<Ent_Brand> listBrand = new List<Ent_Brand>();
            listBrand = balMaster.SelectBrandList(0);

            ViewBag.listBrand = listBrand;
            ViewBag.listCategory = listCategory;
            ViewBag.listProduct = listProduct;
            Session["Products"] = listProduct;

            return View();
        }
        // GET: Master     
        #endregion

        #region Brand
        public ActionResult Brand()
        {
            List<Ent_Brand> listBrand = new List<Ent_Brand>();
            listBrand = balMaster.SelectBrandList(0);
            ViewBag.listBrand = listBrand;
            return View();
        }

        public JsonResult SelectBrandList()
        {
            List<Ent_Brand> listBrand = new List<Ent_Brand>();
            listBrand = balMaster.SelectBrandList(0);
            return Json(listBrand,JsonRequestBehavior.AllowGet);
        }

        public int DeleteBrand(int Brand_ID)
        {
            Ent_Brand ent = new Ent_Brand();

            ent.Brand_ID = Brand_ID;

            //HttpCookie UserID = Request.Cookies["User_ID"];
            //var UserId = UserID != null ? UserID.Value.Split('=')[1] : "";
            //ent.Modified_By = Convert.ToInt32(UserId);
            ent.Modified_By = 1;
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            DateTime indiTime = Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd h:m:s"));
            ent.Modified_Date = indiTime;
            SafeTransaction trans = new SafeTransaction();
            int i = balMaster.DeleteBrand(ent, trans);
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
        #endregion

        #region Category       
        public ActionResult Category()
        {
            List<Ent_Category> listCategory = new List<Ent_Category>();
            listCategory = balCategory.SelectCategoryList(0,"");
            ViewBag.listCategory = listCategory;
            return View();
        }
        #endregion

        #region Users       
        public ActionResult Users()
        {
            List<Ent_User> listUsers = new List<Ent_User>();
            listUsers = balMaster.SelectUserList(0);
            ViewBag.listUsers = listUsers;
            return View();
        }

        public ActionResult AddUser()
        {
            int User_ID = Request.QueryString["UserID"] != null ? Convert.ToInt32(Request.QueryString["UserID"]) : 0;
            Ent_User ent = new Ent_User();
            if (User_ID != 0)
            {
                ent = balMaster.SelectUser(User_ID);
            }
            return View(ent);
        }

        public int SaveUser(Ent_User model)
        {
            int result = 0;
            SafeTransaction trans = new SafeTransaction();
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            DateTime indiTime = Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd h:m:s"));
            model.Created_Date = indiTime;
            //HttpCookie User_ID = Request.Cookies["User_ID"];
            //model.Created_By = Convert.ToInt32(User_ID.Value.Split('=')[1]);
            model.Created_By = 1;          
            result = balMaster.SaveUser(model, trans);
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
        public int DeleteUser(int User_ID)
        {
            Ent_User ent = new Ent_User();

            ent.User_ID = User_ID;

            //HttpCookie UserID = Request.Cookies["User_ID"];
            //var UserId = UserID != null ? UserID.Value.Split('=')[1] : "";
            //ent.Modified_By = Convert.ToInt32(UserId);
            ent.Modified_By = 1;
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            DateTime indiTime = Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd h:m:s"));
            ent.Modified_Date = indiTime;
            SafeTransaction trans = new SafeTransaction();
            int i = balMaster.DeleteUser(ent, trans);
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
        #endregion

        #region Organization
        public ActionResult Organization()
        {
            Ent_Organization ent = balMaster.SelectOrganization();
            return View(ent);
        }

        public int SaveOrganization(Ent_Organization model)
        {
            int result = 0;
            SafeTransaction trans = new SafeTransaction();
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            DateTime indiTime = Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd h:m:s"));
            model.Created_Date = indiTime;
            //HttpCookie User_ID = Request.Cookies["User_ID"];
            //model.Created_By = Convert.ToInt32(User_ID.Value.Split('=')[1]);
            model.Created_By = 1;
            result = balMaster.SaveOrganization(model, trans);
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
    #endregion
    }
}
