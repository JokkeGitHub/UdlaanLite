using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;

namespace UdlaansSystem
{
    class RegisterSQLConnections
    {
        static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UdlaanLite"].ConnectionString);

        public static void CreatePC(string _qrID, string _serialNumber, string _pcModel)
        {
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

            AddPCLocation(_qrID);
        }

        public static void AddPCLocation(string _qrId)
        {
            string location = "Hjemme";

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = @"INSERT INTO locations (location, qrId) VALUES (@location, @qrId)";
            cmd.Parameters.AddWithValue("@location", location);
            cmd.Parameters.AddWithValue("@qrId", _qrId);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
            conn.Close();
        }

        public static void DeletePc(string _qrID)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UdlaanLite"].ConnectionString);

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"DELETE FROM pc WHERE (qrId) = (@qrId);";
            cmd.Parameters.AddWithValue("@qrId", _qrID);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        #region CHECKING DATABASE FOR DATA
        public static bool CheckDatabaseForQR(string qrId)
        {
            bool qrIdExists = false;

            SqlCommand cmd = conn.CreateCommand();
            conn.Open();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT (qrId) FROM PC WHERE (qrId) = (@qrId);";
            cmd.Parameters.AddWithValue("@qrId", qrId);
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow["qrId"].ToString() == qrId)
                {
                    qrIdExists = true;
                }
            }

            conn.Close();
            return qrIdExists;
        }
        #endregion

        public static string GetPCInfo(string qrId)
        {
            string registeredPCInfo = "";

            SqlCommand cmd = conn.CreateCommand();
            conn.Open();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT (qrId), serial, model FROM PC WHERE (qrId) = (@qrId);";
            cmd.Parameters.AddWithValue("@qrId", qrId);
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow["qrId"].ToString() == qrId)
                {
                    registeredPCInfo = $"QR ID: { dataRow["qrId"] } \nLøbenummer: { dataRow["serial"] } \nModel: { dataRow["model"] }";
                }
            }

            conn.Close();
            return registeredPCInfo;
        }
    }
}
