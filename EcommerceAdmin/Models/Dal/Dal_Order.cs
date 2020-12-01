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

        public int InsertCart(Ent_OrderDetail ent, SafeTransaction trans)
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

        public int InsertCartList(List<Ent_OrderDetail> ent, int guestID, SafeTransaction trans)
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

        public List<Ent_OrderDetail> SelectCart(int guestID)
        {
            List<Ent_OrderDetail> result = new List<Ent_OrderDetail>();
            Ent_OrderDetail ent = new Ent_OrderDetail();
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
                        ent = new Ent_OrderDetail();
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

                    cmd.Parameters.Add(new SqlParameter("@Shipping_FirstName", ent.Shipping_FirstName));
                    cmd.Parameters.Add(new SqlParameter("@Shipping_LastName", ent.Shipping_LastName));
                    cmd.Parameters.Add(new SqlParameter("@Shipping_Address1", ent.Shipping_Address1));
                    cmd.Parameters.Add(new SqlParameter("@Shipping_Address2", ent.Shipping_Address2));
                    cmd.Parameters.Add(new SqlParameter("@Shipping_Town", ent.Shipping_Town));
                    cmd.Parameters.Add(new SqlParameter("@Shipping_State", ent.Shipping_State));
                    cmd.Parameters.Add(new SqlParameter("@Shipping_Country", ent.Shipping_Country));
                    cmd.Parameters.Add(new SqlParameter("@Shipping_Email", ent.Shipping_Email));
                    cmd.Parameters.Add(new SqlParameter("@Shipping_Phone", ent.Shipping_Phone));

                    cmd.Parameters.Add(new SqlParameter("@Billing_FirstName", ent.Billing_FirstName));
                    cmd.Parameters.Add(new SqlParameter("@Billing_LastName", ent.Billing_LastName));
                    cmd.Parameters.Add(new SqlParameter("@Billing_Address1", ent.Billing_Address1));
                    cmd.Parameters.Add(new SqlParameter("@Billing_Address2", ent.Billing_Address2));
                    cmd.Parameters.Add(new SqlParameter("@Billing_Town", ent.Billing_Town));
                    cmd.Parameters.Add(new SqlParameter("@Billing_State", ent.Billing_State));
                    cmd.Parameters.Add(new SqlParameter("@Billing_Country", ent.Billing_Country));
                    cmd.Parameters.Add(new SqlParameter("@Billing_Email", ent.Billing_Email));
                    cmd.Parameters.Add(new SqlParameter("@Billing_Phone", ent.Billing_Phone));

                    cmd.Parameters.Add(new SqlParameter("@Order_SubTotal", ent.Order_SubTotal));
                    cmd.Parameters.Add(new SqlParameter("@Order_Shipping", ent.Order_Shipping));
                    cmd.Parameters.Add(new SqlParameter("@Order_Total", ent.Order_Total));
                    cmd.Parameters.Add(new SqlParameter("@Created_Date", ent.Created_Date));
                    cmd.Parameters.Add(new SqlParameter("@Payment_COD", ent.Payment_COD));
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
                                        try
                                        {
                                            dataresult1 = Convert.ToInt32(cmd1.ExecuteScalar());
                                            cmd1.Dispose();
                                            
                                        }
                                        catch (Exception ex)
                                        {
                                            dataresult = 0;
                                            InsertException(ex.Message, "SaveOrderDetail", ent.OrderDetailsList[i].OrderDetail_ID);
                                        }
                                    }
                                }

                                //if (dataresult1 > 0)
                                //{
                                //    if (ent.Guest_ID != 0)
                                //    {
                                //        var query = "delete from EC_Cart where Guest_ID=" + ent.Guest_ID;
                                //        int k = 0;
                                //        using (SqlCommand cmd2 = new SqlCommand(query, trans.DatabaseConnection, trans.Transaction))
                                //        {
                                //            try
                                //            {
                                //                k = cmd2.ExecuteNonQuery();
                                //                cmd2.Dispose();

                                //            }
                                //            catch (Exception ex)
                                //            {
                                //                dataresult = 0;
                                //            }
                                //        }
                                //    }                                    
                                //}
                            }
                            else
                            {
                                dataresult = 0;
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

        public int UpdatePayment(Ent_Order entOrder , SafeTransaction trans)
        {
            int dataresult = 0;
            int dataresult1 = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (SqlCommand cmd = new SqlCommand("EC_UpdateOrder", trans.DatabaseConnection, trans.Transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Order_ID", entOrder.Order_ID));                  
                    cmd.Parameters.Add(new SqlParameter("@Payment_Status", entOrder.Payment_Status));                  
                    cmd.Parameters.Add(new SqlParameter("@Transaction_Id", entOrder.Transaction_Id));                  
                    cmd.Parameters.Add(new SqlParameter("@Payment_Method", entOrder.Payment_Method));                  
                    try
                    {
                        dataresult = Convert.ToInt32(cmd.ExecuteScalar());
                        if (dataresult > 0)
                        {
                            cmd.Dispose();
                            dataresult1 = 1;                                    

                            if (dataresult1 > 0)
                            {
                                if (entOrder.Guest_ID != 0 && entOrder.Payment_Status == "CAPTURED")
                                {
                                    var query = "delete from EC_Cart where Guest_ID=" + entOrder.Guest_ID;
                                    int k = 0;
                                    using (SqlCommand cmd2 = new SqlCommand(query, trans.DatabaseConnection, trans.Transaction))
                                    {
                                        try
                                        {
                                            k = cmd2.ExecuteNonQuery();
                                            cmd2.Dispose();

                                        }
                                        catch (Exception ex)
                                        {
                                            dataresult = 0;
                                        }
                                    }
                                }
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

        public List<Ent_Order> SelectGuestOrder(int guestID )
        {
            List<Ent_Order> result = new List<Ent_Order>();
            Ent_Order ent = new Ent_Order();
            int orderid = 0;
            try
            {
                using (SqlCommand cmd = new SqlCommand("EC_SelectGuestOrder", con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Guest_ID", guestID));
                    cmd.Parameters.Add(new SqlParameter("@Order_ID", orderid));
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ent = new Ent_Order();
                        ent.Order_ID = Convert.ToInt32(dr["Order_ID"]);
                        ent.Created_Date = Convert.ToDateTime(dr["Created_Date"]);
                        ent.Is_Active = Convert.ToInt32(dr["Is_Active"]);                     
                        ent.Order_Total = Convert.ToDouble(dr["Order_Total"]);    
                                    ent.Order_SubTotal = Convert.ToDouble(dr["Order_SubTotal"]);
                                    ent.Order_Shipping = Convert.ToDouble(dr["Order_Shipping"]);
                        ent.Total_Qty = Convert.ToInt32(dr["Total_Qty"]);          
                        
                        ent.Billing_FirstName = Convert.ToString(dr["Billing_FirstName"]);                     
                        ent.Billing_LastName = Convert.ToString(dr["Billing_LastName"]);
                        ent.Billing_Address1 = Convert.ToString(dr["Billing_Address1"]);
                        ent.Billing_Address2 = Convert.ToString(dr["Billing_Address2"]);
                        ent.Billing_Town = Convert.ToString(dr["Billing_Town"]);
                        ent.Billing_State = Convert.ToString(dr["Billing_State"]);
                        ent.Billing_Country = Convert.ToString(dr["Billing_Country"]);
                        ent.Billing_Email = Convert.ToString(dr["Billing_Email"]);                     
                        ent.Billing_Phone = Convert.ToString(dr["Billing_Phone"]);

                        ent.Shipping_FirstName = Convert.ToString(dr["Shipping_FirstName"]);
                        ent.Shipping_LastName = Convert.ToString(dr["Shipping_LastName"]);
                        ent.Shipping_Address1 = Convert.ToString(dr["Shipping_Address1"]);
                        ent.Shipping_Address2 = Convert.ToString(dr["Shipping_Address2"]);
                        ent.Shipping_Town = Convert.ToString(dr["Shipping_Town"]);
                        ent.Shipping_State = Convert.ToString(dr["Shipping_State"]);
                        ent.Shipping_Country = Convert.ToString(dr["Shipping_Country"]);
                        ent.Shipping_Email = Convert.ToString(dr["Shipping_Email"]);
                        ent.Shipping_Phone = Convert.ToString(dr["Shipping_Phone"]);

                        ent.Created_Date = Convert.ToDateTime(dr["Created_Date"]);                     
                        result.Add(ent);
                    }
                }
            }
            catch (Exception ex)
            {
                InsertException(ex.Message, "SelectGuestOrder", guestID);
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public Ent_Order SelectOrder( int OrderID)
        {          
            Ent_Order ent = new Ent_Order();
            int guestid = 0;
            try
            {
                using (SqlCommand cmd = new SqlCommand("EC_SelectGuestOrder", con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Guest_ID", guestid));
                    cmd.Parameters.Add(new SqlParameter("@Order_ID", OrderID));
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ent = new Ent_Order();
                        ent.Order_ID = Convert.ToInt32(dr["Order_ID"]);
                        ent.Guest_ID = Convert.ToInt32(dr["Guest_ID"]);
                        ent.Created_Date = Convert.ToDateTime(dr["Created_Date"]);
                        ent.Is_Active = Convert.ToInt32(dr["Is_Active"]);
                        ent.Order_Total = Convert.ToDouble(dr["Order_Total"]);
                        ent.Order_SubTotal = Convert.ToDouble(dr["Order_SubTotal"]);
                        ent.Order_Shipping = Convert.ToDouble(dr["Order_Shipping"]);
                        ent.Total_Qty = Convert.ToInt32(dr["Total_Qty"]);
                        ent.Is_Active = Convert.ToInt32(dr["Is_Active"]);
                        if (dr["Received_Date"].ToString() == "")
                            ent.Received_Date = null;
                        else
                            ent.Received_Date = dr["Received_Date"].ToString();

                        if (dr["Shipped_Date"].ToString() == "")
                            ent.Shipped_Date = null;
                        else
                           ent.Shipped_Date =  dr["Shipped_Date"].ToString();

                        if (dr["Delivered_Date"].ToString() == "")
                            ent.Delivered_Date = null;
                        else
                            ent.Delivered_Date = dr["Delivered_Date"].ToString();

                        if (dr["Cancel_Date"].ToString() == "")
                            ent.Cancel_Date = null;
                        else
                            ent.Cancel_Date = dr["Cancel_Date"].ToString();

                        if (dr["Return_Date"].ToString() == "")
                            ent.Return_Date = null;
                        else
                            ent.Return_Date = dr["Return_Date"].ToString();                                      

                        ent.Billing_FirstName = Convert.ToString(dr["Billing_FirstName"]);
                        ent.Billing_LastName = Convert.ToString(dr["Billing_LastName"]);
                        ent.Billing_Address1 = Convert.ToString(dr["Billing_Address1"]);
                        ent.Billing_Address2 = Convert.ToString(dr["Billing_Address2"]);
                        ent.Billing_Town = Convert.ToString(dr["Billing_Town"]);
                        ent.Billing_State = Convert.ToString(dr["Billing_State"]);
                        ent.Billing_Country = Convert.ToString(dr["Billing_Country"]);
                        ent.Billing_Email = Convert.ToString(dr["Billing_Email"]);
                        ent.Billing_Phone = Convert.ToString(dr["Billing_Phone"]);

                        ent.Shipping_FirstName = Convert.ToString(dr["Shipping_FirstName"]);
                        ent.Shipping_LastName = Convert.ToString(dr["Shipping_LastName"]);
                        ent.Shipping_Address1 = Convert.ToString(dr["Shipping_Address1"]);
                        ent.Shipping_Address2 = Convert.ToString(dr["Shipping_Address2"]);
                        ent.Shipping_Town = Convert.ToString(dr["Shipping_Town"]);
                        ent.Shipping_State = Convert.ToString(dr["Shipping_State"]);
                        ent.Shipping_Country = Convert.ToString(dr["Shipping_Country"]);
                        ent.Shipping_Email = Convert.ToString(dr["Shipping_Email"]);
                        ent.Shipping_Phone = Convert.ToString(dr["Shipping_Phone"]);
                        ent.Payment_COD = Convert.ToInt32(dr["Payment_COD"]);
                        
                        ent.Created_Date = Convert.ToDateTime(dr["Created_Date"]);
                    }
                }
            }
            catch (Exception ex)
            {
                InsertException(ex.Message, "SelectOrder", OrderID);
            }
            finally
            {
                con.Close();
            }
            return ent;
        }

        public int UpdateNotification(int flag)
        {
            int dataResult = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = "";
                if (flag==0)
                 query = "update EC_GuestLogin set Notification_Status=0 ";
                else
                    query = "update EC_Order set Notification_Status=0 ";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    try
                    {
                        dataResult = cmd.ExecuteNonQuery();
                        if (dataResult > 0)
                        {
                            cmd.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        dataResult = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                dataResult = 0;
            }
            finally
            { con.Close(); }
            return dataResult;
        }

        public List<Ent_OrderDetail> SelectOrderDetails(int OrderId)
        {
            List<Ent_OrderDetail> result = new List<Ent_OrderDetail>();
            Ent_OrderDetail ent = new Ent_OrderDetail();
            try
            {
                using (SqlCommand cmd = new SqlCommand("EC_SelectOrderDetails", con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Order_ID", OrderId));
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ent = new Ent_OrderDetail();
                        ent.OrderDetail_ID = Convert.ToInt32(dr["OrderDetail_ID"]);
                        ent.Order_ID = Convert.ToInt32(dr["Order_ID"]);
                        ent.Product_ID = Convert.ToInt32(dr["Product_ID"]);
                        ent.Product_Total = float.Parse(dr["Product_Total"].ToString());
                        ent.Product_Price = float.Parse(dr["Product_Price"].ToString());
                        ent.Quantity = Convert.ToInt32(dr["Quantity"]);
                        ent.Product_Image = Convert.ToString(dr["Product_Image"]);
                        ent.Product_Name = Convert.ToString(dr["Product_Name"]);                      
                        ent.entOrder.Order_Shipping = float.Parse(dr["Order_Shipping"].ToString());
                        ent.entOrder.Order_Total = float.Parse(dr["Order_Total"].ToString());
                        ent.entOrder.Order_SubTotal = float.Parse(dr["Order_SubTotal"].ToString());
                        ent.entOrder.Is_Active = Convert.ToInt32(dr["Is_Active"]);
                        if (dr["Received_Date"].ToString() == "")
                            ent.entOrder.Received_Date = null;
                        else
                            ent.entOrder.Received_Date = dr["Received_Date"].ToString();

                        if (dr["Shipped_Date"].ToString() == "")
                            ent.entOrder.Shipped_Date = null;
                        else
                            ent.entOrder.Shipped_Date = dr["Shipped_Date"].ToString();

                        if (dr["Delivered_Date"].ToString() == "")
                            ent.entOrder.Delivered_Date = null;
                        else
                            ent.entOrder.Delivered_Date = dr["Delivered_Date"].ToString();

                        if (dr["Cancel_Date"].ToString() == "")
                            ent.entOrder.Cancel_Date = null;
                        else
                            ent.entOrder.Cancel_Date = dr["Cancel_Date"].ToString();

                        if (dr["Return_Date"].ToString() == "")
                            ent.entOrder.Return_Date = null;
                        else
                            ent.entOrder.Return_Date = dr["Return_Date"].ToString();
                        result.Add(ent);
                    }
                }
            }
            catch (Exception ex)
            {
                InsertException(ex.Message, "SelectOrderDetails", OrderId);
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
                catch (Exception e)
                {
                    cmd.Dispose();
                }
            }
        }

        public int UpdateOrderStatus(Ent_Order ent,SafeTransaction trans)
        {
            int dataresult = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (SqlCommand cmd = new SqlCommand("EC_UpdateOrderStatus", trans.DatabaseConnection, trans.Transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Created_Date", ent.Created_Date));
                    cmd.Parameters.Add(new SqlParameter("@status", ent.Is_Active));
                    cmd.Parameters.Add(new SqlParameter("@Order_ID", ent.Order_ID));
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
    }
}