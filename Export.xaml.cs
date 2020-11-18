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
            CreateFolders();

        }
        /// <summary>
        /// Scan card click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 


        public void CreateFolders()
        {
            // Specify the directory you want to manipulate.
            string folderPathPC = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\QRcodesPC";
            string folderPathLoaner = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\QRcodesLoaner";

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

            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(folderPathLoaner))
                {
                    return;
                }
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(folderPathLoaner);
            }
            catch { }
            finally { }
        }


        private void QRScannerUniLogin_KeyDown(object sender, KeyEventArgs e)
        {
            string loanerQR = "udlaan" + UniLoginInput.Text;

            if (e.Key == Key.Enter)
            {
                try
                {
                    QRScannerUniLogin.Text = "Write some data: ";
                    WebClient client = new WebClient();
                    client.DownloadFile($@"https://api.qrserver.com/v1/create-qr-code/?color=255-0-0&bgcolor=255-255-255&format=png&data={loanerQR}", $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\QRcodesLoaner\{loanerQR}.png");
                }
                catch (Exception)
                { }
                if (File.Exists($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\QRcodesLoaner\{loanerQR}.png"))
                {
                    QRScannerUniLogin.Text = "Success";
                    Process.Start($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\QRcodesLoaner\{loanerQR}.png");
                }
                else
                    QRScannerUniLogin.Text = "Failed";
            }
        }

        private void QRScannerSerialNumber_KeyDown(object sender, KeyEventArgs e)
        {
            string dummySerial = "W5tR213lIm";

            string computerQR = "SkpRiIt" + dummySerial;

            if (e.Key == Key.Enter)
            {
                try
                {
                    QRScannerSerialNumber.Text = "Write some data: ";
                    WebClient client = new WebClient();
                    client.DownloadFile($@"https://api.qrserver.com/v1/create-qr-code/?color=255-0-0&bgcolor=255-255-255&format=png&data={computerQR}", $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\QRcodesPC\{computerQR}.png");
                }
                catch (Exception)
                { }
                if (File.Exists($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\QRcodesPC\{computerQR}.png"))
                {
                    // QRScannerSerialNumber.Text = "Success";
                    QRScannerSerialNumber.Text = dummySerial;
                    Process.Start($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\QRcodesPC\{computerQR}.png");
                }
                else
                    QRScannerSerialNumber.Text = "Failed";
            }
        }
    }
}
