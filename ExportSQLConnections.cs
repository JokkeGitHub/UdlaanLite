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
    class ExportSQLConnections
    {

        public static void CreateLoaner(string _uniLogin, string _name, string _phone, int _isStudent)
        {
            SqlConnection conn = new SqlConnection(@"Database=SKPUdlaanDB;Trusted_Connection=Yes;");
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;

            cmd.CommandText = @"INSERT INTO Loaner(login, name, phone, isStudent) VALUES (@login, @name, @phone, @isStudent)";
            cmd.Parameters.AddWithValue("@login", _uniLogin);
            cmd.Parameters.AddWithValue("@name", _name);
            cmd.Parameters.AddWithValue("@phone", _phone);
            cmd.Parameters.AddWithValue("@isStudent", _isStudent);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }

        public static void CreateLoan(string _uniLogin, string _qrId, DateTime _startDate, DateTime _endDate)
        {
            SqlConnection conn = new SqlConnection(@"Database=SKPUdlaanDB;Trusted_Connection=Yes;");
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;

            cmd.CommandText = @"INSERT INTO Loan(uniLogin, qrId, startDate, endDate) VALUES ((SELECT login FROM Loaner WHERE login = @login), (SELECT qrId FROM PC WHERE qrId = @qrId), @startDate, @endDate)";
            cmd.Parameters.AddWithValue("@login", _uniLogin);
            cmd.Parameters.AddWithValue("@qrId", _qrId);
            cmd.Parameters.AddWithValue("@startDate", _startDate);
            cmd.Parameters.AddWithValue("@endDate", _endDate);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
            conn.Close();
        }

        #region CHECKING DATABASE FOR DATA
        public static bool CheckDatabaseForLogin(string uniLogin)
        {
            bool uniLoginExists = false;

            SqlConnection conn = new SqlConnection(@"Database=SKPUdlaanDB;Trusted_Connection=Yes;");

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT (login) FROM Loaner WHERE (login) = (@login);";
            cmd.Parameters.AddWithValue("@login", uniLogin);
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow["login"].ToString() == uniLogin.ToLower())
                {
                    uniLoginExists = true;
                }
                else
                {
                    uniLoginExists = false;
                }
            }

            conn.Close();
            return uniLoginExists;
        }
        #endregion

        public static string GetLoanInfo(string uniLogin)
        {
            string activeLoanInfo = "";

            SqlConnection conn = new SqlConnection(@"Database=SKPUdlaanDB;Trusted_Connection=Yes;");

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT loanId, (uniLogin), qrId, startDate, endDate FROM Loan WHERE (uniLogin) = (@uniLogin);";
            cmd.Parameters.AddWithValue("@uniLogin", uniLogin);
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow["uniLogin"].ToString() == uniLogin.ToLower())
                {
                    // Måske fjern QR ID'et og indsæt i stedet PC model og elev telefonnummer
                    activeLoanInfo = $"Lån ID: { dataRow["loanId"] } \nUNI Login: { dataRow["uniLogin"] } \nQR ID: { dataRow["qrId"] } \nStart dato: { dataRow["startDate"] } \nSlut dato:  { dataRow["endDate"] }";
                }
            }

            conn.Close();
            return activeLoanInfo;
        }
    }
}
