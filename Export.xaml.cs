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
        private void QRScannerInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    QRScannerInput.Text = "Write some data: ";
                    WebClient client = new WebClient();
                    client.DownloadFile($@"https://api.qrserver.com/v1/create-qr-code/?color=255-0-0&bgcolor=255-255-255&format=png&data={QRScannerInput.Text}", $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\example.png");
                }
                catch (Exception)
                { }
                if (File.Exists($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\example.png"))
                {
                    QRScannerInput.Text = "Success";
                    Process.Start($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\example.png");
                }
                else
                    QRScannerInput.Text = "Failed";
            }

        }

    }
}
