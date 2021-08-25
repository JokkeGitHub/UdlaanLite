using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaansSystem
{
    class ImportSQLConnection
    {
        static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UdlaanLite"].ConnectionString);

        public static string GetUniLoginFromLoan(string qrId)
        {
            string tempUniLogin = "";

            SqlCommand cmd = conn.CreateCommand();
            conn.Open();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT qrId, uniLogin FROM Loan WHERE (qrId) = (@qrId);";
            cmd.Parameters.AddWithValue("@qrId", qrId.ToLower());
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow["qrId"].ToString() == qrId)
                {
                    tempUniLogin = dataRow["uniLogin"].ToString();
                }
            }

            conn.Close();
            return tempUniLogin;
        }

        public static void RemoveLoanFromDatabase(string qrId)
        {
            SqlCommand cmd = conn.CreateCommand();
            conn.Open();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"DELETE FROM Loan WHERE (qrId) = (@qrId);";
            cmd.Parameters.AddWithValue("@qrId", qrId.ToLower());
            cmd.ExecuteNonQuery();

            conn.Close();

            RemovePCFromLocation(qrId);
        }

        public static void RemovePCFromLocation(string _qrId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = @"DELETE FROM Locations WHERE (qrId) = (@qrId);";
            cmd.Parameters.AddWithValue("@qrId", _qrId.ToLower());

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
            conn.Close();

            AddPCToLocation(_qrId);
        }

        public static void AddPCToLocation(string _qrId)
        {
            string location = "Hjemme";

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = @"INSERT INTO locations (location, qrId) VALUES (@location, @qrId)";
            cmd.Parameters.AddWithValue("@location", location.ToLower());
            cmd.Parameters.AddWithValue("@qrId", _qrId.ToLower());

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
            conn.Close();
        }

        public static void RemoveLoaner()
        {
            SqlCommand cmd = conn.CreateCommand();
            conn.Open();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"DELETE FROM Loaner WHERE NOT EXISTS (SELECT * FROM Loan WHERE uniLogin = Loaner.login)";
            cmd.ExecuteNonQuery();

            conn.Close();
        }
    }
}
