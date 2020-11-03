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
    public class Dal_Category
    {
        SqlConnection con = null;
        DBConnection dbcon = new DBConnection();
        public Dal_Category()
        {
            con = dbcon.DatabaseConnection;
        }

        public int SaveCategory(Ent_Category ent, SafeTransaction trans)
        {
            int dataresult = 0;
            int dataresult1 = 0;
            int dataresult2 = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                //Insert To category Main Table
                using (SqlCommand cmd = new SqlCommand("EC_InsertCategory", trans.DatabaseConnection, trans.Transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Category_ID", ent.Category_ID));
                    cmd.Parameters.Add(new SqlParameter("@Category_Name", ent.Category_Name));             
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
                        InsertException(ex.Message, "SaveCategory", ent.Category_ID);                      
                    }
                }



                //Insert To Sub Category Table
                if (dataresult > 0)
                {
                    var query = "update EC_Category set Is_Active=0 where Parent_Category=" + dataresult;
                    int k = 0;
                    using (SqlCommand cmd2 = new SqlCommand(query, trans.DatabaseConnection, trans.Transaction))
                    {
                        k = cmd2.ExecuteNonQuery();
                    }

                    if (ent.SubCategoryList != null)
                    {
                        for (int i = 0; i < ent.SubCategoryList.Count; i++)
                        {
                            using (SqlCommand cmd1 = new SqlCommand("EC_InsertSubCategory", trans.DatabaseConnection, trans.Transaction))
                            {
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.Add(new SqlParameter("@Category_ID", ent.SubCategoryList[i].Category_ID));
                                cmd1.Parameters.Add(new SqlParameter("@Category_Name", ent.SubCategoryList[i].Category_Name));
                                cmd1.Parameters.Add(new SqlParameter("@Parent_Category", dataresult));
                                cmd1.Parameters.Add(new SqlParameter("@Created_Date", ent.Created_Date));
                                cmd1.Parameters.Add(new SqlParameter("@Created_By", ent.Created_By));
                                try
                                {
                                    dataresult1 = Convert.ToInt32(cmd1.ExecuteScalar());
                                    cmd1.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    //General gl = new General();
                                    //gl.UpdateErrorLog(Ex, "CreateDietPlan");
                                    dataresult1 = -1;
                                    InsertException(ex.Message, "SaveSubCategory", ent.Category_ID);
                                }
                            }
                        }
                    }
                    else
                    {
                        dataresult1 = 1;
                    }
                }
                

                if (dataresult1 > 0)
                {                   
                    using (SqlCommand cmd = new SqlCommand("EC_InsertLog", trans.DatabaseConnection, trans.Transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Created_By", ent.Created_By));
                        cmd.Parameters.Add(new SqlParameter("@Created_Date", ent.Created_Date));
                        cmd.Parameters.Add(new SqlParameter("@Primary_Id", dataresult));
                        if (ent.Category_ID > 0)
                            cmd.Parameters.Add(new SqlParameter("@Log_Action", "Update Category"));
                        else
                            cmd.Parameters.Add(new SqlParameter("@Log_Action", "Insert Category"));
                        
                        try
                        {
                            dataresult2 = Convert.ToInt32(cmd.ExecuteScalar());
                            if (dataresult2 > 0)
                            {
                                cmd.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            dataresult2 = -2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dataresult2 = -2;
            }
            finally { con.Close(); }

            if (dataresult1 <= 0)
                dataresult = -1;
            return dataresult;
        }

        //Select All Category
        public List<Ent_Category> SelectCategoryList(int categoryId,string condition)
        {
            List<Ent_Category> result = new List<Ent_Category>();
            Ent_Category ent = new Ent_Category();
            try
            {
                using (SqlCommand cmd = new SqlCommand("EC_SelectCategory", con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Category_ID", categoryId));
                    cmd.Parameters.Add(new SqlParameter("@condition", condition));
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ent = new Ent_Category();
                        ent.Category_ID = Convert.ToInt32(dr["Category_ID"]);
                        ent.Category_Name = Convert.ToString(dr["Category_Name"]);
                       
                            ent.Parent_Category = Convert.ToInt32(dr["Parent_Category"]);                                            
                        result.Add(ent);
                    }
                }
            }
            catch (Exception ex)
            {
                InsertException(ex.Message, "SelectCategoryList", categoryId);           
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        //Select BY Category Id
        public Ent_Category SelectCategory(int categoryId,string condition)
        {
            Ent_Category result = new Ent_Category();
            Ent_Category ent = new Ent_Category();
            try
            {
                using (SqlCommand cmd = new SqlCommand("EC_SelectCategory", con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Category_ID", categoryId));
                    cmd.Parameters.Add(new SqlParameter("@condition", condition));
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {                      
                        ent.Category_ID = Convert.ToInt32(dr["Category_ID"]);
                        ent.Category_Name = Convert.ToString(dr["Category_Name"]);                        
                    }
                }
            }
            catch (Exception ex)
            {
                InsertException(ex.Message, "SelectCategoryList", categoryId);
            }
            finally
            {
                con.Close();
            }
            return ent;
        }

        //Select All Sub Category
        public List<Ent_SubCategory> SelectSubCategoryList(int categoryId)
        {
            List<Ent_SubCategory> result = new List<Ent_SubCategory>();
            Ent_SubCategory ent = new Ent_SubCategory();
            try
            {
                using (SqlCommand cmd = new SqlCommand("EC_SelectSubCategory", con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Category_ID", categoryId));
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ent = new Ent_SubCategory();
                        ent.Category_ID = Convert.ToInt32(dr["Category_ID"]);
                        ent.Category_Name = Convert.ToString(dr["Category_Name"]);
                        ent.Parent_Category = Convert.ToInt32(dr["Parent_Category"]);
                        result.Add(ent);
                    }
                }
            }
            catch (Exception ex)
            {
                InsertException(ex.Message, "SelectSubCategoryList", categoryId);
            }
            finally
            {
                con.Close();
            }
            return result;
        }

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

        public int DeleteCategory(Ent_Category ent, SafeTransaction trans)
        {
            int dataResult = 0;
            int dataresult1 = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (SqlCommand cmd = new SqlCommand("EC_DeleteCategory", trans.DatabaseConnection, trans.Transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Category_ID", ent.Category_ID));
                    cmd.Parameters.Add(new SqlParameter("@Modified_By", ent.Modified_By));
                    cmd.Parameters.Add(new SqlParameter("@Modified_Date", ent.Modified_Date));
                    try
                    {
                        dataResult = Convert.ToInt32(cmd.ExecuteScalar());
                        cmd.Dispose();
                    }
                    catch (Exception e)
                    {
                        InsertException(e.Message, "DeleteCategory", ent.Category_ID);
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
                        cmd.Parameters.Add(new SqlParameter("@Primary_Id", ent.Category_ID));
                        cmd.Parameters.Add(new SqlParameter("@Log_Action", "Delete Category "));

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
    }
}