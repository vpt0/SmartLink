<<<<<<< HEAD
using System.Windows;
using System.Windows.Input;
=======
using System;
using System.Windows;
using SmartLink.Host.Services;
>>>>>>> 0238eb2 (Update: Login system, error handling, and window management improvements)

namespace SmartLink.Host.Views
{
    public partial class LoginView : Window
    {
<<<<<<< HEAD
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
=======
        private readonly FirebaseService _firebaseService;

        public LoginView()
        {
            InitializeComponent();
            _firebaseService = FirebaseService.Instance;
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoginButton.IsEnabled = false;

                string email = EmailTextBox.Text;
                string password = PasswordBox.Password;

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Please enter both email and password", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    var userCredential = await _firebaseService.SignInWithEmailAndPasswordAsync(email, password);

                    var dashboard = new DashboardView(userCredential.User.Uid);
                    Application.Current.MainWindow = dashboard;
                    dashboard.Show();

                    this.Close();
                }
                catch (Firebase.Auth.FirebaseAuthException)
                {
                    MessageBox.Show("Incorrect email or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected error occurred. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoginButton.IsEnabled = true;
            }
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            var createAccountView = new CreateAccountView();
            createAccountView.EmailSet += (sender, email) =>
            {
                EmailTextBox.Text = email;
                PasswordBox.Focus();
            };

            bool? result = createAccountView.ShowDialog();

            if (result == true)
            {
                PasswordBox.Focus();
            }
>>>>>>> 0238eb2 (Update: Login system, error handling, and window management improvements)
        }
    }
}