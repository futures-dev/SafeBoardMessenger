using System.Windows;
using SafeboardChat.ViewModels;
using MessengerCLR;
using MessengerCLR.Enums;
using Safeboard.ViewModels;
using System;

namespace SafeboardChat.Views
{
    /// <summary>
    /// Interaction logic for LauncherWindow.xaml
    /// </summary>
    public partial class LauncherWindow : Window
    {
        private LauncherViewModel _viewModel;
        private Action<Type> _navigator;
        public LauncherWindow(LauncherViewModel viewModel, Action<Type> navigator)
        {
            System.Diagnostics.Debug.WriteLine("LauncherWindow: " + Dispatcher.GetHashCode());
            InitializeComponent();
            DataContext = _viewModel = viewModel;
            _navigator = navigator;
            _viewModel.LoginCompleted += OnLoginCompleted;
            System.Diagnostics.Debug.WriteLine("LauncherWindow()");
        }

        /*
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //connectingLayout(true);
            
        }
        */

        private void connectingLayout(bool connecting)
        {
            if (connecting)
            {
                ConnectingButton.Visibility = Visibility.Visible;
                ConnectButton.Visibility = Visibility.Hidden;
            }
            else
            {
                ConnectButton.Visibility = Visibility.Visible;
                ConnectingButton.Visibility = Visibility.Hidden;
            }
        }
        private void OnLoginCompleted(operation_result operationResult)
        {
            Dispatcher.Invoke(() =>
            {
                switch (operationResult)
                {
                    case operation_result.Ok:
                        _navigator?.Invoke(typeof(MainWindow));
                        Close();
                        break;
                    default:
                        //connectingLayout(false);
                        MessageBox.Show(operationResult.ToString());
                        break;
                }
            }
        );
        }


        private void SettingsMenu_Click(object sender, RoutedEventArgs e)
        {
            _navigator?.Invoke(typeof(SettingsDialog));
        }

        private void AboutMenu_Click(object sender, RoutedEventArgs e)
        {
            _navigator?.Invoke(typeof(AboutWindow));
        }

        private void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
