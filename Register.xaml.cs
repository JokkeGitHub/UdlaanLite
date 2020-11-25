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
            string qrId = QRIDInput.Text;
            string serialNumber = SerialNumberInput.Text;
            string pcModel = PcModelInput.Text;

            SQLManager.RegisterPC(qrId, serialNumber, pcModel);
        }
        
        // Make a mathed which checks if the field is empty
    }
}
