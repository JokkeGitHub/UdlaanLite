﻿using System;
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
        public static string GetUniLoginFromLoan(string qrId)
        {
            string tempUniLogin = "";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UdlaanLite"].ConnectionString);

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT qrId, uniLogin FROM Loan WHERE (qrId) = (@qrId);";
            cmd.Parameters.AddWithValue("@qrId", qrId);
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
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UdlaanLite"].ConnectionString);

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"DELETE FROM Loan WHERE (qrId) = (@qrId);";
            cmd.Parameters.AddWithValue("@qrId", qrId);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public static void RemoveLoaner()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UdlaanLite"].ConnectionString);

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"DELETE FROM Loaner WHERE NOT EXISTS (SELECT * FROM Loan WHERE uniLogin = Loaner.login)";
            cmd.ExecuteNonQuery();

            conn.Close();
        }
    }
}
