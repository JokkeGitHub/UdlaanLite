using System;
using System.Windows;
using System.Windows.Input;

namespace UdlaansSystem
{
    public partial class MainWindow : Window
    {
        #region WindowUI
        /// <summary>
        /// Minimize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Button_Click_Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// Resize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Close
        /// </summary>
        /// <param name = "sender" ></ param >
        /// < param name="e"></param>
        public void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Drag header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void WindowHeader_Mousedown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        /// <summary>
        /// Export page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Export_Click(object sender, RoutedEventArgs e)
        {
            Display.Content = new Export();
        }
        /// <summary>
        /// Import page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Import_Click(object sender, RoutedEventArgs e)
        {
            Display.Content = new Import();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Display.Content = new Register();
        }
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            Display.Content = new Export();
        }

        private void SearchDatabase_Click(object sender, RoutedEventArgs e)
        {
            Display.Content = new SearchDatabase();
        }
    }
}
