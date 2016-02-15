using System.Windows;
using SafeboardChat.ViewModels;
using SafeboardChat.Models;

namespace SafeboardChat.Views
{
    /// <summary>
    /// Interaction logic for SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        private SettingsViewModel _viewModel;
        public SettingsDialog(SettingsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = _viewModel = viewModel;
        }

        private ISettings _settings;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var a = _viewModel.ServerPort;
            ushort.TryParse(PortTextBox.Text, out a);
            _viewModel.ServerPort = a;
            _viewModel.ServerURL = URLTextBox.Text;
            DialogResult = true;
        }
    }
}
