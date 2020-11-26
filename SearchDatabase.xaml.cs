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

namespace UdlaansSystem
{
    /// <summary>
    /// Interaction logic for SearchDatabase.xaml
    /// </summary>
    public partial class SearchDatabase : Page
    {
        public SearchDatabase()
        {
            InitializeComponent();
        }

        private void BtnShowLoaners_Click(object sender, RoutedEventArgs e)
        {
            string title;
            LoanerColumns();

            SqlConnection conn = new SqlConnection(@"Database=SKPUdlaanDB;Trusted_Connection=Yes;");

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT login, name, phone, isStudent FROM Loaner";
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow["isStudent"].ToString() == "True")
                {
                    title = "Elev";
                }
                else
                {
                    title = "Lærer";
                }

                DataGridView.Items.Add( new { Column1 = dataRow["login"].ToString(), Column2 = dataRow["name"].ToString(), Column3 = dataRow["phone"].ToString(), Column4 = title });
                 // if isStudent = true, test += ja osv
            }

            conn.Close();
        }

        private void LoanerColumns()
        {            
            DataGridView.Items.Clear();

            ((GridView)DataGridView.View).Columns[0].Header = "UNI Login :";
            ((GridView)DataGridView.View).Columns[1].Header = "Navn :";
            ((GridView)DataGridView.View).Columns[2].Header = "Telefon :";
            ((GridView)DataGridView.View).Columns[3].Header = "Titel :";
            ((GridView)DataGridView.View).Columns[4].Header = "";
            //((GridView)Test2.View).Columns[0].Header = "UNI Login:";
        }

        private void BtnShowPCs_Click(object sender, RoutedEventArgs e)
        {
            PCColumns();

            SqlConnection conn = new SqlConnection(@"Database=SKPUdlaanDB;Trusted_Connection=Yes;");

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT qrId, serial, model FROM PC";
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                DataGridView.Items.Add(new { Column1 = dataRow["qrId"].ToString(), Column2 = dataRow["serial"].ToString(), Column3 = dataRow["model"].ToString() });
            }

            conn.Close();
        }

        private void PCColumns()
        {
            DataGridView.Items.Clear();

            ((GridView)DataGridView.View).Columns[0].Header = "QR ID :";
            ((GridView)DataGridView.View).Columns[1].Header = "Løbenummer :";
            ((GridView)DataGridView.View).Columns[2].Header = "Model :";
            ((GridView)DataGridView.View).Columns[3].Header = "";
            ((GridView)DataGridView.View).Columns[4].Header = "";
        }

        private void BtnShowLoans_Click(object sender, RoutedEventArgs e)
        {
            LoanColumns();

            SqlConnection conn = new SqlConnection(@"Database=SKPUdlaanDB;Trusted_Connection=Yes;");

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT loanId, uniLogin, qrId, endDate FROM Loan";
            cmd.ExecuteNonQuery();

            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTable);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                DataGridView.Items.Add(new { Column1 = dataRow["loanId"].ToString(), Column2 = dataRow["uniLogin"].ToString(), Column3 = dataRow["qrId"].ToString(), Column4 = dataRow["endDate"].ToString() });
            }

            conn.Close();

        }

        private void LoanColumns()
        {
            DataGridView.Items.Clear();

            ((GridView)DataGridView.View).Columns[0].Header = "Lån ID :";
            ((GridView)DataGridView.View).Columns[1].Header = "UNI Login :";
            ((GridView)DataGridView.View).Columns[2].Header = "QR ID :";
            ((GridView)DataGridView.View).Columns[3].Header = "Slut Dato";
            ((GridView)DataGridView.View).Columns[4].Header = "";

            // + Låner navn og telefon, måske model eller istedet for QR ID
        }
    }
}
