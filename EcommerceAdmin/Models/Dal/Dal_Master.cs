using EcommerceAdmin.Models.Common;
using EcommerceAdmin.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EcommerceAdmin.Models.Dal
{
    public class Dal_Master
    {
        SqlConnection con = null;
        DBConnection dbcon = new DBConnection();
        public Dal_Master()
        {
            con = dbcon.DatabaseConnection;
        }

        #region Organization
        public int SaveOrganization(Ent_Organization ent, SafeTransaction trans)
        {
            int dataresult = 0;
            int dataresult1 = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (SqlCommand cmd = new SqlCommand("EC_InsertOrganization", trans.DatabaseConnection, trans.Transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Organization_ID", ent.Organization_ID));
                    cmd.Parameters.Add(new SqlParameter("@Organization_Name", ent.Organization_Name));
                    cmd.Parameters.Add(new SqlParameter("@Organization_Address", ent.Organization_Address));
                    cmd.Parameters.Add(new SqlParameter("@Organization_ContactPerson", ent.Organization_ContactPerson));
                    cmd.Parameters.Add(new SqlParameter("@Organization_State", ent.Organization_State));
                    cmd.Parameters.Add(new SqlParameter("@Organization_Country", ent.Organization_Country));
                    cmd.Parameters.Add(new SqlParameter("@Organization_Phone", ent.Organization_Phone));
                    cmd.Parameters.Add(new SqlParameter("@organization_email", ent.organization_Email));
                    cmd.Parameters.Add(new SqlParameter("@organization_web", ent.organization_Web));
                    cmd.Parameters.Add(new SqlParameter("@Created_Date", ent.Created_Date));
                    cmd.Parameters.Add(new SqlParameter("@Created_By", ent.Created_By));
                    try
                    {
                        dataresult = Convert.ToInt32(cmd.ExecuteScalar());
                        if (dataresult > 0)
                        {
                            cmd.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        dataresult = -2;
                        InsertException(ex.Message, "SaveOrganization", ent.Organization_ID);
                    }
                }

                if (dataresult > 0)
                {
                    using (SqlCommand cmd = new SqlCommand("EC_InsertLog", trans.DatabaseConnection, trans.Transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Created_By", ent.Created_By));
                        cmd.Parameters.Add(new SqlParameter("@Created_Date", ent.Created_Date));
                        cmd.Parameters.Add(new SqlParameter("@Primary_Id", dataresult));
                        if (ent.Organization_ID > 0)
                            cmd.Parameters.Add(new SqlParameter("@Log_Action", "Update Organization"));
                        else
                            cmd.Parameters.Add(new SqlParameter("@Log_Action", "Insert Organization"));
                        cmd.Parameters.Add(new SqlParameter("@Log_Status", "Admin"));
                        try
                        {
                            dataresult1 = Convert.ToInt32(cmd.ExecuteScalar());
                            if (dataresult1 > 0)
                            {
                                cmd.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            dataresult1 = -2;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                dataresult = -2;
            }
            finally { con.Close(); }
            return dataresult;
        }

        public Ent_Organization SelectOrganization()
        {
            Ent_Organization ent = new Ent_Organization();
            try
            {
                string query = "select * from EC_Organization where Is_Active=1;";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ent = new Ent_Organization();
                        ent.Organization_ID = Convert.ToInt32(dr["Organization_ID"]);
                        ent.Organization_Name = Convert.ToString(dr["Organization_Name"]);
                        ent.Organization_Address = Convert.ToString(dr["Organization_Address"]);
                        ent.Organization_ContactPerson = Convert.ToString(dr["Organization_ContactPerson"]);
                        ent.Organization_Phone = Convert.ToString(dr["Organization_Phone"]);
                        ent.Organization_State = Convert.ToString(dr["Organization_State"]);
                        ent.Organization_Country = Convert.ToString(dr["Organization_Country"]);
                        ent.organization_Email = Convert.ToString(dr["organization_email"]);
                        ent.organization_Web = Convert.ToString(dr["organization_web"]);
                    }
                }
            }
            catch (Exception e)
            {

            }
            finally { con.Close(); }
            return ent;
        }
        #endregion

        #region Brand
        public int SaveBrand(Ent_Brand ent, SafeTransaction trans)
        {
            int dataresult = 0;
            int dataresult1 = 0;        
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                //Insert To category Main Table
                using (SqlCommand cmd = new SqlCommand("EC_InsertBrand", trans.DatabaseConnection, trans.Transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Brand_ID", ent. Brand_ID));
                    cmd.Parameters.Add(new SqlParameter("@Brand_Name", ent.Brand_Name));             
                    cmd.Parameters.Add(new SqlParameter("@Created_Date", ent.Created_Date));
                    cmd.Parameters.Add(new SqlParameter("@Created_By", ent.Created_By));
                    try
                    {
                        dataresult = Convert.ToInt32(cmd.ExecuteScalar());
                        if (dataresult > 0)
                        {
                            cmd.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        //General gl = new General();
                        //gl.UpdateErrorLog(Ex, "CreateDietPlan");

                        dataresult = -2;
                        InsertException(ex.Message, "SaveBrand", ent.Brand_ID);                      
                    }
                }

                if (dataresult > 0)
                {                   
                    using (SqlCommand cmd = new SqlCommand("EC_InsertLog", trans.DatabaseConnection, trans.Transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Created_By", ent.Created_By));
                        cmd.Parameters.Add(new SqlParameter("@Created_Date", ent.Created_Date));
                        cmd.Parameters.Add(new SqlParameter("@Primary_Id", dataresult));
                        if (ent.Brand_ID > 0)
                            cmd.Parameters.Add(new SqlParameter("@Log_Action", "Update Brand"));
                        else
                            cmd.Parameters.Add(new SqlParameter("@Log_Action", "Insert Brand"));
                        cmd.Parameters.Add(new SqlParameter("@Log_Status", "Admin"));
                        try
                        {
                            dataresult1 = Convert.ToInt32(cmd.ExecuteScalar());
                            if (dataresult1 > 0)
                            {
                                cmd.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            dataresult1 = -2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dataresult1 = -2;
            }
            finally { con.Close(); }
      
            return dataresult;
        }

        //Select All Brand
        public List<Ent_Brand> SelectBrandList(int brandId)
        {
            List<Ent_Brand> result = new List<Ent_Brand>();
            Ent_Brand ent = new Ent_Brand();
            try
            {
                using (SqlCommand cmd = new SqlCommand("EC_Selectbrand", con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Brand_ID", brandId));
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ent = new Ent_Brand();
                        ent.Brand_ID = Convert.ToInt32(dr["Brand_ID"]);
                        ent.Brand_Name = Convert.ToString(dr["Brand_Name"]);                                            
                        result.Add(ent);
                    }
                }
            }
            catch (Exception ex)
            {
                InsertException(ex.Message, "SelectCategoryList", brandId);           
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public int DeleteBrand(Ent_Brand ent, SafeTransaction trans)
        {
            int dataResult = 0;
            int dataresult1 = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (SqlCommand cmd = new SqlCommand("EC_DeleteBrand", trans.DatabaseConnection, trans.Transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Brand_ID", ent.Brand_ID));
                    cmd.Parameters.Add(new SqlParameter("@Modified_By", ent.Modified_By));
                    cmd.Parameters.Add(new SqlParameter("@Modified_Date", ent.Modified_Date));
                    try
                    {
                        dataResult = Convert.ToInt32(cmd.ExecuteScalar());
                        cmd.Dispose();
                    }
                    catch (Exception e)
                    {
                        InsertException(e.Message, "DeleteBrand", ent.Brand_ID);
                        dataResult = -1;
                    }
                }

                if (dataResult > 0)
                {
                    using (SqlCommand cmd = new SqlCommand("EC_InsertLog", trans.DatabaseConnection, trans.Transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Created_By", ent.Modified_By));
                        cmd.Parameters.Add(new SqlParameter("@Created_Date", ent.Modified_Date));
                        cmd.Parameters.Add(new SqlParameter("@Primary_Id", ent.Brand_ID));
                        cmd.Parameters.Add(new SqlParameter("@Log_Action", "Delete Brand"));
                        cmd.Parameters.Add(new SqlParameter("@Log_Status", "Admin"));
                        try
                        {
                            dataresult1 = Convert.ToInt32(cmd.ExecuteScalar());
                            if (dataresult1 > 0)
                            {
                                cmd.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            dataresult1 = -2;
                        }
                    }
                }
            }
            catch (Exception)
            {
                dataResult = -1;
            }
            finally
            {
                con.Close();
            }
            return dataResult;
        }
        #endregion

        #region User
        public int SaveUser(Ent_User ent, SafeTransaction trans)
        {
            int dataresult = 0;
            int dataresult1 = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (SqlCommand cmd = new SqlCommand("EC_InsertUser", trans.DatabaseConnection, trans.Transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_ID", ent.User_ID));
                    cmd.Parameters.Add(new SqlParameter("@User_Name", ent.User_Name));
                    cmd.Parameters.Add(new SqlParameter("@User_Designation", ent.User_Designation));
                    cmd.Parameters.Add(new SqlParameter("@User_Address", ent.User_Address));
                    cmd.Parameters.Add(new SqlParameter("@User_Email", ent.User_Email));
                    cmd.Parameters.Add(new SqlParameter("@User_Phone", ent.User_Phone));
                    cmd.Parameters.Add(new SqlParameter("@User_Type", ent.User_Type));
                    cmd.Parameters.Add(new SqlParameter("@User_Username", ent.User_Username));
                    cmd.Parameters.Add(new SqlParameter("@User_Password", ent.User_Password));
                    cmd.Parameters.Add(new SqlParameter("@Created_Date", ent.Created_Date));
                    cmd.Parameters.Add(new SqlParameter("@Created_By", ent.Created_By));
                    try
                    {
                        dataresult = Convert.ToInt32(cmd.ExecuteScalar());
                        if (dataresult > 0)
                        {
                            cmd.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        dataresult = -2;
                        InsertException(ex.Message, "SaveUser", ent.User_ID);
                    }
                }

                if (dataresult > 0)
                {
                    using (SqlCommand cmd = new SqlCommand("EC_InsertLog", trans.DatabaseConnection, trans.Transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Created_By", ent.Created_By));
                        cmd.Parameters.Add(new SqlParameter("@Created_Date", ent.Created_Date));
                        cmd.Parameters.Add(new SqlParameter("@Primary_Id", dataresult));
                        if (ent.User_ID > 0)
                            cmd.Parameters.Add(new SqlParameter("@Log_Action", "Update User"));
                        else
                            cmd.Parameters.Add(new SqlParameter("@Log_Action", "Insert User"));
                        cmd.Parameters.Add(new SqlParameter("@Log_Status", "Admin"));
                        try
                        {
                            dataresult1 = Convert.ToInt32(cmd.ExecuteScalar());
                            if (dataresult1 > 0)
                            {
                                cmd.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            dataresult1 = -2;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                dataresult = -2;
            }
            finally { con.Close(); }
            return dataresult;
        }

        public List<Ent_User> SelectUserList(int user_id)
        {
            List<Ent_User> list = new List<Ent_User>();
            Ent_User ent = new Ent_User();
            try
            {
                using (SqlCommand cmd = new SqlCommand("EC_SelectUser", con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_ID", user_id));
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ent = new Ent_User();
                        ent.User_ID = Convert.ToInt32(dr["User_ID"]);
                        ent.User_Name = Convert.ToString(dr["User_Name"]);
                        ent.User_Type = Convert.ToString(dr["User_Type"]);
                        ent.User_Designation = Convert.ToString(dr["User_Designation"]);
                        ent.User_Username = Convert.ToString(dr["User_Username"]);
                        ent.User_Password = Convert.ToString(dr["User_Password"]);
                        ent.User_Email = Convert.ToString(dr["User_Email"]);
                        ent.User_Phone = Convert.ToString(dr["User_Phone"]);
                        list.Add(ent);
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                InsertException(e.Message, "SelectUserList", user_id);
            }
            finally
            {
                con.Close();
            }
            return list;
        }

        public Ent_User SelectUser(int user_id)
        {
            Ent_User ent = new Ent_User();
            try
            {
                using (SqlCommand cmd = new SqlCommand("EC_SelectUser", con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_ID", user_id));
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ent = new Ent_User();
                        ent.User_ID = Convert.ToInt32(dr["User_ID"]);
                        ent.User_Name = Convert.ToString(dr["User_Name"]);
                        ent.User_Address = Convert.ToString(dr["User_Address"]);
                        ent.User_Designation = Convert.ToString(dr["User_Designation"]);
                        ent.User_Type = Convert.ToString(dr["User_Type"]);
                        ent.User_Username = Convert.ToString(dr["User_Username"]);
                        ent.User_Password = Convert.ToString(dr["User_Password"]);
                        ent.User_Email = Convert.ToString(dr["User_Email"]);
                        ent.User_Phone = Convert.ToString(dr["User_Phone"]);
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                InsertException(e.Message, "SelectUser", user_id);
            }
            finally
            {
                con.Close();
            }
            return ent;
        }

        public int DeleteUser(Ent_User ent, SafeTransaction trans)
        {
            int dataResult = 0;
            int dataresult1 = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (SqlCommand cmd = new SqlCommand("EC_DeleteUser", trans.DatabaseConnection, trans.Transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_ID", ent.User_ID));
                    cmd.Parameters.Add(new SqlParameter("@Modified_By", ent.Modified_By));
                    cmd.Parameters.Add(new SqlParameter("@Modified_Date", ent.Modified_Date));
                    try
                    {
                        dataResult = Convert.ToInt32(cmd.ExecuteScalar());
                        cmd.Dispose();
                    }
                    catch (Exception e)
                    {
                        InsertException(e.Message, "DeleteUser", ent.User_ID);
                        dataResult = -1;
                    }
                }

                if (dataResult > 0)
                {
                    using (SqlCommand cmd = new SqlCommand("EC_InsertLog", trans.DatabaseConnection, trans.Transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Created_By", ent.Modified_By));
                        cmd.Parameters.Add(new SqlParameter("@Created_Date", ent.Modified_Date));
                        cmd.Parameters.Add(new SqlParameter("@Primary_Id", ent.User_ID));
                        cmd.Parameters.Add(new SqlParameter("@Log_Action", "Delete User"));
                        cmd.Parameters.Add(new SqlParameter("@Log_Status", "Admin"));
                        try
                        {
                            dataresult1 = Convert.ToInt32(cmd.ExecuteScalar());
                            if (dataresult1 > 0)
                            {
                                cmd.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            dataresult1 = -2;
                        }
                    }
                }
            }
            catch (Exception)
            {
                dataResult = -1;
            }
            finally
            {
                con.Close();
            }
            return dataResult;
        }
        #endregion

        public void InsertException(string exception, string from, int id)
        {
            using (SqlCommand cmd = new SqlCommand("EC_InsertException", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Exception", exception));
                cmd.Parameters.Add(new SqlParameter("@ExceptionFrom", from));
                cmd.Parameters.Add(new SqlParameter("@ExceptionID", id));
                cmd.Parameters.Add(new SqlParameter("@CreatedDate", DateTime.Now));
                try
                {
                    int r = Convert.ToInt32(cmd.ExecuteScalar());
                    if (r > 0)
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
                catch(Exception e)
                {
                    cmd.Dispose();
                }
            }
        }

        #region Login
        public List<Ent_User> SelectLogin(Ent_User entu)
        {
            List<Ent_User> result = new List<Ent_User>();
            Ent_User ent = new Ent_User();
            try
            {
                using (SqlCommand cmd = new SqlCommand("select User_ID,User_Name,User_Type from EC_Users where User_Username='" + entu.User_Username + "' and User_Password='" + entu.User_Password + "' and Is_Active=1", con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ent = new Ent_User();
                        ent.User_ID = Convert.ToInt32(dr["User_ID"]);                     
                        ent.User_Name = Convert.ToString(dr["User_Name"]);
                        ent.User_Type = Convert.ToString(dr["User_Type"]);
                        result.Add(ent);
                    }
                }
            }
            catch (Exception Ex)
            {
                InsertException(Ex.Message, "SelectLogin", ent.User_ID);         
            }
            return result;
        }
        #endregion

        public DataTable SelectDashboardData()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlCommand cmd = new SqlCommand("EC_SelectDashboardData", con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                InsertException(e.Message, "SelectDashboardData", 0);
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
    }
}