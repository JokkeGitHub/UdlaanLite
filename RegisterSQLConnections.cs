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
        public static void CreatePC(string _qrID, string _serialNumber, string _pcModel)
        {
            SqlConnection conn = new SqlConnection(@"Database=SKPUdlaanDB;Trusted_Connection=Yes;");
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;

            cmd.CommandText = @"INSERT INTO pc (qrId, serial, model) VALUES (@qrId, @serial, @model)";
            cmd.Parameters.AddWithValue("@qrId", _qrID);
            cmd.Parameters.AddWithValue("@serial", _serialNumber);
            cmd.Parameters.AddWithValue("@model", _pcModel);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
            conn.Close();
        }
    }
}
