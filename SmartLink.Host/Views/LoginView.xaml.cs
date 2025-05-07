using System;
using System.Windows;
using SmartLink.Host.Services;

namespace SmartLink.Host.Views
{
    public partial class LoginView : Window
    {
        private readonly FirebaseService _firebaseService;
        
        public LoginView()
        {
            InitializeComponent();
            
            // Get the Firebase service instance
            _firebaseService = FirebaseService.Instance;
            
            // Load saved email if available
            if (Properties.Settings.Default.RememberEmail)
            {
                txtEmail.Text = Properties.Settings.Default.SavedEmail;
                chkRememberMe.IsChecked = true;
            }
        }
        
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;
            
            // Basic validation
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both email and password.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            try
            {
                // Attempt to sign in
                var userCredential = await _firebaseService.SignInWithEmailAndPasswordAsync(email, password);
                
                // Save email if remember me is checked
                Properties.Settings.Default.RememberEmail = chkRememberMe.IsChecked ?? false;
                Properties.Settings.Default.SavedEmail = Properties.Settings.Default.RememberEmail ? email : string.Empty;
                Properties.Settings.Default.Save();
                
                // Navigate to main application window
                var mainWindow = new MainWindow();
                mainWindow.Show();
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
            createAccountView.EmailSet += (s, email) => txtEmail.Text = email;
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
            
            // TODO: Implement password reset functionality
            MessageBox.Show($"Password reset functionality will be implemented in a future update.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}