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
using System.Drawing;
using QRCoder;
using System.Text.RegularExpressions;

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

            DeletePcCheckBoxNOTChecked();
            //CreateFolderNewQRCodes();
        }

        #region REGISTER
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

                    if (DeletePcCheckBox.IsChecked == false)
                    {
                        RegisterPC();
                    }
                    else if (DeletePcCheckBox.IsChecked == true)
                    {
                        Delete();
                    }
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

            if (qrIdExists == true && NoEmptyFields == true)
            {
                RegisteredPCMessageBox(qrId);
            }
            else if (qrIdExists == false && NoEmptyFields == true)
            {
                SQLManager.RegisterPC(qrId, serialNumber, pcModel);
                SuccessfullyRegisteredPCMessageBox();
            }
        }
        #endregion

        #region CHECK FOR EMPTY FIELDS
        public void ResetLabelColors()
        {
            ModelLabel.Foreground = new SolidColorBrush(Colors.White);
            SerialLabel.Foreground = new SolidColorBrush(Colors.White);
            QRLabel.Foreground = new SolidColorBrush(Colors.White);
        }

        public bool CheckForEmptyFields(bool NoEmptyFields)
        {
            NoEmptyFields = false;

            if (PcModelInput.Text == "")
            {
                ModelLabel.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (SerialNumberInput.Text.Length < 4)
            {
                SerialLabel.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (QRIDInput.Text.Length < 6)
            {
                QRLabel.Foreground = new SolidColorBrush(Colors.Red);
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
        public bool CheckForExistingLoan(bool qrIdExists, string qrId)
        {
            qrIdExists = SQLManager.CheckLoanTableForQRID(qrId);

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

        public void QRCodeCreationFinishedMessageBox(int numberOfQRcodes)
        {
            string qrCodesCreated = $"{numberOfQRcodes} QR koder blev genereret!";

            ClearInputFields();

            MessageBox.Show(qrCodesCreated);
        }
        #endregion

        #region DELETE
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Delete();
        }
        private void Delete()
        {
            string pcDeleted = "PC'en er blevet slettet fra databasen.";
            string pcNotFound = "PC'en kunne ikke findes i databasen.";
            string pcActiveLoan = "Det er ikke muligt at slette udlånte PC'er!";
            string deleteQR = QRIDInput.Text;

            bool pcIsHome = true;

            pcIsHome = CheckForExistingLoan(pcIsHome, deleteQR);

            if (pcIsHome == true)
            {
                if (CheckForExistingPC(true, QRIDInput.Text) != true)
                {
                    MessageBox.Show(pcNotFound);
                }
                else if (CheckForExistingPC(true, QRIDInput.Text) == true) // || loan exists == false
                {
                    SQLManager.DeletePC(deleteQR);
                    MessageBox.Show(pcDeleted);
                }
            }
            else
            {
                MessageBox.Show(pcActiveLoan);
            }
        }

        private void CheckLoanTableForQR(string qrId)
        {

        }
        #endregion

        #region CLEAR INPUT
        public void ClearInputFields()
        {
            PcModelInput.Clear();
            SerialNumberInput.Clear();
            QRIDInput.Clear();
            SerialMultiInput.Items.Clear();
        }

        private void BtnClearInput_Click(object sender, RoutedEventArgs e)
        {
            ClearInputFields();
        }
        #endregion

        #region CHECKBOX
        /*
        private void NewQRCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (NewQRCheckBox.IsChecked == true)
            {
                ModelLabel.Visibility = Visibility.Hidden;
                PcModelInput.Visibility = Visibility.Hidden;
                QRLabel.Visibility = Visibility.Hidden;
                QRIDInput.Visibility = Visibility.Hidden;
                BtnRegister.Visibility = Visibility.Hidden;

                BtnGenerate.Visibility = Visibility.Visible;
                ListLabel.Visibility = Visibility.Visible;
                BtnAddSerialToList.Visibility = Visibility.Visible;
                NewQRCheckBoxLabel.Visibility = Visibility.Visible;
                SerialMultiInput.Visibility = Visibility.Visible;
                InnerBorder.Visibility = Visibility.Visible;
                OuterBorder.Visibility = Visibility.Visible;
            }
        }
        */
        private void DeletePcCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (DeletePcCheckBox.IsChecked == true)
            {
                QRLabel.Visibility = Visibility.Visible;
                QRIDInput.Visibility = Visibility.Visible;
                BtnDelete.Visibility = Visibility.Visible;

                ModelLabel.Visibility = Visibility.Hidden;
                PcModelInput.Visibility = Visibility.Hidden;
                BtnRegister.Visibility = Visibility.Hidden;
                BtnAddSerialToList.Visibility = Visibility.Hidden;
                ListLabel.Visibility = Visibility.Hidden;
                SerialMultiInput.Visibility = Visibility.Hidden;
                InnerBorder.Visibility = Visibility.Hidden;
                OuterBorder.Visibility = Visibility.Hidden;
                SerialLabel.Visibility = Visibility.Hidden;
                SerialNumberInput.Visibility = Visibility.Hidden;
            }
        }

        private void DeletePcCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DeletePcCheckBoxNOTChecked();
        }

        public void DeletePcCheckBoxNOTChecked()
        {
            if (DeletePcCheckBox.IsChecked == false)
            {
                ModelLabel.Visibility = Visibility.Visible;
                PcModelInput.Visibility = Visibility.Visible;
                QRLabel.Visibility = Visibility.Visible;
                QRIDInput.Visibility = Visibility.Visible;
                SerialLabel.Visibility = Visibility.Visible;
                SerialNumberInput.Visibility = Visibility.Visible;
                BtnRegister.Visibility = Visibility.Visible;

                ListLabel.Visibility = Visibility.Hidden;
                BtnAddSerialToList.Visibility = Visibility.Hidden;
                SerialMultiInput.Visibility = Visibility.Hidden;
                InnerBorder.Visibility = Visibility.Hidden;
                OuterBorder.Visibility = Visibility.Hidden;
                BtnDelete.Visibility = Visibility.Hidden;
            }
        }
        #endregion

        #region LISTBOX
        private void BtnAddSerialToList_Click(object sender, RoutedEventArgs e)
        {
            ListBoxAddItem();
        }
        private void SerialNumberInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && DeletePcCheckBox.IsChecked == true)
            {
                try
                {
                    e.Handled = true;
                    ListBoxAddItem();
                }
                catch (Exception) { }
            }
        }
        private void SerialMultiInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                try
                {
                    e.Handled = true;
                    ListBoxRemoveItem(SerialMultiInput.SelectedIndex);
                }
                catch (Exception) { }
            }
        }
        private void ListBoxRemoveItem(int listBoxIndex)
        {
            SerialMultiInput.Items.RemoveAt(listBoxIndex);
        }

        public void ListBoxAddItem()
        {
            if (SerialNumberInput.Text != "")
            {
                SerialMultiInput.Items.Add(SerialNumberInput.Text);
            }

            SerialNumberInput.Clear();
        }
        #endregion

        #region REGEX

        //Numbers Only
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }
        //Letters & Numbers
        private void LetterAndNumberPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^a-zA-Z0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion

        /*
        public void CreateFolderNewQRCodes()
        {
            // Specify the directory you want to manipulate.
            string folderPathPC = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\Nye QRkoder";

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
        

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            ListBoxAddItem();
            CreateNewQRCodes();
        }

        public void CreateNewQRCodes()
        {
            for (int i = 0; i < SerialMultiInput.Items.Count; i++)
            {
                string tempSerial = SerialMultiInput.Items[i].ToString();

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode("SkpRiIt" + tempSerial, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                Bitmap image = qrCode.GetGraphic(20);

                image.Save($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\Nye QRkoder\{"SkpRiIt" + tempSerial}.png");
            }

            QRCodeCreationFinishedMessageBox(SerialMultiInput.Items.Count);
        }
        */
    }
}

