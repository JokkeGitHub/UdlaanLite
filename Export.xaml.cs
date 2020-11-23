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
            string uniLogin = UniLoginInput.Text;
            string name = NameInput.Text;
            string phone = PhonenumberInput.Text;
            int isStudent = 1;
            // Lav en checkbox til elev

            SQLManager.ExportToLoaner(uniLogin, name, phone, isStudent);
        }
    }
}
