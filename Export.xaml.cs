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

            DateInput.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            DateInput.DisplayDateStart = DateTime.Now.AddDays(1);

            IsStudentCheckBox.IsChecked = true;

            //QRInput.Visibility = Visibility.Hidden;
        }

        #region DATEPICKER
        private void DateInput_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (e.RightButton == MouseButtonState.Pressed)
            {
                ContextMenu cm = (ContextMenu)Resources["cmCalendar"];
                cm.IsOpen = true;
            }
        }
        #endregion

        #region SUBMIT
        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            ResetLabelColors();

            bool NoEmptyFields = false;
            NoEmptyFields = CheckForEmptyFields(NoEmptyFields);

            bool uniLoginExists = true;
            string uniLogin = UniLoginInput.Text.ToLower();

            int isStudent = 1;

            string name = NameInput.Text.ToLower();
            string phone = PhonenumberInput.Text;

            string qrId = QRInput.Text;

            if (NoEmptyFields == true)
            {
                CheckForExistingUNILogin(uniLoginExists, uniLogin);
                isStudent = StudentOrTeacher(isStudent);
                PassOnLoanerData(uniLoginExists, uniLogin, name, phone, isStudent);
            }




            DateTime startDate = DateTime.Now;
            DateTime endDate = DateInput.DisplayDate;

            SQLManager.CreateLoan(uniLogin, qrId, startDate, endDate);


            // Tjek db om pc er lånt ud
            // Tjek om UNILlogin har aktivt lån, hvis det er en elev


        }

        #region CHECK FOR EMPTY FIELDS
        public void ResetLabelColors()
        {
            UNILoginLabel.Foreground = new SolidColorBrush(Colors.White);
            NameLabel.Foreground = new SolidColorBrush(Colors.White);
            PhonenumberLabel.Foreground = new SolidColorBrush(Colors.White);
            QRLabel.Foreground = new SolidColorBrush(Colors.White);
        }

        public bool CheckForEmptyFields(bool NoEmptyFields)
        {
            NoEmptyFields = false;

            if (UniLoginInput.Text.Length != 8)
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
        #endregion

        #region CHECK DATABASE FOR UNILOGIN
        public bool CheckForExistingUNILogin(bool uniLoginExists, string uniLogin)
        {
            uniLoginExists = SQLManager.CheckUniLogin(uniLogin);

            return uniLoginExists;
        }
        #endregion

        #region STUDENT OR TEACHER CHECK
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
        public bool PassOnLoanerData(bool uniLoginExists, string uniLogin, string name, string phone, int isStudent)
        {
            // Tjek om UNILlogin har aktivt lån, hvis det er en elev

            if (uniLoginExists == false)
            {
                SQLManager.CreateLoaner(uniLogin, name, phone, isStudent);
            }
            else
            {
                UniLoginInput.Text = "Har aktivt lån";
            }

            return uniLoginExists;
        }
        #endregion

        #endregion

        #region CHECKBOXES
        private void IsStudentCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (IsStudentCheckBox.IsChecked == true)
            {
                IsTeacherCheckBox.IsChecked = false;
                QRMultiInput.Visibility = Visibility.Hidden;
                ListLabel.Visibility = Visibility.Hidden;
            }
        }

        private void IsTeacherCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (IsTeacherCheckBox.IsChecked == true)
            {
                IsStudentCheckBox.IsChecked = false;
                QRMultiInput.Visibility = Visibility.Visible;
                ListLabel.Visibility = Visibility.Visible;
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

                    if (IsTeacherCheckBox.IsChecked == true)
                    {
                        AddToListBox();
                    }
                    else
                    { }
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

    }
}
