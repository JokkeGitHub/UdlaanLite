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
        #region LOANER TABLE

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

        #region LOAN TABLE

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
                    activeLoanInfo = $"Lån ID: { dataRow["loanId"] } \nUNI Login: { dataRow["uniLogin"] } \nQR ID: { dataRow["qrId"] } \nStart dato: { dataRow["startDate"] } \nSlut dato:  { dataRow["endDate"] }";
                }
            }

            conn.Close();
            return activeLoanInfo;
        }

        public static bool CheckLoanTableForQR(string qrId)
        {
            bool pcInStock = true;

            SqlConnection conn = new SqlConnection(@"Database=SKPUdlaanDB;Trusted_Connection=Yes;");

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT (qrId) FROM Loan WHERE (qrId) = (@qrId);";
            cmd.Parameters.AddWithValue("@qrId", qrId);
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow["qrId"].ToString() == qrId)
                {
                    pcInStock = false;
                    conn.Close();
                    return pcInStock;
                }
            }

            conn.Close();
            return pcInStock;
        }

        public static string GetPCNotInStockInfo(string qrId)
        {
            string pcNotInStockInfo = "";

            SqlConnection conn = new SqlConnection(@"Database=SKPUdlaanDB;Trusted_Connection=Yes;");

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT loanId, (qrId) FROM Loan WHERE (qrId) = (@qrId);";
            cmd.Parameters.AddWithValue("@qrId", qrId);
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow["qrId"].ToString() == qrId)
                {
                    pcNotInStockInfo = $"PC'en er allerede udlånt! \nLån ID: { dataRow["loanId"] } \nQR ID: { dataRow["qrId"] }";
                }
            }

            conn.Close();
            return pcNotInStockInfo;
        }

        #endregion

        #region PC TABLE

        public static bool CheckPCTableForQR(string qrId)
        {
            bool pcInStock = false;

            SqlConnection conn = new SqlConnection(@"Database=SKPUdlaanDB;Trusted_Connection=Yes;");

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

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
                    pcInStock = true;
                    conn.Close();
                    return pcInStock;
                }
            }

            conn.Close();
            return pcInStock;
        }

        #endregion
    }
}
