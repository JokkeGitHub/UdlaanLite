using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        public void CreateFoldersPCQR()
        {
            // Specify the directory you want to manipulate.
            string folderPathPC = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\QRcodesPC";

            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(folderPathPC))
                {
                    return;
                }
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(folderPathPC);
            }
            catch { }
            finally { }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterPC();
        }

        private void QRIDInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    e.Handled = true;
                    RegisterPC();
                }
                catch (Exception)
                { }
            }
        }

        public void RegisterPC()
        {
            ResetLabelColors();

            bool NoEmptyFields = false;
            NoEmptyFields = CheckForEmptyFields(NoEmptyFields);

            bool qrIdExists = true;
            string qrId = QRIDInput.Text;

            string serialNumber = SerialNumberInput.Text;
            string pcModel = PcModelInput.Text;

            if (NoEmptyFields == true)
            {
                qrIdExists = CheckForExistingPC(qrIdExists, qrId);
            }

            if (qrIdExists == true)
            {
                RegisteredPCMessageBox(qrId);
            }
            else
            {
                CreateFoldersPCQR();
                SQLManager.RegisterPC(qrId, serialNumber, pcModel);
                SuccessfullyRegisteredPCMessageBox();
            }
        }

        #region CHECK FOR EMPTY FIELDS
        public void ResetLabelColors()
        {
            QRLabel.Foreground = new SolidColorBrush(Colors.White);
            SerialLabel.Foreground = new SolidColorBrush(Colors.White);
            ModelLabel.Foreground = new SolidColorBrush(Colors.White);
        }

        public bool CheckForEmptyFields(bool NoEmptyFields)
        {
            NoEmptyFields = false;

            if (QRIDInput.Text.Length < 11)
            {
                QRLabel.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (SerialNumberInput.Text.Length < 4)
            {
                SerialLabel.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (PcModelInput.Text == "")
            {
                ModelLabel.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                NoEmptyFields = true;
            }

            return NoEmptyFields;
        }
        #endregion

        #region CHECK DATABASE FOR PC
        public bool CheckForExistingPC(bool qrIdExists, string qrId)
        {
            qrIdExists = SQLManager.CheckQR(qrId);

            return qrIdExists;
        }
        #endregion

        #region MESSSAGEBOXES
        public void RegisteredPCMessageBox(string qrId)
        {
            string registeredPCInfo = "PC med denne QR kode er allerede registreret!\n";
            registeredPCInfo += SQLManager.GetRegisteredPCInfo(qrId);

            MessageBox.Show(registeredPCInfo);
        }

        public void SuccessfullyRegisteredPCMessageBox()
        {
            string successMessage = "PC'en blev registreret i databasen!";

            ClearInputFields();

            MessageBox.Show(successMessage);
        }
        #endregion

        public void ClearInputFields()
        {
            PcModelInput.Clear();
            SerialNumberInput.Clear();
            QRIDInput.Clear();
        }
        private void BtnClearInput_Click(object sender, RoutedEventArgs e)
        {
            ClearInputFields();
        }










        // Prøv at få det til at virke med FontSize når vinduet bliver mindre
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if (Register.WindowState = WindowState.Maximized)
            //{

            //}
            //QRIDInput.FontSize = 10;
        }
    }
}

