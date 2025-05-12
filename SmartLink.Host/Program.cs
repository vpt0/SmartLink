using System;
using System.Windows;
<<<<<<< HEAD
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartLink.Host.Services;
=======
using SmartLink.Host.Views;
>>>>>>> eee952ce3da11a2d061ccfb915d29686bd487330

namespace SmartLink.Host
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            
            var serviceProvider = services.BuildServiceProvider();
            
            var application = new Application();
            var dashboardView = new DashboardView();
            application.MainWindow = dashboardView;
            dashboardView.Show();
            application.Run();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            // Add logging
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            // Add HttpClient
            services.AddHttpClient();

            // Add ResourceIsolationService
            services.AddSingleton<ResourceIsolationService>();
        }
    }
}