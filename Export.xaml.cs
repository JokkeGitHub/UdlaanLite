using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace UdlaansSystem
{
    public partial class Export : Page
    {
        /// <summary>
        /// Main
        /// </summary>
        public Export()
        {
            InitializeComponent();

            //DateInput.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            //DateInput.DisplayDateStart = DateTime.Now.AddDays(1);

            IsStudentCheckBox.IsChecked = true;
        }

        #region DATEPICKER
        /*
        private void DateInput_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (e.RightButton == MouseButtonState.Pressed)
            {
                ContextMenu cm = (ContextMenu)Resources["cmCalendar"];
                cm.IsOpen = true;                
            }
        }
        */
        #endregion

        // Submit region needs clean-up
        #region SUBMIT
        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            Submit();
        }

        public void Submit()
        {
            // Put hele den her i flere metoder og henvis til en "Submit" metode
            ResetLabelColors();

            bool noEmptyFields = true;
            //noEmptyFields = CheckForEmptyFields(noEmptyFields);

            bool uniLoginExists = true;
            string uniLogin = UniLoginInput.Text.ToLower();

            int isStudent = 1;

            /*
            string nameTemp = NameInput.Text.ToLower();
            string phoneTemp = PhonenumberInput.Text;
            string commentTemp = CommentInput.Text.ToLower();
            string qrIdTemp = QRInput.Text.ToLower();*/

            string name = NameInput.Text.ToLower();
            string phone = PhonenumberInput.Text;
            string comment = CommentInput.Text.ToLower();
            string qrId = QRInput.Text.ToLower();

            bool isTeacher = false;

            bool pcInStock = false;


            if (noEmptyFields == true)
            {
                uniLoginExists = CheckForExistingUNILogin(uniLoginExists, uniLogin);
            }

            if (uniLoginExists == true)
            {
                isStudent = CheckExistingUniLoginForStudentOrTeacher(isStudent, uniLogin);
            }
            else
            {
                isStudent = StudentOrTeacher(isStudent);
            }

            if (isStudent == 0)
            {
                isTeacher = true;
            }

            DateTime startDate = DateTime.Now;
            //DateTime endDate = (DateTime)DateInput.SelectedDate;

            if (uniLoginExists == false && isTeacher == false)
            {
                pcInStock = CheckForPCInStock(pcInStock, qrId);

                if (pcInStock == true)
                {
                    PassOnLoanerData(uniLoginExists, uniLogin, name, phone, isStudent);
                    SQLManager.CreateLoan(uniLogin, qrId, comment, startDate);
                    LoanConfirmationMessageBox();
                    Clear();
                }
            }
            else if (uniLoginExists == true && isTeacher == false)
            {
                ActiveLoanMessageBox(uniLogin);
            }/*
            else if (uniLoginExists == true && isTeacher == false && IsTeacherCheckBox.IsChecked == true)
            {
                UniLoginBelongsToStudentMessage();
            }*/
            else if (uniLoginExists == true && isTeacher == true && IsStudentCheckBox.IsChecked == true)
            {
                UniLoginBelongsToTeacherMessage();
            }
            else if (isTeacher == true)
            {
                // Ny metode for de her og henvis til den
                List<string> qrMultiList = new List<string>();

                if (QRInput.Text != "")
                {
                    QRMultiInput.Items.Add(QRInput.Text);
                }

                foreach (var pc in QRMultiInput.Items)
                {
                    pcInStock = CheckForPCInStock(pcInStock, pc.ToString());

                    if (pcInStock == true)
                    {
                        qrMultiList.Add(pc.ToString());
                    }
                }

                if (qrMultiList.Count != 0)
                {
                    PassOnLoanerData(uniLoginExists, uniLogin, name, phone, isStudent);

                    foreach (string qr in qrMultiList)
                    {
                        SQLManager.CreateLoan(uniLogin, qr, comment, startDate);
                    }

                    LoanConfirmationMessageBox();
                    qrMultiList.Clear();
                    Clear();
                }
            }
        }

        #region CHECK FOR EMPTY FIELDS
        public void ResetLabelColors()
        {
            StudentCheckBoxLabel.Foreground = new SolidColorBrush(Colors.White);
            UNILoginLabel.Foreground = new SolidColorBrush(Colors.White);
            NameLabel.Foreground = new SolidColorBrush(Colors.White);
            PhonenumberLabel.Foreground = new SolidColorBrush(Colors.White);
            QRLabel.Foreground = new SolidColorBrush(Colors.White);
        }
        /*
        public bool CheckForEmptyFields(bool NoEmptyFields)
        {
            NoEmptyFields = false;

            if (IsStudentCheckBox.IsChecked == false && IsTeacherCheckBox.IsChecked == false)
            {
                StudentCheckBoxLabel.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (UniLoginInput.Text.Length != 8)
            {
                UNILoginLabel.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (NameInput.Text == "")
            {
                NameLabel.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (PhonenumberInput.Text.Length != 8)
            {
                PhonenumberLabel.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (QRInput.Text.Length < 11 && QRMultiInput.Items.Count == 0)
            {
                QRLabel.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                NoEmptyFields = true;
            }

            return NoEmptyFields;
        }
        */
        #endregion

        #region CHECK DATABASE FOR UNILOGIN
        public bool CheckForExistingUNILogin(bool uniLoginExists, string uniLogin)
        {
            uniLoginExists = SQLManager.CheckUniLogin(uniLogin);

            return uniLoginExists;
        }
        #endregion

        #region STUDENT OR TEACHER CHECK

        public int CheckExistingUniLoginForStudentOrTeacher(int isStudent, string uniLogin)
        {
            isStudent = SQLManager.CheckIsStudentOrTeacher(isStudent, uniLogin);

            return isStudent;
        }

        public int StudentOrTeacher(int isStudent)
        {
            if (IsStudentCheckBox.IsChecked == true)
            {
                isStudent = 1;
            }
            else
            {
                isStudent = 0;
            }

            return isStudent;
        }
        #endregion

        #region PASS ON LOANER DATA TO DATABASE
        public void PassOnLoanerData(bool uniLoginExists, string uniLogin, string name, string phone, int isStudent)
        {
            if (uniLoginExists == false)
            {
                SQLManager.CreateLoaner(uniLogin, name, phone, isStudent);
            }
            else if (uniLoginExists == true && isStudent == 0)
            {

            }
        }
        #endregion

        #region CHECK DATABASE FOR QRID
        public bool CheckForPCInStock(bool pcInStock, string qrId)
        {
            pcInStock = SQLManager.CheckPCTableForQRID(qrId);

            if (pcInStock == true)
            {
                pcInStock = CheckForPCInLoan(pcInStock, qrId);
            }
            else
            {
                PCNotInStockMessageBox(qrId);
            }

            return pcInStock;
        }

        public bool CheckForPCInLoan(bool pcInStock, string qrId)
        {
            pcInStock = SQLManager.CheckLoanTableForQRID(qrId);

            if (pcInStock == true)
            {
                return pcInStock;
            }
            else
            {
                PCNotInStockMessageBox(qrId);
            }

            return pcInStock;
        }
        #endregion

        #endregion

        #region CHECKBOXES
        private void IsStudentCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (IsStudentCheckBox.IsChecked == true)
            {
                UniLoginInput.IsReadOnly = false;

                ToServiceCheckBox.IsChecked = false;
                QRMultiInput.Items.Clear();
                QRMultiInput.Visibility = Visibility.Hidden;
                ListLabel.Visibility = Visibility.Hidden;
                InnerBorder.Visibility = Visibility.Hidden;
                OuterBorder.Visibility = Visibility.Hidden;

                NameLabel.Visibility = Visibility.Visible;
                NameInput.Visibility = Visibility.Visible;
                PhonenumberLabel.Visibility = Visibility.Visible;
                PhonenumberInput.Visibility = Visibility.Visible;

                CommentLabel.Visibility = Visibility.Hidden;
                CommentInput.Visibility = Visibility.Hidden;

                UniLoginInput.Text = "";
            }
        }

        private void ToServiceCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (ToServiceCheckBox.IsChecked == true)
            {
                UniLoginInput.IsReadOnly = true;

                IsStudentCheckBox.IsChecked = false;
                QRMultiInput.Items.Clear();
                QRMultiInput.Visibility = Visibility.Hidden;
                ListLabel.Visibility = Visibility.Hidden;
                InnerBorder.Visibility = Visibility.Hidden;
                OuterBorder.Visibility = Visibility.Hidden;

                NameLabel.Visibility = Visibility.Hidden;
                NameInput.Visibility = Visibility.Hidden;
                PhonenumberLabel.Visibility = Visibility.Hidden;
                PhonenumberInput.Visibility = Visibility.Hidden;

                CommentLabel.Visibility = Visibility.Visible;
                CommentInput.Visibility = Visibility.Visible;

                UniLoginInput.Text = "Service";
            }
        }
        #endregion

        #region LISTBOX
        private void QRInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    e.Handled = true;

                    Submit();

                }
                catch (Exception) { }
            }
        }

        public void AddToListBox()
        {
            QRMultiInput.Items.Add(QRInput.Text);

            QRInput.Clear();
        }

        private void QRMultiInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                try
                {
                    e.Handled = true;
                    ListBoxRemoveItem(QRMultiInput.SelectedIndex);
                }
                catch (Exception) { }
            }
        }

        private void ListBoxRemoveItem(int listBoxIndex)
        {
            QRMultiInput.Items.RemoveAt(listBoxIndex);
        }
        #endregion

        // Fix the error messageboxes
        #region MESSSAGEBOXES
        public void ActiveLoanMessageBox(string uniLogin)
        {
            string activeLoanInfo = "Eleven har aktivt lån!\n";
            activeLoanInfo += SQLManager.GetActiveStudentLoanInfo(uniLogin); // Den her skal fikses

            MessageBox.Show(activeLoanInfo);
        }

        public void PCNotInStockMessageBox(string qrId)
        {
            string pcNotInStockInfo = "";
            pcNotInStockInfo += SQLManager.GetActivePCNotInStockInfo(qrId);

            if (pcNotInStockInfo != "")
            {
                pcNotInStockInfo = $"PC'en med QR {qrId} er ikke registreret i databasen!";
            }

            MessageBox.Show(pcNotInStockInfo);
        }

        public void LoanConfirmationMessageBox()
        {
            string confirmationMessage = "Lånet blev registreret i databasen!";

            ClearInputFields();

            MessageBox.Show(confirmationMessage);
        }

        public void UniLoginBelongsToStudentMessage()
        {
            string notTeacherMessage = "Dette UNI Login er tilknyttet en elev!";

            MessageBox.Show(notTeacherMessage);
        }
        public void UniLoginBelongsToTeacherMessage()
        {
            string notStudentMessage = "Dette UNI Login er tilknyttet en lærer!";

            MessageBox.Show(notStudentMessage);
        }
        #endregion

        #region CLEAR METHODS
        public void ClearInputFields()
        {
            if (IsStudentCheckBox.IsChecked == true)
            {
                Clear();
            }
        }

        private void BtnClearInput_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        public void Clear()
        {
            if (UniLoginInput.Text != "Service")
            {
                UniLoginInput.Clear();
            }
            NameInput.Clear();
            PhonenumberInput.Clear();
            CommentInput.Clear();
            QRInput.Clear();
            QRMultiInput.Items.Clear();
        }
        #endregion

        #region REGEX INPUT FIELDS

        //Phonenumber
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        // QR Input
        private void QRInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^a-zA-Z0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion

    }
}
