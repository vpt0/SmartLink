using System.Windows;

namespace SmartLink.Host.Views
{
    public partial class DashboardView : Window
    {
        private string? _userId;

        public DashboardView()
        {
            _userId = string.Empty;
            InitializeComponent();
            this.Loaded += DashboardView_Loaded;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public DashboardView(string userId) : this()
        {
            _userId = userId;
        }

        private void DashboardView_Loaded(object sender, RoutedEventArgs e)
        {
            // Ensure window is visible
            this.Visibility = Visibility.Visible;
            this.Activate();
            this.Focus();
        }
    }
}