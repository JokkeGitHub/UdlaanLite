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
                //PassOnLoanerData(uniLoginExists, uniLogin, name, phone, isStudent);
            }

            if (qrIdExists == true)
            {
                QRIDInput.Text = "ERROR";
            }
            else
            {
                SQLManager.RegisterPC(qrId, serialNumber, pcModel);
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

        // Make a method which checks if the field is empty
    }
}

