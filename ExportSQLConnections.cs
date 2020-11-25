using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaansSystem
{
    class ExportSQLConnections
    {

        public static void CreateLoaner(string _uniLogin, string _name, string _phone, int _isStudent)
        {
            SqlConnection conn = new SqlConnection(@"Database=SKPUdlaanDB;Trusted_Connection=Yes;");
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;

            cmd.CommandText = @"INSERT INTO loaner(login, name, phone, isStudent) VALUES (@login, @name, @phone, @isStudent)";
            cmd.Parameters.AddWithValue("@login", _uniLogin);
            cmd.Parameters.AddWithValue("@name", _name);
            cmd.Parameters.AddWithValue("@phone", _phone);
            cmd.Parameters.AddWithValue("@isStudent", _isStudent);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }

        public static void CreateLoan(DateTime _startDate, DateTime _endDate)
        {
            SqlConnection conn = new SqlConnection(@"Database=SKPUdlaanDB;Trusted_Connection=Yes;");
            SqlCommand cmd = new SqlCommand();

            // Find ud af det med de foreign keys!
            cmd.Connection = conn;

            cmd.CommandText = @"INSERT INTO udlaant(start_date, end_date) VALUES (@start_date, @end_date)";
            cmd.Parameters.AddWithValue("@start_date", _startDate);
            cmd.Parameters.AddWithValue("@end_date", _endDate);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
            conn.Close();
        }
    }
}
