using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace SafeboardChat.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        const string SERVER_URL = "127.0.0.1";
        const ushort SERVER_PORT = 5222;

        private string _serverURL = SERVER_URL;
        private ushort _serverPort = SERVER_PORT;

        public string ServerURL
        {
            get { return _serverURL; }
            set { SetProperty(ref _serverURL, value); }
        }

        public ushort ServerPort
        {
            get { return _serverPort; }
            set { SetProperty(ref _serverPort, value); }
        }
    }
}
