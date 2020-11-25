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
    public class Dal_Guest
    {
        SqlConnection con = null;
        DBConnection dbcon = new DBConnection();
        public Dal_Guest()
        {
            con = dbcon.DatabaseConnection;
        }

        public int SaveGuest(Ent_Guest ent,SafeTransaction trans)
        {
            int dataresult = 0;
            int dataresult1 = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (SqlCommand cmd = new SqlCommand("EC_InsertGuest", trans.DatabaseConnection, trans.Transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Guest_ID", ent.Guest_ID));
                    cmd.Parameters.Add(new SqlParameter("@Guest_FirstName", ent.Guest_FirstName));
                    cmd.Parameters.Add(new SqlParameter("@Guest_LastName", ent.Guest_LastName));
                    cmd.Parameters.Add(new SqlParameter("@Guest_Email", ent.Guest_Username));
                    cmd.Parameters.Add(new SqlParameter("@Guest_Phone", ent.Guest_Phone));
                    cmd.Parameters.Add(new SqlParameter("@Guest_Username", ent.Guest_Username));
                    cmd.Parameters.Add(new SqlParameter("@Guest_Password", ent.Guest_Password));
                    cmd.Parameters.Add(new SqlParameter("@Created_Date", ent.Created_Date));
                    cmd.Parameters.Add(new SqlParameter("@Unique_ID", ent.Unique_ID));
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
                        InsertException(ex.Message, "SaveGuest", ent.Guest_ID);
                    }
                }

                if (dataresult > 0)
                {
                    using (SqlCommand cmd = new SqlCommand("EC_InsertLog", trans.DatabaseConnection, trans.Transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Created_By", ent.Guest_ID));
                        cmd.Parameters.Add(new SqlParameter("@Created_Date", ent.Created_Date));
                        cmd.Parameters.Add(new SqlParameter("@Primary_Id", dataresult));
                        if (ent.Guest_ID > 0)
                            cmd.Parameters.Add(new SqlParameter("@Log_Action", "Save Guest"));
                        else
                            cmd.Parameters.Add(new SqlParameter("@Log_Action", "Update Guest"));
                            cmd.Parameters.Add(new SqlParameter("@Log_Status", "Guest"));
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

        public int ActivateGuest(Ent_Guest ent, string token, SafeTransaction trans)
        {
            int dataresult = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (SqlCommand cmd = new SqlCommand("EC_ActivateGuest", trans.DatabaseConnection, trans.Transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Created_Date", ent.Created_Date));
                    cmd.Parameters.Add(new SqlParameter("@token", token));
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
                        InsertException(ex.Message, "ActivateGuest", 0);
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

        public Ent_Guest SelectLogin(Ent_Guest entR)
        {
            Ent_Guest ent = new Ent_Guest();          
            try
            {
                string query= "select * from EC_GuestLogin  where Guest_Username='"+ entR.Guest_Username  + "' and Guest_Password= '" + entR.Guest_Password + "'  and Is_Active=1";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }                  
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ent.Guest_ID = Convert.ToInt32(dr["Guest_ID"]);
                        ent.Guest_FirstName = Convert.ToString(dr["Guest_FirstName"]);
                        ent.Guest_Username = Convert.ToString(dr["Guest_Username"]);
                        ent.Guest_Password = Convert.ToString(dr["Guest_Password"]);
                    }
                }
            }
            catch (Exception ex)
            {
                InsertException(ex.Message, "SelectLogin", 0);
            }
            finally
            {
                con.Close();
            }
            return ent;
        }

        public Ent_Guest SelectGuestDetails(int ID)
        {
            Ent_Guest ent = new Ent_Guest();
            try
            {              
                using (SqlCommand cmd = new SqlCommand("EC_SelectGuestDetails", con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Guest_ID", ID));
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ent.Guest_ID = Convert.ToInt32(dr["Guest_ID"]);
                        ent.Guest_Username = Convert.ToString(dr["Guest_Username"]);
                        ent.Guest_Password = Convert.ToString(dr["Guest_Password"]);
                        ent.Guest_FirstName = Convert.ToString(dr["Guest_FirstName"]);
                        ent.Guest_LastName = Convert.ToString(dr["Guest_LastName"]);                      
                        ent.Guest_Email = Convert.ToString(dr["Guest_Email"]);
                        ent.Guest_Phone = Convert.ToString(dr["Guest_Phone"]);
                        ent.entGuestAddress.First_Name = Convert.ToString(dr["First_Name"]);
                        ent.entGuestAddress.Last_Name = Convert.ToString(dr["Last_Name"]);
                        ent.entGuestAddress.Guest_Address1 = Convert.ToString(dr["Guest_Address1"]);
                        ent.entGuestAddress.Guest_Address2 = Convert.ToString(dr["Guest_Address2"]);
                        ent.entGuestAddress.Guest_Town = Convert.ToString(dr["Guest_Town"]);
                        ent.entGuestAddress.Guest_State = Convert.ToString(dr["Guest_State"]);
                        ent.entGuestAddress.Guest_Country = Convert.ToString(dr["Guest_Country"]);
                    }
                }
            }
            catch (Exception ex)
            {
                InsertException(ex.Message, "SelectLogin", 0);
            }
            finally
            {
                con.Close();
            }
            return ent;
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

        public Ent_GuestAddress SelectGuestAddress(int ID)
        {
            Ent_GuestAddress ent = new Ent_GuestAddress();
            try
            {
                string query = "select * from EC_GuestAddress where Guest_ID=" + ID + "  and Address_Default=1";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ent.Address_ID = Convert.ToInt32(dr["Address_ID"]);
                        ent.Guest_ID = Convert.ToInt32(dr["Guest_ID"]);
                        ent.First_Name = Convert.ToString(dr["First_Name"]);
                        ent.Last_Name = Convert.ToString(dr["Last_Name"]);                       
                        ent.Guest_Address1 = Convert.ToString(dr["Guest_Address1"]);
                        ent.Guest_Address2 = Convert.ToString(dr["Guest_Address2"]);
                        ent.Guest_Town = Convert.ToString(dr["Guest_Town"]);
                        ent.Guest_State = Convert.ToString(dr["Guest_State"]);
                        ent.Guest_Country = Convert.ToString(dr["Guest_Country"]);
                        ent.Address_Type = Convert.ToString(dr["Address_Type"]);
                        ent.Address_Default = Convert.ToInt32(dr["Address_Default"]);
                    }
                }
            }
            catch (Exception ex)
            {
                InsertException(ex.Message, "SelectGuestAddress", ID);
            }
            finally
            {
                con.Close();
            }
            return ent;
        }

        public List<Ent_GuestAddress> SelectGuestAddressList(int ID)
        {
            List<Ent_GuestAddress> list = new List<Ent_GuestAddress>();

            Ent_GuestAddress ent = new Ent_GuestAddress();
            try
            {
                string query = "select * from EC_GuestAddress where Guest_ID=" + ID ;
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ent = new Ent_GuestAddress();
                        ent.Address_ID = Convert.ToInt32(dr["Address_ID"]);
                        ent.Guest_ID = Convert.ToInt32(dr["Guest_ID"]);
                        ent.First_Name = Convert.ToString(dr["First_Name"]);
                        ent.Last_Name = Convert.ToString(dr["Last_Name"]);
                        ent.Guest_Address1 = Convert.ToString(dr["Guest_Address1"]);
                        ent.Guest_Address2 = Convert.ToString(dr["Guest_Address2"]);
                        ent.Guest_Town = Convert.ToString(dr["Guest_Town"]);
                        ent.Guest_State = Convert.ToString(dr["Guest_State"]);
                        ent.Guest_Country = Convert.ToString(dr["Guest_Country"]);
                        ent.Address_Type = Convert.ToString(dr["Address_Type"]);
                        ent.Address_Default = Convert.ToInt32(dr["Address_Default"]);
                        list.Add(ent);
                    }
                }
            }
            catch (Exception ex)
            {
                InsertException(ex.Message, "SelectGuestAddressList", ID);
            }
            finally
            {
                con.Close();
            }
            return list;
        }

        public int UpdateAddress(Ent_GuestAddress ent, SafeTransaction trans)
        {
            int dataresult = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (SqlCommand cmd = new SqlCommand("EC_UpdateAddress", trans.DatabaseConnection, trans.Transaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Address_ID", ent.Address_ID));
                    cmd.Parameters.Add(new SqlParameter("@Guest_ID", ent.Guest_ID));
                    cmd.Parameters.Add(new SqlParameter("@First_Name", ent.First_Name));
                    cmd.Parameters.Add(new SqlParameter("@Last_Name", ent.Last_Name));
                    cmd.Parameters.Add(new SqlParameter("@Guest_Address1", ent.Guest_Address1));
                    cmd.Parameters.Add(new SqlParameter("@Guest_Address2", ent.Guest_Address2));       
                    cmd.Parameters.Add(new SqlParameter("@Guest_Town", ent.Guest_Town));
                    cmd.Parameters.Add(new SqlParameter("@Guest_State", ent.Guest_State));
                    cmd.Parameters.Add(new SqlParameter("@Guest_Country", ent.Guest_Country));
                    cmd.Parameters.Add(new SqlParameter("@Address_Type", ent.Address_Type));
                    cmd.Parameters.Add(new SqlParameter("@Address_Default", ent.Address_Default));
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
                        InsertException(ex.Message, "UpdateAddress", ent.Guest_ID);
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