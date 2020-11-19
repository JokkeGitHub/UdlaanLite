using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace UdlaansSystem
{
    class RegisterSQLConnections
    {
        public static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["INDSÆT-DB-NAVN-HER"].ToString());
        public static SqlCommand cmd = new SqlCommand();

        public static void CreatePC(string _qrID, string _serialNumber, string _pcModel, bool _inStock)
        {
            cmd.Connection = conn;

            cmd.CommandText = @"INSERT INTO Costumers (QR_ID, loebe_nummer, pc_model, in_stock) VALUES (@QR_ID, @loebe_nummer, @pc_model, @in_stock)";
            cmd.Parameters.AddWithValue("@QR_ID", _qrID);
            cmd.Parameters.AddWithValue("@loebe_nummer", _serialNumber);
            cmd.Parameters.AddWithValue("@pc_model", _pcModel);
            cmd.Parameters.AddWithValue("@in_stock", _inStock);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
    }
}
