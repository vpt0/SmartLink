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

        /// <summary>
        /// Initializes a new instance of the DashboardView window, sets the user ID to an empty string, centers the window on the screen, and attaches the Loaded event handler.
        /// </summary>
        public DashboardView()
        {
            _userId = string.Empty;
            InitializeComponent();
            this.Loaded += DashboardView_Loaded;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        /// <summary>
        /// Initializes a new instance of the DashboardView window with the specified user ID.
        /// </summary>
        /// <param name="userId">The identifier of the user associated with this dashboard view.</param>
        public DashboardView(string userId) : this()
        {
            _userId = userId;
        }

        /// <summary>
        /// Ensures the window is visible, activated, and focused when it finishes loading.
        /// </summary>
        private void DashboardView_Loaded(object sender, RoutedEventArgs e)
        {
            // Ensure window is visible
            this.Visibility = Visibility.Visible;
            this.Activate();
            this.Focus();
        }

        /// <summary>
        /// Opens the profile editing popup and sets focus to the profile name input box.
        /// </summary>
        private void OnProfileClick(object sender, RoutedEventArgs e)
        {
            ProfilePopup.IsOpen = true;
            ProfileNameBox.Focus();
        }

        /// <summary>
        /// Handles the event when the profile navigation is clicked by displaying a notification message.
        /// </summary>
        private void OnProfileNavClick(object sender, RoutedEventArgs e)
        {
            // Example: Show profile section or dialog
            MessageBox.Show("Profile navigation clicked.");
        }

        /// <summary>
        /// Handles changes to the CPU cores slider value.
        /// </summary>
        private void OnCPUCoresValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Optionally update UI or perform logic when CPU slider changes
        }

        /// <summary>
        /// Disables the CPU slider when the CPU lock checkbox is checked.
        /// </summary>
        private void OnCPULockChecked(object sender, RoutedEventArgs e)
        {
            // Optionally lock CPU slider
            CPUSlider.IsEnabled = false;
        }
        /// <summary>
        /// Enables the CPU slider when the CPU lock checkbox is unchecked.
        /// </summary>
        private void OnCPULockUnchecked(object sender, RoutedEventArgs e)
        {
            CPUSlider.IsEnabled = true;
        }

        /// <summary>
        /// Handles changes to the RAM slider value.
        /// </summary>
        private void OnRAMValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Optionally update UI or perform logic when RAM slider changes
        }
        /// <summary>
        /// Disables the RAM slider when the RAM lock checkbox is checked.
        /// </summary>
        private void OnRAMLockChecked(object sender, RoutedEventArgs e)
        {
            RAMSlider.IsEnabled = false;
        }
        /// <summary>
        /// Enables the RAM slider when the RAM lock checkbox is unchecked.
        /// </summary>
        private void OnRAMLockUnchecked(object sender, RoutedEventArgs e)
        {
            RAMSlider.IsEnabled = true;
        }
        /// <summary>
        /// Handles the Home button click event by displaying a message indicating home navigation was triggered.
        /// </summary>
        private void OnHomeClick(object sender, RoutedEventArgs e)
        {
            // Example: Show home section or dialog
            MessageBox.Show("Home navigation clicked.");
        }
        /// <summary>
        /// Saves the profile name and email from the popup fields to the main profile fields and closes the profile popup.
        /// </summary>
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

        /// <summary>
        /// Handles changes to the storage slider value.
        /// </summary>
        private void OnStorageValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Optionally update UI or perform logic when Storage slider changes
        }

        /// <summary>
        /// Disables the storage slider when the storage lock checkbox is checked.
        /// </summary>
        private void OnStorageLockChecked(object sender, RoutedEventArgs e)
        {
            StorageSlider.IsEnabled = false;
        }

        /// <summary>
        /// Enables the storage slider when the storage lock checkbox is unchecked.
        /// </summary>
        private void OnStorageLockUnchecked(object sender, RoutedEventArgs e)
        {
            StorageSlider.IsEnabled = true;
        }

        /// <summary>
        /// Generates a shareable link containing the selected CPU, RAM, and storage values, displays it, and shows a connection request popup after a short delay.
        /// </summary>
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
        
        /// <summary>
        /// Handles the termination of the current session and displays a confirmation message to the user.
        /// </summary>
        private void OnTerminateSessionClick(object sender, RoutedEventArgs e)
        {
            // In a real app, this would terminate the active session
            MessageBox.Show("Session terminated!", "SmartLink", MessageBoxButton.OK, MessageBoxImage.Information);
            // Additional cleanup logic would go here
        }

        /// <summary>
        /// Copies the generated link text to the clipboard and displays a confirmation message.
        /// </summary>
        private void OnCopyLinkClick(object sender, RoutedEventArgs e)
        {
            // Copy the link to clipboard
            Clipboard.SetText(GeneratedLinkTextBox.Text);
            MessageBox.Show("Link copied to clipboard!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        /// <summary>
        /// Handles the acceptance of a connection request, displaying a confirmation message and closing the connection request popup.
        /// </summary>
        private void OnAcceptConnectionClick(object sender, RoutedEventArgs e)
        {
            // In a real app, this would establish the connection with the remote user
            MessageBox.Show("Connection accepted!", "SmartLink", MessageBoxButton.OK, MessageBoxImage.Information);
            ConnectionRequestPopup.IsOpen = false;
        }
        
        /// <summary>
        /// Handles the rejection of a connection request by displaying a confirmation message and closing the connection request popup.
        /// </summary>
        private void OnRejectConnectionClick(object sender, RoutedEventArgs e)
        {
            // In a real app, this would reject the connection request
            MessageBox.Show("Connection rejected!", "SmartLink", MessageBoxButton.OK, MessageBoxImage.Information);
            ConnectionRequestPopup.IsOpen = false;
        }
    }
}
