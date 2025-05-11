using System;
using System.Windows;
using SmartLink.Host.Views;

namespace SmartLink.Host
{
    public class Program
    {
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