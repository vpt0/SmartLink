using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartLink.Host.Services;

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
            application.StartupUri = new Uri("Views/LoginView.xaml", UriKind.Relative);
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