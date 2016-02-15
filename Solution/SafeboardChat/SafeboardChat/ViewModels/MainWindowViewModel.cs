using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using MessengerCLR;
using MessengerCLR.Enums;
using Prism.Mvvm;
using Safeboard.ViewModels;

namespace SafeboardChat.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public event Action<operation_result> RequestUsersError;

        public ChatChooserViewModel ChatChooserViewModel
        {
            get;
            private set;
        }

        private ChatViewModel chatFragmentViewModel;
        public ChatViewModel ChatFragmentViewModel
        {
            get { return chatFragmentViewModel; }
            set { SetProperty(ref chatFragmentViewModel, value); }
        }

        public MainWindowViewModel(ChatChooserViewModel chatChooserViewModel, string title = "")
        {
            System.Diagnostics.Debug.WriteLine("MainWindowViewModel: " + Dispatcher.CurrentDispatcher.GetHashCode());
            ChatChooserViewModel = chatChooserViewModel;
            Title = "Safeboard Chat " + title;
        }
        public string Title{get;private set;}
        /*
        */
    }
}
