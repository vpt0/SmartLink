using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Windows.Data;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

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

        private void OnProfileClick(object sender, RoutedEventArgs e)
        {
            ProfilePopup.IsOpen = true;
            ProfileNameBox.Focus();
        }

        private void OnProfileNavClick(object sender, RoutedEventArgs e)
        {
            // Example: Show profile section or dialog
            MessageBox.Show("Profile navigation clicked.");
        }

        private void OnCPUCoresValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Optionally update UI or perform logic when CPU slider changes
        }

        private void OnCPULockChecked(object sender, RoutedEventArgs e)
        {
            // Optionally lock CPU slider
            CPUSlider.IsEnabled = false;
        }
        private void OnCPULockUnchecked(object sender, RoutedEventArgs e)
        {
            CPUSlider.IsEnabled = true;
        }

        private void OnRAMValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Optionally update UI or perform logic when RAM slider changes
        }
        private void OnRAMLockChecked(object sender, RoutedEventArgs e)
        {
            RAMSlider.IsEnabled = false;
        }
        private void OnRAMLockUnchecked(object sender, RoutedEventArgs e)
        {
            RAMSlider.IsEnabled = true;
        }
        private void OnHomeClick(object sender, RoutedEventArgs e)
        {
            // Example: Show home section or dialog
            MessageBox.Show("Home navigation clicked.");
        }
        private void OnProfileSaveClick(object sender, RoutedEventArgs e)
        {
            string name = PopupProfileNameBox.Text;
            string email = PopupProfileEmailBox.Text;
            // Update the main profile fields with the popup values
            ProfileNameBox.Text = name;
            ProfileEmailBox.Text = email;
            // Optionally save or process the profile info here
            ProfilePopup.IsOpen = false;
        }

        private void OnStorageValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Optionally update UI or perform logic when Storage slider changes
        }

        private void OnStorageLockChecked(object sender, RoutedEventArgs e)
        {
            StorageSlider.IsEnabled = false;
        }

        private void OnStorageLockUnchecked(object sender, RoutedEventArgs e)
        {
            StorageSlider.IsEnabled = true;
        }

        private void OnGenerateLinkClick(object sender, RoutedEventArgs e)
        {
            // Generate a link based on selected resources
            int cpuCores = (int)CPUSlider.Value;
            int ramGB = (int)RAMSlider.Value;
            int storageGB = (int)StorageSlider.Value;
            
            // Create a simple link format (this could be more sophisticated in a real app)
            string link = $"https://smartlink.example.com/share?cpu={cpuCores}&ram={ramGB}&storage={storageGB}&id={Guid.NewGuid().ToString().Substring(0, 8)}";
            
            // Display the generated link
            GeneratedLinkTextBox.Text = link;
            LinkDisplayBorder.Visibility = Visibility.Visible;
            
            // Simulate a connection request after a short delay (in a real app, this would come from a server)
            System.Threading.Tasks.Task.Delay(3000).ContinueWith(_ => {
                Dispatcher.Invoke(() => {
                    // Show the connection request popup
                    ConnectionRequestPopup.IsOpen = true;
                });
            });
        }
        
        private void OnTerminateSessionClick(object sender, RoutedEventArgs e)
        {
            // In a real app, this would terminate the active session
            MessageBox.Show("Session terminated!", "SmartLink", MessageBoxButton.OK, MessageBoxImage.Information);
            // Additional cleanup logic would go here
        }

        private void OnCopyLinkClick(object sender, RoutedEventArgs e)
        {
            // Copy the link to clipboard
            Clipboard.SetText(GeneratedLinkTextBox.Text);
            MessageBox.Show("Link copied to clipboard!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private void OnAcceptConnectionClick(object sender, RoutedEventArgs e)
        {
            // In a real app, this would establish the connection with the remote user
            MessageBox.Show("Connection accepted!", "SmartLink", MessageBoxButton.OK, MessageBoxImage.Information);
            ConnectionRequestPopup.IsOpen = false;
        }
        
        private void OnRejectConnectionClick(object sender, RoutedEventArgs e)
        {
            // In a real app, this would reject the connection request
            MessageBox.Show("Connection rejected!", "SmartLink", MessageBoxButton.OK, MessageBoxImage.Information);
            ConnectionRequestPopup.IsOpen = false;
        }
    }
}
