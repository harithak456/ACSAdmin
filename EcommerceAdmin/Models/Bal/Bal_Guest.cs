using EcommerceAdmin.Models.Common;
using EcommerceAdmin.Models.Dal;
using EcommerceAdmin.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceAdmin.Models.Bal
{
    public class Bal_Guest
    {
        public int SaveGuest(Ent_Guest entGuest,SafeTransaction trans)
        {
            int dataResult = 0;
            try
            {
                Dal_Guest dal = new Dal_Guest();
                dataResult = dal.SaveGuest(entGuest, trans);
                return dataResult;
            }
            catch
            {
                return 0;
            }
        }

        public int ActivateGuest(Ent_Guest entGuest, string token, SafeTransaction trans)
        {
            int dataResult = 0;
            try
            {
                Dal_Guest dal = new Dal_Guest();
                dataResult = dal.ActivateGuest(entGuest, token, trans);
                return dataResult;
            }
            catch
            {
                return 0;
            }
        }

        public Ent_Guest SelectLogin(Ent_Guest entGuest)
        {
            Ent_Guest ent = new Ent_Guest();
            try
            {
                Dal_Guest dal = new Dal_Guest();
                ent = dal.SelectLogin(entGuest);
                return ent;
            }
            catch
            {
                return ent;
            }
        }

        public int InsertCart(Ent_Product entGuest, SafeTransaction trans)
        {
            int dataResult = 0;
            try
            {
                Dal_Guest dal = new Dal_Guest();
                dataResult = dal.InsertCart(entGuest, trans);
                return dataResult;
            }
            catch
            {
                return 0;
            }
        }
        public int InsertCartList(List<Ent_Product> entGuest, int guestID,SafeTransaction trans)
        {
            int dataResult = 0;
            try
            {
                Dal_Guest dal = new Dal_Guest();
                dataResult = dal.InsertCartList(entGuest,guestID, trans);
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
                Dal_Guest dal = new Dal_Guest();
                ent = dal.SelectCart(guestID);
                return ent;
            }
            catch
            {
                return ent;
            }
        }
    }
}