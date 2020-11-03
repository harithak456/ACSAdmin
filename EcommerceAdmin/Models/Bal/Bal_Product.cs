using EcommerceAdmin.Models.Common;
using EcommerceAdmin.Models.Dal;
using EcommerceAdmin.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceAdmin.Models.Bal
{
    public class Bal_Product
    {
        public int SaveProduct(Ent_Product entGuest, SafeTransaction trans)
        {
            int dataResult = 0;
            try
            {
                Dal_Product dal = new Dal_Product();
                dataResult = dal.SaveProduct(entGuest, trans);
                return dataResult;
            }
            catch
            {
                return 0;
            }
        }

        //Select All ProductList
        public List<Ent_Product> SelectProductList(int productId)
        {
            List<Ent_Product> result = new List<Ent_Product>();
            try
            {
                Dal_Product dal = new Dal_Product();
                result = dal.SelectProductList(productId);
                return result;
            }
            catch
            {
                return result;
            }
        }

        //Select Product By ID
        public Ent_Product SelectProduct(int productId)
        {
            Ent_Product result = new Ent_Product();
            try
            {
                Dal_Product dal = new Dal_Product();
                result = dal.SelectProduct(productId);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public List<Ent_Product> SelectProductFilter(int Category_ID, int SubCategory_ID,string condition)
        {
            List<Ent_Product> result = new List<Ent_Product>();
            try
            {
                Dal_Product dal = new Dal_Product();
                result = dal.SelectProductFilter(Category_ID, SubCategory_ID, condition);
                return result;
            }
            catch
            {
                return result;
            }
        }


        public int DeleteProduct(Ent_Product ent, SafeTransaction trans)
        {
            int dataResult;
            try
            {
                Dal_Product dal = new Dal_Product();
                dataResult = dal.DeleteProduct(ent, trans);
                return dataResult;
            }
            catch
            {
                return -1;
            }
        }
    }
}