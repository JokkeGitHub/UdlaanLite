using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Diagnostics;

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
        }
        /// <summary>
        /// Scan card click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        private void DateInput_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (e.RightButton == MouseButtonState.Pressed)
            {
                ContextMenu cm = (ContextMenu)Resources["cmCalendar"];
                cm.IsOpen = true;
            }
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string uniLogin = UniLoginInput.Text.ToLower(); // Lav automatisk tjekker til UniLogin / Primary key
            bool uniLoginExists = SQLManager.CheckUniLogin(uniLogin);

            string name = NameInput.Text.ToLower();
            string phone = PhonenumberInput.Text;
            int isStudent;

            if (IsStudentCheckBox.IsChecked == true)
            {
                isStudent = 1;
            }
            else
            {
                isStudent = 0;
            }

            if (uniLoginExists == false)
            {

                SQLManager.CreateLoaner(uniLogin, name, phone, isStudent);
            }
            else
            {
                UniLoginInput.Text = "Har aktivt lån";
            }

            string qrId = QRInput.Text;

            DateTime startDate = DateTime.Now;
            DateTime endDate = DateInput.DisplayDate;

            SQLManager.CreateLoan(uniLogin, qrId, startDate, endDate);


            // Tjek db om pc er lånt ud

        }

        private void IsStudentCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (IsStudentCheckBox.IsChecked == true)
            {
                IsTeacherCheckBox.IsChecked = false;
            }
        }

        private void IsTeacherCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (IsTeacherCheckBox.IsChecked == true)
            {
                IsStudentCheckBox.IsChecked = false;
            }
        }
    }
}
