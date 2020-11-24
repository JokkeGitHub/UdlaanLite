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
            DoStuff();

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
                Test.Items.Add(dataRow["login"].ToString() + ", " + dataRow["name"].ToString() + ", " + dataRow["phone"].ToString() + ", " + dataRow["isStudent"].ToString());

                // if isStudent = true, test += ja osv
            }

            conn.Close();
        }

        private void DoStuff()
        {
            Test.Items.Clear();

            labelColumn1.Content = "UNI Login:";
            labelColumn2.Content = "Navn:";
            labelColumn3.Content = "Telefon:";
            labelColumn4.Content = "Elev :";
        }
    }
}
