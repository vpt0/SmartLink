using System;
using System.Windows;
using SmartLink.Host.Views;

namespace SmartLink.Host
{
    public class Program
    {
        /// <summary>
        /// Initializes and starts the WPF application with <c>DashboardView</c> as the main window.
        /// </summary>
        /// <param name="args">Command-line arguments passed to the application.</param>
        [STAThread]
        public static void Main(string[] args)
        {
            var application = new Application();
            var dashboardView = new DashboardView();
            application.MainWindow = dashboardView;
            dashboardView.Show();
            application.Run();
        }
    }
}