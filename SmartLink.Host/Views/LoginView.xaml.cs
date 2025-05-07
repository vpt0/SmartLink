using System;
using System.Windows;
using System.Windows.Controls;
using SmartLink.Host.Services;

namespace SmartLink.Host.Views
{
    public partial class LoginView : Window
    {
        private readonly FirebaseService _firebaseService;

        public LoginView()
        {
            InitializeComponent();
            _firebaseService = FirebaseService.Instance;
            this.Loaded += LoginView_Loaded;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            // Ensure window is visible
            this.Visibility = Visibility.Visible;
            this.Activate();
            this.Focus();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both email and password.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var userCredential = await _firebaseService.SignInWithEmailAndPasswordAsync(email, password);

                // Example: Navigate to dashboard or main window
                var dashboard = new DashboardView(userCredential.User.Uid);
                Application.Current.MainWindow = dashboard;
                dashboard.Show();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login failed: {ex.Message}", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            var createAccountView = new CreateAccountView();
            createAccountView.EmailSet += (s, email) =>
            {
                txtEmail.Text = email;
                txtPassword.Focus();
            };
            createAccountView.ShowDialog();
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please enter your email address first.", "Password Reset", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            MessageBox.Show("Password reset functionality will be implemented in a future update.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}