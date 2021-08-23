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
    class ExportSQLConnections
    {
        #region LOANER TABLE

        public static void CreateLoaner(string _uniLogin, string _name, string _phone, int _isStudent)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UdlaanLite"].ConnectionString);
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

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UdlaanLite"].ConnectionString);

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
            }

            conn.Close();
            return uniLoginExists;
        }

        public static int CheckDataBaseForIsStudent(int isStudent, string uniLogin)
        {            
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UdlaanLite"].ConnectionString);

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT (login), isStudent FROM Loaner WHERE (login) = (@login);";
            cmd.Parameters.AddWithValue("@login", uniLogin);
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow["login"].ToString() == uniLogin.ToLower())
                {
                    isStudent = Convert.ToInt32(dataRow["isStudent"]);
                }
            }
            
            conn.Close();
            return isStudent;
        }

        #endregion

        #region LOAN TABLE

        public static void CreateLoan(string _uniLogin, string _qrId, string comment, DateTime _startDate)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UdlaanLite"].ConnectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;

            cmd.CommandText = @"INSERT INTO Loan(uniLogin, qrId, comment, startDate) VALUES ((SELECT login FROM Loaner WHERE login = @login), (SELECT qrId FROM PC WHERE qrId = @qrId), @comment, @startDate)";
            cmd.Parameters.AddWithValue("@login", _uniLogin);
            cmd.Parameters.AddWithValue("@qrId", _qrId);
            cmd.Parameters.AddWithValue("@comment", comment);
            cmd.Parameters.AddWithValue("@startDate", _startDate);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
            conn.Close();

            RemovePCFromLocation(_qrId, _uniLogin);
        }

        public static void RemovePCFromLocation(string _qrId, string _uniLogin)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UdlaanLite"].ConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = @"DELETE FROM Locations WHERE (qrId) = (@qrId);";
            cmd.Parameters.AddWithValue("@qrId", _qrId);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
            conn.Close();

            AddPCToLocation(_qrId, _uniLogin);
        }

        public static void AddPCToLocation(string _qrId, string _uniLogin)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UdlaanLite"].ConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = @"INSERT INTO locations (location, qrId) VALUES (@location, @qrId)";
            cmd.Parameters.AddWithValue("@location", _uniLogin);
            cmd.Parameters.AddWithValue("@qrId", _qrId);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
            conn.Close();
        }

        public static string GetLoanInfo(string uniLogin)
        {
            string activeLoanInfo = "";

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UdlaanLite"].ConnectionString);

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT loanId, (uniLogin), qrId, startDate FROM Loan WHERE (uniLogin) = (@uniLogin);";
            cmd.Parameters.AddWithValue("@uniLogin", uniLogin);
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow["uniLogin"].ToString() == uniLogin.ToLower())
                {
                    activeLoanInfo = $"Lån ID: { dataRow["loanId"] } \nUNI Login: { dataRow["uniLogin"] } \nQR ID: { dataRow["qrId"] } \nStart dato: { dataRow["startDate"].ToString().Remove(dataRow["startDate"].ToString().Length - 8) }";
                }
            }

            conn.Close();
            return activeLoanInfo;
        }

        public static bool CheckLoanTableForQR(string qrId)
        {
            bool pcInStock = true;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UdlaanLite"].ConnectionString);

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

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UdlaanLite"].ConnectionString);

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
                    pcNotInStockInfo = $"PC'en med QR { dataRow["qrId"] } allerede udlånt! \nLån ID: { dataRow["loanId"] }";
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

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UdlaanLite"].ConnectionString);

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
