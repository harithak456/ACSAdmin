using EcommerceAdmin.Models.Common;
using EcommerceAdmin.Models.Dal;
using EcommerceAdmin.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceAdmin.Models.Bal
{
    public class Bal_Category
    {
        #region Category
        public int SaveCategory(Ent_Category entGuest, SafeTransaction trans)
        {
            int dataResult = 0;
            try
            {
                Dal_Category dal = new Dal_Category();
                dataResult = dal.SaveCategory(entGuest, trans);
                return dataResult;
            }
            catch
            {
                return 0;
            }
        }

        //Select All Category
        public List<Ent_Category> SelectCategoryList(int categoryId, string condition)
        {
            List<Ent_Category> result = new List<Ent_Category>();
            try
            {
                Dal_Category dal = new Dal_Category();
                result = dal.SelectCategoryList(categoryId,condition);
                return result;
            }
            catch
            {
                return result;
            }
        }

        //Select By Category ID
        public Ent_Category SelectCategory(int categoryId,string condition)
        {
            Ent_Category result = new Ent_Category();
            try
            {
                Dal_Category dal = new Dal_Category();
                result = dal.SelectCategory(categoryId, condition);
                return result;
            }
            catch
            {
                return result;
            }
        }

        //Select All Category
        public List<Ent_SubCategory> SelectSubCategoryList(int categoryId)
        {
            List<Ent_SubCategory> result = new List<Ent_SubCategory>();
            try
            {
                Dal_Category dal = new Dal_Category();
                result = dal.SelectSubCategoryList(categoryId);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public int DeleteCategory(Ent_Category ent, SafeTransaction trans)
        {
            int dataResult;
            try
            {
                Dal_Category dal = new Dal_Category();
                dataResult = dal.DeleteCategory(ent, trans);
                return dataResult;
            }
            catch
            {
                return -1;
            }
        }
        #endregion
    }
}