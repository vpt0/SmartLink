using System.Windows;
using System.Windows.Input;

namespace SmartLink.Host.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // Simple validation
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("Please enter both email and password", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // For now, just navigate to Dashboard
            var dashboard = new Dashboard();
            dashboard.Show();
            this.Close();
        }
    }
}