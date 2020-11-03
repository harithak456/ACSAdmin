using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EcommerceAdmin.Models.Common
{
    public class DBConnection
    {
        SqlConnection Connection;
        public DBConnection()
        {
            OpenConnection();
        }

        public SqlConnection DatabaseConnection
        {
            get { return Connection; }
        }

        public void OpenConnection()
        {
            if (Connection != null)
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
                Connection.Dispose();
            }

            Connection = new SqlConnection();
           // Connection.ConnectionString = @"Data Source=(local);Initial Catalog=AsterDietApp;User ID=sa;Password=admin123;MultipleActiveResultSets=True";            
            //Connection.ConnectionString = @"Data Source=184.168.47.21;Initial Catalog=epinoxerp_db;User ID=epinoxerp_usr;Password=epinoxerp_usr@123;MultipleActiveResultSets=True;";
            Connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            try
            {
                Connection.Open();
            }
            catch (Exception e)
            {
            }
        }
    }
}