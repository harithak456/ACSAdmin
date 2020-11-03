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
    public class Dal_Product
    {
        SqlConnection con = null;
        DBConnection dbcon = new DBConnection();
        public Dal_Product()
        {
            con = dbcon.DatabaseConnection;
        }

        public int SaveProduct(Ent_Product ent, SafeTransaction trans)
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
                using (SqlCommand cmd = new SqlCommand("EC_InsertProduct", trans.DatabaseConnection, trans.Transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Product_ID", ent.Product_ID));
                    cmd.Parameters.Add(new SqlParameter("@Product_Name", ent.Product_Name));
                    cmd.Parameters.Add(new SqlParameter("@Product_SubText", ent.Product_SubText));
                    cmd.Parameters.Add(new SqlParameter("@Category_ID", ent.Category_ID));
                    cmd.Parameters.Add(new SqlParameter("@SubCategory_ID", ent.SubCategory_ID));
                    cmd.Parameters.Add(new SqlParameter("@Brand_ID", ent.Brand_ID));
                    cmd.Parameters.Add(new SqlParameter("@Product_Description", ent.Product_Description));
                    cmd.Parameters.Add(new SqlParameter("@Product_Price", ent.Product_Price));
                    cmd.Parameters.Add(new SqlParameter("@Product_Discount", ent.Product_Discount));
                    cmd.Parameters.Add(new SqlParameter("@Product_Image", ent.Product_Image));
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
                        InsertException(ex.Message, "SaveProduct", ent.Product_ID);
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
                        if (ent.Product_ID > 0)
                            cmd.Parameters.Add(new SqlParameter("@Log_Action", "Update Product"));
                        else
                            cmd.Parameters.Add(new SqlParameter("@Log_Action", "Insert Product"));
                        
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
                catch (Exception e)
                {
                    cmd.Dispose();
                }
            }
        }

        //Select All ProductList
        public List<Ent_Product> SelectProductList(int productId)
        {
            List<Ent_Product> result = new List<Ent_Product>();
            Ent_Product ent = new Ent_Product();
            try
            {
                using (SqlCommand cmd = new SqlCommand("EC_SelectProduct", con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Product_ID", productId));
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ent = new Ent_Product();
                        ent.Product_ID = Convert.ToInt32(dr["Product_ID"]);
                        ent.Product_Name = Convert.ToString(dr["Product_Name"]);
                        ent.Product_SubText = Convert.ToString(dr["Product_SubText"]);
                        ent.Category_ID = Convert.ToInt32(dr["Category_ID"]);
                        ent.entCategory.Category_Name = Convert.ToString(dr["Category_Name"]);
                        ent.SubCategory_ID = Convert.ToInt32(dr["SubCategory_ID"]);
                        ent.Brand_ID = Convert.ToInt32(dr["Brand_ID"]);
                        ent.Product_Description = Convert.ToString(dr["Product_Description"]);
                        ent.Product_Price = float.Parse(dr["Product_Price"].ToString());
                        ent.Product_Discount = float.Parse(dr["Product_Discount"].ToString());
                        ent.Product_Image = Convert.ToString(dr["Product_Image"]);
                        result.Add(ent);
                    }
                }
            }
            catch (Exception ex)
            {
                InsertException(ex.Message, "SelectProductList", productId);
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        //Select  Product by ID
        public Ent_Product SelectProduct(int productId)
        {       
            Ent_Product ent = new Ent_Product();
            try
            {
                using (SqlCommand cmd = new SqlCommand("EC_SelectProduct", con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Product_ID", productId));
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {                      
                        ent.Product_ID = Convert.ToInt32(dr["Product_ID"]);
                        ent.Product_Name = Convert.ToString(dr["Product_Name"]);
                        ent.Product_SubText = Convert.ToString(dr["Product_SubText"]);
                        ent.Category_ID = Convert.ToInt32(dr["Category_ID"]);
                        ent.entCategory.Category_Name = Convert.ToString(dr["Category_Name"]);
                        ent.SubCategory_ID = Convert.ToInt32(dr["SubCategory_ID"]);
                        ent.Brand_ID = Convert.ToInt32(dr["Brand_ID"]);
                        ent.Product_Description = Convert.ToString(dr["Product_Description"]);
                        ent.Product_Price = float.Parse(dr["Product_Price"].ToString());
                        ent.Product_Discount = float.Parse(dr["Product_Discount"].ToString());
                        ent.Product_Image = Convert.ToString(dr["Product_Image"]);                     
                    }
                }
            }
            catch (Exception ex)
            {
                InsertException(ex.Message, "SelectProduct", productId);
            }
            finally
            {
                con.Close();
            }
            return ent;
        }

        public List<Ent_Product> SelectProductFilter(int Category_ID,int SubCategory_ID,string condition)
        {
            List<Ent_Product> result = new List<Ent_Product>();
            Ent_Product ent = new Ent_Product();
            try
            {
                using (SqlCommand cmd = new SqlCommand("EC_SelectProductFilter", con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Category_ID", Category_ID));
                    cmd.Parameters.Add(new SqlParameter("@SubCategory_ID", SubCategory_ID));
                    cmd.Parameters.Add(new SqlParameter("@condition", condition));
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ent = new Ent_Product();
                        ent.Product_ID = Convert.ToInt32(dr["Product_ID"]);
                        ent.Product_Name = Convert.ToString(dr["Product_Name"]);
                        ent.Product_SubText = Convert.ToString(dr["Product_SubText"]);
                        ent.Category_ID = Convert.ToInt32(dr["Category_ID"]);
                        ent.entCategory.Category_Name = Convert.ToString(dr["Category_Name"]);
                        ent.SubCategory_ID = Convert.ToInt32(dr["SubCategory_ID"]);
                        ent.entSubCategory.Category_Name = Convert.ToString(dr["SubCategory_Name"]);
                        ent.Brand_ID = Convert.ToInt32(dr["Brand_ID"]);
                        ent.entBrand.Brand_Name = Convert.ToString(dr["Brand_Name"]);
                        ent.Product_Description = Convert.ToString(dr["Product_Description"]);
                        ent.Product_Price = float.Parse(dr["Product_Price"].ToString());
                        ent.Product_Discount = float.Parse(dr["Product_Discount"].ToString());
                        ent.Product_Image = Convert.ToString(dr["Product_Image"]);
                        result.Add(ent);
                    }
                }
            }
            catch (Exception ex)
            {
                InsertException(ex.Message, "SelectProductList", Category_ID);
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public int DeleteProduct(Ent_Product ent, SafeTransaction trans)
        {
            int dataResult = 0;
            int dataresult1 = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (SqlCommand cmd = new SqlCommand("EC_DeleteProduct", trans.DatabaseConnection, trans.Transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Product_ID", ent.Product_ID));
                    cmd.Parameters.Add(new SqlParameter("@Modified_By", ent.Modified_By));
                    cmd.Parameters.Add(new SqlParameter("@Modified_Date", ent.Modified_Date));
                    try
                    {
                        dataResult = Convert.ToInt32(cmd.ExecuteScalar());
                        cmd.Dispose();
                    }
                    catch (Exception e)
                    {
                        InsertException(e.Message, "DeleteProduct", ent.Product_ID);
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
                        cmd.Parameters.Add(new SqlParameter("@Primary_Id", ent.Product_ID));
                        cmd.Parameters.Add(new SqlParameter("@Log_Action", "Delete Product"));

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