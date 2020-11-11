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
    public class HomeController : Controller
    {
        #region Declaration
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        Bal_Category balCategory = new Bal_Category();
        Bal_Product balProduct = new Bal_Product();
        Bal_Master balMaster = new Bal_Master();
        Bal_Order balOrder = new Bal_Order();
        #endregion

        [ChildActionOnly]
        public ActionResult NavPartialMenu()
        {
            List<Ent_Category> listCategory = new List<Ent_Category>();
            listCategory = balCategory.SelectCategoryList(0,"");

            List<Ent_SubCategory> listSubCategory = new List<Ent_SubCategory>();
            listSubCategory = balCategory.SelectSubCategoryList(0);
            for (int i = 0; i < listCategory.Count(); i++)
            {
                listCategory[i].SubCategoryList = listSubCategory.Where(l => l.Parent_Category == listCategory[i].Category_ID).ToList<Ent_SubCategory>();
            }
            return PartialView("NavPartialMenu", listCategory);
        }

        // GET: Home
        public ActionResult Index()
        {
            HttpCookie Guest_ID = Request.Cookies["Guest_ID"];
             string GuestID = Guest_ID != null ? Guest_ID.Value.Split('=')[1] : "";
            if (GuestID != "" && Session["Cart"]==null)
            {
                List<Ent_OrderDetail> list = new List<Ent_OrderDetail>();
                list = balOrder.SelectCart(Convert.ToInt32(GuestID));              
                    Session["Cart"] = list;
                    Session["Total"] = Session["SubTotal"] = list.Sum(y => y.Product_Total);                
            }
            return View();
        }     

       [HttpPost]
        public ActionResult NavPartialProduct(int categoryId, int subCategoryId)
        {
            List<Ent_Product> listProduct = new List<Ent_Product>();          
            listProduct = balProduct.SelectProductFilter(categoryId, subCategoryId, "");
            Session["listProduct"] = listProduct;
            ViewBag.listProduct = listProduct;

            List<Ent_Brand> listBrand = new List<Ent_Brand>();
            listBrand = balMaster.SelectBrandList(0);
            ViewBag.listBrand = listBrand;

            List<Ent_Category> listCategory = new List<Ent_Category>();
            List<Ent_SubCategory> listSubCategory = new List<Ent_SubCategory>();
            listCategory = balCategory.SelectCategoryList(0, "all");
                   
            if (subCategoryId != 0)
            {
                ViewBag.SubCategoryName = listCategory.Where(c => c.Category_ID == subCategoryId).FirstOrDefault().Category_Name;
                categoryId = listCategory.Where(c => c.Category_ID == subCategoryId).FirstOrDefault().Parent_Category;
                ViewBag.CategoryID = categoryId;
                ViewBag.CategoryName = listCategory.Where(c => c.Category_ID == categoryId).FirstOrDefault().Category_Name;
                ViewBag.SubCategoryID = subCategoryId;
            }
            else
            {
                ViewBag.CategoryID = categoryId;
                ViewBag.CategoryName = listCategory.Where(c => c.Category_ID == categoryId).FirstOrDefault().Category_Name;
                ViewBag.SubCategoryName = "";
                ViewBag.SubCategoryID = 0;
            }
            listSubCategory = balCategory.SelectSubCategoryList(categoryId);
            ViewBag.listSubCategory = listSubCategory;
            return PartialView( listCategory);
        }
   
        public ActionResult ProductsList(int categoryId, string SubCategory, string brand)
        {
            List<Ent_Category> listCategory = new List<Ent_Category>();
            List<Ent_SubCategory> listSubCategory = new List<Ent_SubCategory>();
            listCategory = balCategory.SelectCategoryList(0, "all");

            List<Ent_Product> listProduct = new List<Ent_Product>();          
            if (categoryId != 0)
            {
                listProduct = balProduct.SelectProductFilter(categoryId, 0, "");
                Session["listProduct"] = listProduct;
                if (!string.IsNullOrEmpty(SubCategory))
                {
                    List<Ent_Product> objlt = (List<Ent_Product>)Session["listProduct"];
                   listProduct = objlt.Where(v => SubCategory.Contains(v.SubCategory_ID.ToString())).ToList<Ent_Product>();
                    if (!string.IsNullOrEmpty(brand))
                    {
                        listProduct = listProduct.Where(v => brand.Contains(v.Brand_ID.ToString())).ToList<Ent_Product>();
                    }
                }
                ViewBag.SubCategoryIDArray = SubCategory.Split(',');
                ViewBag.SubCategoryID = 0;
            }
            else
            {              
                listProduct = balProduct.SelectProductFilter(0,Convert.ToInt32(SubCategory), "");
                Session["listProduct"] = listProduct;
                 categoryId = listCategory.Where(c => c.Category_ID == Convert.ToInt32(SubCategory)).FirstOrDefault().Parent_Category;
                ViewBag.SubCategoryIDArray = null;
                ViewBag.SubCategoryID = Convert.ToInt32(SubCategory);
                ViewBag.SubCategoryName = listCategory.Where(c => c.Category_ID == Convert.ToInt32(SubCategory)).FirstOrDefault().Category_Name; 

            }
            if (!string.IsNullOrEmpty(brand))
            {
                List<Ent_Product> objlt = (List<Ent_Product>)Session["listProduct"];
                listProduct = objlt.Where(v => brand.Contains(v.Brand_ID.ToString())).ToList<Ent_Product>();
            }
            ViewBag.listProduct = listProduct;
            if(brand!=null)
            ViewBag.BrandArray = brand.Split(',');
            else
                ViewBag.BrandArray = null;
            List<Ent_Brand> listBrand = new List<Ent_Brand>();
            listBrand = balMaster.SelectBrandList(0);
            ViewBag.listBrand = listBrand;                     
            ViewBag.CategoryID = categoryId;
            ViewBag.CategoryName = listCategory.Where(c => c.Category_ID == categoryId).FirstOrDefault().Category_Name;        
            listSubCategory = balCategory.SelectSubCategoryList(categoryId);
            ViewBag.listSubCategory = listSubCategory;
            return View(listCategory);

        }

        [HttpPost]
        public ActionResult PartialProductFilter(int categoryId, string subCategoryId, string brand,int sort)
        {
            List<Ent_Product> listProduct = new List<Ent_Product>();
           
                listProduct = balProduct.SelectProductFilter(categoryId, 0, "");
                Session["listProduct"] = listProduct;            

            if (!string.IsNullOrEmpty(subCategoryId))
            {
                List<Ent_Product> objlt = (List<Ent_Product>)Session["listProduct"];
                listProduct = objlt.Where(v => subCategoryId.Contains(v.SubCategory_ID.ToString())).ToList<Ent_Product>();
                if (!string.IsNullOrEmpty(brand))
                {                   
                    listProduct = listProduct.Where(v => brand.Contains(v.Brand_ID.ToString())).ToList<Ent_Product>();
                }
            }
            else if (!string.IsNullOrEmpty(brand))
            {
                List<Ent_Product> objlt = (List<Ent_Product>)Session["listProduct"];
                listProduct = objlt.Where(v => brand.Contains(v.Brand_ID.ToString())).ToList<Ent_Product>();
            }
            if(sort==4)
            {
                listProduct = listProduct.OrderBy(x => x.Product_Price).ToList<Ent_Product>();
            }
            else if (sort == 5)
            {
                listProduct = listProduct.OrderByDescending(x => x.Product_Price).ToList<Ent_Product>();
            }
            ViewBag.listProduct = listProduct;

            List<Ent_Brand> listBrand = new List<Ent_Brand>();
            listBrand = balMaster.SelectBrandList(0);
            ViewBag.listBrand = listBrand;

            List<Ent_Category> listCategory = new List<Ent_Category>();
            List<Ent_SubCategory> listSubCategory = new List<Ent_SubCategory>();
            listCategory = balCategory.SelectCategoryList(0, "all");

           
                ViewBag.CategoryID = categoryId;
                ViewBag.CategoryName = listCategory.Where(c => c.Category_ID == categoryId).FirstOrDefault().Category_Name;
                ViewBag.SubCategoryName = "";
                ViewBag.SubCategoryID = 0;
            
            listSubCategory = balCategory.SelectSubCategoryList(categoryId);
            ViewBag.listSubCategory = listSubCategory;
            return PartialView("NavPartialProduct",listCategory);
        }


        [HttpPost]
        public ActionResult SearchProducts(string SearchText)
        {
            List<Ent_Product> listProduct = new List<Ent_Product>();

            listProduct = balProduct.SelectProductFilter(0, 0, SearchText);
            Session["listProduct"] = listProduct;         

            ViewBag.listProduct = listProduct;

            List<Ent_Brand> listBrand = new List<Ent_Brand>();
            listBrand = balMaster.SelectBrandList(0);
            ViewBag.listBrand = listBrand;

            List<Ent_Category> listCategory = new List<Ent_Category>();
            List<Ent_SubCategory> listSubCategory = new List<Ent_SubCategory>();
            listCategory = balCategory.SelectCategoryList(0, "all");
            int categoryId = 0;
            if (listProduct.Count > 0)
            {
                categoryId = listProduct[0].Category_ID;
                ViewBag.CategoryID = categoryId;
                ViewBag.CategoryName = listCategory.Where(c => c.Category_ID == categoryId).FirstOrDefault().Category_Name;
                ViewBag.SubCategoryName = "";
                ViewBag.SubCategoryID = 0;


                listSubCategory = balCategory.SelectSubCategoryList(categoryId);
                ViewBag.listSubCategory = listSubCategory;
                return PartialView("NavPartialProduct", listCategory);
            }
            else
            {
                return PartialView("PartialError");
            }           
        }
    }
}