using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Safeboard.ViewModels;
using SafeboardChat.Models;
using MessengerCLR.Enums;
using System;
using System.Windows.Threading;

namespace SafeboardChat.ViewModels
{
    public class LauncherViewModel : BindableBase
    {
        public event Action<operation_result> LoginCompleted;

        private SettingsViewModel currentSettings = new SettingsViewModel();
        public static Sender CurrentUser { get; set; }

        public SettingsViewModel CurrentSettings
        {
            get { return currentSettings; }
            set
            {
                if (value != null)
                    currentSettings = value;
            }
        }

        private string _login;

        public string Login
        {
            get
            {
                return _login;
            }
            set { SetProperty(ref _login, value); }
        }

        private string _password;

        public string Password
        {
            get { return _password;}
            set { SetProperty(ref _password, value); }
        }

        private IMessenger _messenger;

        public LauncherViewModel(IMessenger messenger )
        {
            System.Diagnostics.Debug.WriteLine("LauncherViewModel: " + Dispatcher.CurrentDispatcher.GetHashCode());
            _messenger = messenger;
            _messenger.LoginCompleted += OnLoginCompleted;
            ConnectCommand = new DelegateCommand(OnConnect,CanConnect);
        }

        public ICommand ConnectCommand { get; private set; }

        public void OnConnect()
        {
            if (_canConnect)
            {
                _canConnect = false;
                _messenger.Login(Login??"", Password??"");
            }
        }
        private bool _canConnect = true;
        public bool CanConnect()
        {
            return _canConnect;
        }

        public void OnLoginCompleted(operation_result result)
        {
            LoginCompleted?.Invoke(result);
            _canConnect = true;
        }
    }
}
