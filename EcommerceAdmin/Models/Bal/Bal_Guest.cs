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

        public Ent_Guest SelectGuestDetails(int ID)
        {
            Ent_Guest ent = new Ent_Guest();
            try
            {
                Dal_Guest dal = new Dal_Guest();
                ent = dal.SelectGuestDetails(ID);
                return ent;
            }
            catch
            {
                return ent;
            }
        }

    }
}