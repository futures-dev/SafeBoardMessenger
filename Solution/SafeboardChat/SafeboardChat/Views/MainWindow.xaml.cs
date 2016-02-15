using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MessengerCLR;
using SafeboardChat.ViewModels;

namespace SafeboardChat.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        public MainWindow(MainWindowViewModel viewModel)
        {
            System.Diagnostics.Debug.WriteLine("MainWindow: " + Dispatcher.GetHashCode());
            InitializeComponent();
            DataContext = _viewModel = viewModel;
            ChatChooserFragment.ViewModel = viewModel.ChatChooserViewModel;
            System.Diagnostics.Debug.WriteLine(ChatChooserFragment.ViewModel);
            viewModel.RequestUsersError += (result => MessageBox.Show(result.ToString()));
            System.Diagnostics.Debug.WriteLine("Main Window thread: " + Dispatcher.Thread.GetHashCode());
        }      
        

    }
}
