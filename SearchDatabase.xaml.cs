using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace UdlaansSystem
{
    /// <summary>
    /// Interaction logic for SearchDatabase.xaml
    /// </summary>
    public partial class SearchDatabase : Page
    {

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UdlaanLite"].ConnectionString);
        public string TableToSearch = "";

        public SearchDatabase()
        {
            InitializeComponent();
        }

        #region CLICK BUTTON SEARCHES

        private void BtnShowLoaners_Click(object sender, RoutedEventArgs e)
        {
            TableToSearch = "Loaner";

            LoanerColumns();

            SqlCommand cmd = conn.CreateCommand();
            conn.Open();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT * FROM Loaner;";
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow["login"].ToString().ToLower() == "service")
                { }
                else
                {
                    DataGridView.Items.Add(new { Column1 = dataRow["login"].ToString(), Column2 = dataRow["name"].ToString(), Column3 = dataRow["phone"].ToString() });
                }
            }

            conn.Close();
        }

        private void BtnShowPCs_Click(object sender, RoutedEventArgs e)
        {
            TableToSearch = "PcsOut";

            PCColumns();

            SqlCommand cmd = conn.CreateCommand();
            conn.Open();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT * FROM PC INNER JOIN Locations ON PC.qrId = Locations.qrId;";
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                DataGridView.Items.Add(new { Column1 = dataRow["qrId"].ToString(), Column2 = dataRow["model"].ToString(), Column3 = dataRow["serial"].ToString(), Column4 = dataRow["location"].ToString() });
            }

            conn.Close();
        }

        private void BtnShowLoans_Click(object sender, RoutedEventArgs e)
        {
            TableToSearch = "Loan";

            LoanColumns();

            SqlCommand cmd = conn.CreateCommand();
            conn.Open();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT * FROM ((Loan INNER JOIN Loaner ON Loan.uniLogin = Loaner.login) INNER JOIN PC ON Loan.qrId = PC.qrId INNER JOIN Locations ON PC.qrId = Locations.qrId);";
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                DataGridView.Items.Add(new { Column1 = dataRow["startDate"].ToString().Remove(dataRow["startDate"].ToString().Length - 8), Column2 = dataRow["qrId"].ToString(), Column3 = dataRow["model"].ToString(), Column4 = dataRow["name"].ToString(), Column5 = dataRow["phone"].ToString(), Column6 = dataRow["location"].ToString(), Column7 = dataRow["comment"].ToString() });
            }

            conn.Close();
        }

        private void BtnShowAvailablePCs_Click(object sender, RoutedEventArgs e)
        {
            TableToSearch = "PcsHome";

            string location = "Hjemme";

            PCColumns();

            SqlCommand cmd = conn.CreateCommand();
            conn.Open();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT * FROM PC WHERE NOT EXISTS (SELECT * FROM Loan WHERE qrId = PC.qrId);";
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                DataGridView.Items.Add(new { Column1 = dataRow["qrId"].ToString(), Column2 = dataRow["model"].ToString(), Column3 = dataRow["serial"].ToString(), Column4 = location });
            }

            conn.Close();
        }


        #endregion

        #region USER INPUT SEARCHES
        private void BtnSearchButton_Click(object sender, RoutedEventArgs e)
        {
            UserInputSearch();
        }

        private void BtnSearchInput_KeyUp(object sender, KeyEventArgs e)
        {
            UserInputSearch();
        }

        public void UserInputSearch()
        {
            if (TableToSearch == "Loan")
            {
                UserInputSearchLoan();
            }
            else if (TableToSearch == "Loaner")
            {
                UserInputSearchLoaner();
            }
            else if (TableToSearch == "PcsHome")
            {
                UserInputSearchPcsHome();
            }
            else if (TableToSearch == "PcsOut")
            {
                UserInputSearchPcsOut();
            }
        }

        #region LOAN
        public void UserInputSearchLoan()
        {
            string input = BtnSearchInput.Text.ToLower();
            LoanColumns();

            SqlCommand cmd = conn.CreateCommand();
            conn.Open();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT * FROM ((Loan INNER JOIN Loaner ON Loan.uniLogin = Loaner.login) INNER JOIN PC ON Loan.qrId = PC.qrId INNER JOIN Locations ON PC.qrId = Locations.qrId);";
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow["loanId"].ToString().ToLower().Contains(input) || dataRow["model"].ToString().ToLower().Contains(input) || dataRow["qrId"].ToString().ToLower().Contains(input) || dataRow["uniLogin"].ToString().ToLower().Contains(input) || dataRow["name"].ToString().ToLower().Contains(input) || dataRow["phone"].ToString().ToLower().Contains(input) || dataRow["location"].ToString().ToLower().Contains(input) || dataRow["comment"].ToString().ToLower().Contains(input))
                {
                    DataGridView.Items.Add(new { Column1 = dataRow["startDate"].ToString().Remove(dataRow["startDate"].ToString().Length - 8), Column2 = dataRow["qrId"].ToString(), Column3 = dataRow["model"].ToString(), Column4 = dataRow["name"].ToString(), Column5 = dataRow["phone"].ToString(), Column6 = dataRow["location"].ToString(), Column7 = dataRow["comment"].ToString() });
                }
            }

            conn.Close();
        }
        #endregion 
        #region LOANER
        public void UserInputSearchLoaner()
        {
            string input = BtnSearchInput.Text.ToLower();
            LoanerColumns();

            SqlCommand cmd = conn.CreateCommand();
            conn.Open();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT * FROM Loaner;";
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow["login"].ToString().ToLower() == "service")
                { }
                else if (dataRow["login"].ToString().ToLower().Contains(input) || dataRow["name"].ToString().ToLower().Contains(input) || dataRow["phone"].ToString().ToLower().Contains(input))
                {
                    DataGridView.Items.Add(new { Column1 = dataRow["login"].ToString(), Column2 = dataRow["name"].ToString(), Column3 = dataRow["phone"].ToString() });
                }
            }

            conn.Close();
        }
        #endregion  
        #region PCS HOME
        public void UserInputSearchPcsHome()
        {
            string input = BtnSearchInput.Text.ToLower();
            string location = "Hjemme";

            PCColumns();

            SqlCommand cmd = conn.CreateCommand();
            conn.Open();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT * FROM PC WHERE NOT EXISTS (SELECT * FROM Loan WHERE qrId = PC.qrId);";
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow["model"].ToString().ToLower().Contains(input) || dataRow["serial"].ToString().ToLower().Contains(input) || dataRow["qrId"].ToString().ToLower().Contains(input) || location.ToLower().Contains(input))
                {
                    DataGridView.Items.Add(new { Column1 = dataRow["qrId"].ToString(), Column2 = dataRow["model"].ToString(), Column3 = dataRow["serial"].ToString(), Column4 = location });
                }
            }

            conn.Close();
        }
        #endregion
        #region PCS OUT
        public void UserInputSearchPcsOut()
        {
            string input = BtnSearchInput.Text.ToLower();
            PCColumns();

            SqlCommand cmd = conn.CreateCommand();
            conn.Open();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT * FROM PC INNER JOIN Locations ON PC.qrId = Locations.qrId;";
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow["model"].ToString().ToLower().Contains(input) || dataRow["qrId"].ToString().ToLower().Contains(input) || dataRow["location"].ToString().ToLower().Contains(input))
                {
                    DataGridView.Items.Add(new { Column1 = dataRow["qrId"].ToString(), Column2 = dataRow["model"].ToString(), Column3 = dataRow["serial"].ToString(), Column4 = dataRow["location"].ToString() });
                }
            }

            conn.Close();
        }
        #endregion
        #endregion

        #region COLUMNS
        private void LoanerColumns()
        {
            DataGridView.Items.Clear();

            ((GridView)DataGridView.View).Columns[0].Header = "UNI Login :";
            ((GridView)DataGridView.View).Columns[1].Header = "Navn :";
            ((GridView)DataGridView.View).Columns[2].Header = "Telefon :";
            ((GridView)DataGridView.View).Columns[3].Header = "";
            ((GridView)DataGridView.View).Columns[4].Header = "";
            ((GridView)DataGridView.View).Columns[5].Header = "";
            ((GridView)DataGridView.View).Columns[6].Header = "";
            ((GridView)DataGridView.View).Columns[7].Header = "";
        }

        private void PCColumns()
        {
            DataGridView.Items.Clear();

            ((GridView)DataGridView.View).Columns[0].Header = "QR ID :";
            ((GridView)DataGridView.View).Columns[1].Header = "Model :";
            ((GridView)DataGridView.View).Columns[2].Header = "Serienummer :";
            ((GridView)DataGridView.View).Columns[3].Header = "Lokation :";
            ((GridView)DataGridView.View).Columns[4].Header = "";
            ((GridView)DataGridView.View).Columns[5].Header = "";
            ((GridView)DataGridView.View).Columns[6].Header = "";
            ((GridView)DataGridView.View).Columns[7].Header = "";
        }

        private void LoanColumns()
        {
            DataGridView.Items.Clear();

            ((GridView)DataGridView.View).Columns[0].Header = "Start Dato :";
            ((GridView)DataGridView.View).Columns[1].Header = "QR ID :";
            ((GridView)DataGridView.View).Columns[2].Header = "PC Model :";
            ((GridView)DataGridView.View).Columns[3].Header = "Låner Navn :";
            ((GridView)DataGridView.View).Columns[4].Header = "Telefon :";
            ((GridView)DataGridView.View).Columns[5].Header = "Lokation :";
            ((GridView)DataGridView.View).Columns[6].Header = "Kommentar :";
            ((GridView)DataGridView.View).Columns[7].Header = "";
        }
        #endregion
    }
}
