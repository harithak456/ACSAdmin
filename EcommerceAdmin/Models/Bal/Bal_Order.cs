using EcommerceAdmin.Models.Common;
using EcommerceAdmin.Models.Dal;
using EcommerceAdmin.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceAdmin.Models.Bal
{
    public class Bal_Order
    {
        public int InsertCart(Ent_Product entGuest, SafeTransaction trans)
        {
            int dataResult = 0;
            try
            {
                Dal_Order dal = new Dal_Order();
                dataResult = dal.InsertCart(entGuest, trans);
                return dataResult;
            }
            catch
            {
                return 0;
            }
        }

        public int DeleteCart(int productId, int GuestID, SafeTransaction trans)
        {
            int dataResult = 0;
            try
            {
                Dal_Order dal = new Dal_Order();
                dataResult = dal.DeleteCart(productId, GuestID, trans);
                return dataResult;
            }
            catch
            {
                return 0;
            }
        }

        public int InsertCartList(List<Ent_Product> entGuest, int guestID, SafeTransaction trans)
        {
            int dataResult = 0;
            try
            {
                Dal_Order dal = new Dal_Order();
                dataResult = dal.InsertCartList(entGuest, guestID, trans);
                return dataResult;
            }
            catch
            {
                return 0;
            }
        }

        public List<Ent_Product> SelectCart(int guestID)
        {
            List<Ent_Product> ent = new List<Ent_Product>();
            try
            {
                Dal_Order dal = new Dal_Order();
                ent = dal.SelectCart(guestID);
                return ent;
            }
            catch
            {
                return ent;
            }
        }

        public int SaveOrder(Ent_Order ent, SafeTransaction trans)
        {
            int dataResult = 0;
            try
            {
                Dal_Order dal = new Dal_Order();
                dataResult = dal.SaveOrder(ent, trans);
                return dataResult;
            }
            catch
            {
                return 0;
            }
        }
    }
}