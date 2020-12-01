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

namespace UdlaansSystem
{
    /// <summary>
    /// Interaction logic for Import.xaml
    /// </summary>
    public partial class Import : Page
    {
        public Import()
        {
            InitializeComponent();
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            ReturnPC(QRInput.Text);
        }

        private void QRInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    e.Handled = true;
                    ReturnPC(QRInput.Text);
                }
                catch (Exception)
                { }
            }
        }

        public void ReturnPC(string qrId)
        {
            SQLManager.DeleteLoanAndLoaner(qrId);

            ClearInputField();

            ReturnConfirmationMessageBox();
        }

        public void ClearInputField()
        {
            QRInput.Clear();
        }

        public void ReturnConfirmationMessageBox()
        {
            string confirmationMessage = "Afleveringen er accepteret!";

            MessageBox.Show(confirmationMessage);
        }
    }
}
