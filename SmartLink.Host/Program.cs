using System;
using System.Windows;

namespace SmartLink.Host
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var application = new Application();
            application.StartupUri = new Uri("Views/LoginView.xaml", UriKind.Relative);
            application.Run();
        }
    }
}