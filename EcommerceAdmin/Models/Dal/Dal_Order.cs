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
    public class Dal_Order
    {
        SqlConnection con = null;
        DBConnection dbcon = new DBConnection();
        public Dal_Order()
        {
            con = dbcon.DatabaseConnection;
        }

        public int InsertCart(Ent_Product ent, SafeTransaction trans)
        {
            int dataresult = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (SqlCommand cmd = new SqlCommand("EC_InsertCart", trans.DatabaseConnection, trans.Transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Cart_ID", ent.Cart_ID));
                    cmd.Parameters.Add(new SqlParameter("@Product_ID", ent.Product_ID));
                    cmd.Parameters.Add(new SqlParameter("@Quantity", ent.Quantity));
                    cmd.Parameters.Add(new SqlParameter("@Guest_ID", ent.Guest_ID));
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

        public int InsertCartList(List<Ent_Product> ent, int guestID, SafeTransaction trans)
        {
            int dataresult = 0;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                for (int k = 0; k < ent.Count; k++)
                {
                    using (SqlCommand cmd = new SqlCommand("EC_InsertCart", trans.DatabaseConnection, trans.Transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Cart_ID", ent[k].Cart_ID));
                        cmd.Parameters.Add(new SqlParameter("@Product_ID", ent[k].Product_ID));
                        cmd.Parameters.Add(new SqlParameter("@Quantity", ent[k].Quantity));
                        cmd.Parameters.Add(new SqlParameter("@Guest_ID", guestID));
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

        public int DeleteCart(int productId, int GuestID, SafeTransaction trans)
        {
            int dataResult = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (SqlCommand cmd = new SqlCommand("EC_DeleteCart", trans.DatabaseConnection, trans.Transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Product_ID", productId));
                    cmd.Parameters.Add(new SqlParameter("@Guest_ID", GuestID));
                    try
                    {
                        dataResult = Convert.ToInt32(cmd.ExecuteScalar());
                        cmd.Dispose();
                    }
                    catch (Exception e)
                    {
                        InsertException(e.Message, "DeleteCart", GuestID);
                        dataResult = -1;
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

        public List<Ent_Product> SelectCart(int guestID)
        {
            List<Ent_Product> result = new List<Ent_Product>();
            Ent_Product ent = new Ent_Product();
            try
            {
                using (SqlCommand cmd = new SqlCommand("EC_SelectCart", con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Guest_ID", guestID));
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ent = new Ent_Product();
                        ent.Product_ID = Convert.ToInt32(dr["Product_ID"]);
                        ent.Product_Name = Convert.ToString(dr["Product_Name"]);
                        ent.Quantity = Convert.ToInt32(dr["Quantity"]);
                        ent.Product_Price = float.Parse(dr["Product_Price"].ToString());
                        ent.Product_Total = float.Parse(dr["Product_Total"].ToString());
                        ent.Product_Image = Convert.ToString(dr["Product_Image"]);
                        result.Add(ent);
                    }
                }
            }
            catch (Exception ex)
            {
                InsertException(ex.Message, "SelectCart", guestID);
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public int SaveOrder(Ent_Order ent, SafeTransaction trans)
        {
            int dataresult = 0;
            int dataresult1 = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (SqlCommand cmd = new SqlCommand("EC_InsertOrder", trans.DatabaseConnection, trans.Transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Order_ID", ent.Order_ID));
                    cmd.Parameters.Add(new SqlParameter("@Guest_ID", ent.Guest_ID));
                    cmd.Parameters.Add(new SqlParameter("@Guest_FirstName", ent.Guest_FirstName));
                    cmd.Parameters.Add(new SqlParameter("@Guest_LastName", ent.Guest_LastName));
                    cmd.Parameters.Add(new SqlParameter("@Guest_Address1", ent.Guest_Address1));
                    cmd.Parameters.Add(new SqlParameter("@Guest_Address2", ent.Guest_Address2));
                    cmd.Parameters.Add(new SqlParameter("@Guest_Town", ent.Guest_Town));
                    cmd.Parameters.Add(new SqlParameter("@Guest_State", ent.Guest_State));
                    cmd.Parameters.Add(new SqlParameter("@Guest_Country", ent.Guest_Country));
                    cmd.Parameters.Add(new SqlParameter("@Guest_Email", ent.Guest_Email));
                    cmd.Parameters.Add(new SqlParameter("@Guest_Phone", ent.Guest_Phone));
                    cmd.Parameters.Add(new SqlParameter("@Order_SubTotal", ent.Order_SubTotal));
                    cmd.Parameters.Add(new SqlParameter("@Order_Shipping", ent.Order_Shipping));
                    cmd.Parameters.Add(new SqlParameter("@Order_Total", ent.Order_Total));
                    cmd.Parameters.Add(new SqlParameter("@Created_Date", ent.Created_Date));
                    try
                    {
                        dataresult = Convert.ToInt32(cmd.ExecuteScalar());
                        if (dataresult > 0)
                        {
                            cmd.Dispose();
                            if (ent.OrderDetailsList != null)
                            {
                                for (int i = 0; i < ent.OrderDetailsList.Count; i++)
                                {
                                    using (SqlCommand cmd1 = new SqlCommand("EC_InsertOrderDetails", trans.DatabaseConnection, trans.Transaction))
                                    {
                                        cmd1.CommandType = CommandType.StoredProcedure;
                                        cmd1.Parameters.Add(new SqlParameter("@OrderDetail_ID", ent.OrderDetailsList[i].OrderDetail_ID));
                                        cmd1.Parameters.Add(new SqlParameter("@Order_ID", dataresult));
                                        cmd1.Parameters.Add(new SqlParameter("@Product_ID", ent.OrderDetailsList[i].Product_ID));
                                        cmd1.Parameters.Add(new SqlParameter("@Quantity", ent.OrderDetailsList[i].Quantity));
                                        cmd1.Parameters.Add(new SqlParameter("@Product_Price", ent.OrderDetailsList[i].Product_Price));
                                        cmd1.Parameters.Add(new SqlParameter("@Product_Total", ent.OrderDetailsList[i].Product_Total));
                                        cmd1.Parameters.Add(new SqlParameter("@Created_Date", ent.Created_Date));
                                        try
                                        {
                                            dataresult1 = Convert.ToInt32(cmd1.ExecuteScalar());
                                            cmd1.Dispose();
                                        }
                                        catch (Exception ex)
                                        {
                                            dataresult1 = -1;
                                            InsertException(ex.Message, "SaveOrderDetail", ent.OrderDetailsList[i].OrderDetail_ID);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                dataresult1 = 1;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        dataresult = -2;
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
    }
}