using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            string tempUniLogin = SQLManager.GetUniLoginFromLoan(qrId);

            if (tempUniLogin != "")
            {
                SQLManager.DeleteLoanAndLoaner(qrId);
            }

            ClearInputField();

            ReturnConfirmationMessageBox(tempUniLogin);
        }

        public void DeleteLoan(string qrId)
        {
            SQLManager.DeleteLoanAndLoaner(qrId);
        }

        public void ClearInputField()
        {
            QRInput.Clear();
        }

        public void ReturnConfirmationMessageBox(string tempUniLogin)
        {
            string confirmationMessage = "";

            if (tempUniLogin != "")
            {
                confirmationMessage = "Afleveringen er accepteret!";
            }
            else
            {
                confirmationMessage = "Denne PC er ikke udlånt!";
            }

            MessageBox.Show(confirmationMessage);
        }

        private void BtnClearInput_Click(object sender, RoutedEventArgs e)
        {
            ClearInputField();
        }

        //Letters & Numbers
        private void LetterAndNumberPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^a-zA-Z0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        //No spaces allowed
        private void spacekey_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
