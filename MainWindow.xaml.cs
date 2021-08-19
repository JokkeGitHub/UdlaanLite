using System;
using System.Windows;
using System.Windows.Input;

namespace UdlaansSystem
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Display.Content = new Export();
        }

        #region WindowUI Methods
        
        // Minimize
        public void Button_Click_Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        // Resize
        public void Button_Click_Resize(object sender, RoutedEventArgs e)
        {
            if (WindowState.Equals(WindowState.Normal))
            {
                WindowState = WindowState.Maximized;
                resizeButton.Content = ">";
            }
            else
            {
                WindowState = WindowState.Normal;
                resizeButton.Content = "<";
            }
        }
        // Close
        public void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        // Drag header
        public void WindowHeader_Mousedown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        // Export page
        private void Export_Click(object sender, RoutedEventArgs e)
        {
            Display.Content = new Export();
        }
        // Import page
        private void Import_Click(object sender, RoutedEventArgs e)
        {
            Display.Content = new Import();
        }
        // Register new pc or create qr code 
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Display.Content = new Register();
        }

        private void SearchDatabase_Click(object sender, RoutedEventArgs e)
        {
            Display.Content = new SearchDatabase();
        }
        #endregion 
    }
}
