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
    public class CategoryController : Controller
    {

        #region Declaration
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        Bal_Category balCategory = new Bal_Category();
        Bal_Master balMaster = new Bal_Master();
        #endregion

        //Category
        public ActionResult AddCategory()
        {
            int CategoryId = Request.QueryString["CategoryId"] != null ? Convert.ToInt32(Request.QueryString["CategoryId"]) : 0;
            List<Ent_SubCategory> listSubCategory = new List<Ent_SubCategory>();
            Ent_Category ent = new Ent_Category();
            if (CategoryId != 0)
            {
                ent = balCategory.SelectCategory(CategoryId,"");
                if (ent != null)
                {                   
                    listSubCategory = balCategory.SelectSubCategoryList(CategoryId);                 
                }
            }
            ViewBag.listSubCategory = listSubCategory;
            return View(ent);
        }

        // GET: Category
        public int SaveCategory(Ent_Category model)
        {
            SafeTransaction trans = new SafeTransaction();
         

            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            DateTime indiTime = Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd h:m:s"));
            model.Created_Date = indiTime;
            HttpCookie C_UserID = Request.Cookies["User_ID"];
            string User_ID = C_UserID != null ? C_UserID.Value.Split('=')[1] : "";
            model.Created_By = Convert.ToInt32(User_ID);
            int i = balCategory.SaveCategory(model, trans);
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

        public JsonResult SelectSubCategory(int Category)
        {
            List<Ent_SubCategory> listSubCategory = new List<Ent_SubCategory>();
            listSubCategory = balCategory.SelectSubCategoryList(Category);          
            return Json(listSubCategory, JsonRequestBehavior.AllowGet);
        }

        // GET: Category
        public int SaveBrand(Ent_Brand model)
        {
            SafeTransaction trans = new SafeTransaction();
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            DateTime indiTime = Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd h:m:s"));
            model.Created_Date = indiTime;
            HttpCookie C_UserID = Request.Cookies["User_ID"];
            string User_ID = C_UserID != null ? C_UserID.Value.Split('=')[1] : "";
            model.Created_By = Convert.ToInt32(User_ID);       
            int i = balMaster.SaveBrand(model, trans);
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

        public int DeleteCategory(int Category_ID)
        {
            Ent_Category ent = new Ent_Category();

            ent.Category_ID = Category_ID;

            HttpCookie UserID = Request.Cookies["User_ID"];
            var UserId = UserID != null ? UserID.Value.Split('=')[1] : "";

            ent.Modified_By = Convert.ToInt32(UserId);
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            DateTime indiTime = Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd h:m:s"));
            ent.Modified_Date = indiTime;
            SafeTransaction trans = new SafeTransaction();
            int i = balCategory.DeleteCategory(ent, trans);
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
    }
}