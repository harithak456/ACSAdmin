using EcommerceAdmin.Models.Common;
using EcommerceAdmin.Models.Dal;
using EcommerceAdmin.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EcommerceAdmin.Models.Bal
{
    public class Bal_Master
    {

        #region Organization
        public int SaveOrganization(Ent_Organization entGuest, SafeTransaction trans)
        {
            int dataResult = 0;
            try
            {
                Dal_Master dal = new Dal_Master();
                dataResult = dal.SaveOrganization(entGuest, trans);
                return dataResult;
            }
            catch
            {
                return 0;
            }
        }

        public Ent_Organization SelectOrganization()
        {
            Ent_Organization result = new Ent_Organization();
            try
            {
                Dal_Master dal = new Dal_Master();
                result = dal.SelectOrganization();
                return result;
            }
            catch
            {
                return result;
            }
        }
        #endregion

        #region Brand
        public int SaveBrand(Ent_Brand entGuest, SafeTransaction trans)
        {
            int dataResult = 0;
            try
            {
                Dal_Master dal = new Dal_Master();
                dataResult = dal.SaveBrand(entGuest, trans);
                return dataResult;
            }
            catch
            {
                return 0;
            }
        }
        //Select All Brand
        public List<Ent_Brand> SelectBrandList(int brandId)
        {
            List<Ent_Brand> result = new List<Ent_Brand>();
            try
            {
                Dal_Master dal = new Dal_Master();
                result = dal.SelectBrandList(brandId);
                return result;
            }
            catch
            {
                return result;
            }
        }

        public int DeleteBrand(Ent_Brand ent, SafeTransaction trans)
        {
            int dataResult;
            try
            {
                Dal_Master dal = new Dal_Master();
                dataResult = dal.DeleteBrand(ent, trans);
                return dataResult;
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region User
        public int SaveUser(Ent_User entGuest, SafeTransaction trans)
        {
            int dataResult = 0;
            try
            {
                Dal_Master dal = new Dal_Master();
                dataResult = dal.SaveUser(entGuest, trans);
                return dataResult;
            }
            catch
            {
                return 0;
            }
        }

        public List<Ent_User> SelectUserList(int user_id)
        {
            List<Ent_User> list = new List<Ent_User>();
            try
            {
                Dal_Master dal = new Dal_Master();
                list = dal.SelectUserList(user_id);
                return list;
            }
            catch
            {
                return list;
            }
        }

        public Ent_User SelectUser(int user_id)
        {
            Ent_User list = new Ent_User();
            try
            {
                Dal_Master dal = new Dal_Master();
                list = dal.SelectUser(user_id);
                return list;
            }
            catch
            {
                return list;
            }
        }

        public int DeleteUser(Ent_User ent, SafeTransaction trans)
        {
            int dataResult;
            try
            {
                Dal_Master dal = new Dal_Master();
                dataResult = dal.DeleteUser(ent, trans);
                return dataResult;
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region Login
        public List<Ent_User> SelectLogin(Ent_User entu)
        {
            List<Ent_User> result = new List<Ent_User>();
            try
            {
                Dal_Master dal = new Dal_Master();
                result = dal.SelectLogin(entu);
                return result;
            }
            catch
            {
                return result;
            }
        }
        #endregion

        public DataTable SelectDashboardData()
        {
            DataTable dt = new DataTable();
            try
            {
                Dal_Master dal = new Dal_Master();
                dt = dal.SelectDashboardData();
                return dt;
            }
            catch
            {
                return dt;
            }
        }

        public DataTable SelectYesterdayCount()
        {
            DataTable dt = new DataTable();
            try
            {
                Dal_Master dal = new Dal_Master();
                dt = dal.SelectYesterdayCount();
                return dt;
            }
            catch
            {
                dt.Clear();
            }
            return dt;
        }
    }
}