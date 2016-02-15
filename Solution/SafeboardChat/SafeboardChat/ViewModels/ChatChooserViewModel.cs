using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MessengerCLR.Enums;
using Prism.Mvvm;
using SafeboardChat.Models;
using SafeboardChat.Views;
using System.Windows.Threading;
using Prism.Commands;

namespace SafeboardChat.ViewModels
{
    public class ChatChooserViewModel : BindableBase
    {
        private ObservableCollection<IChat> _chats;

        public ObservableCollection<IChat> Chats
        {
            get { return _chats; }
            set
            {
                SetProperty(ref _chats, value);
            }
        }

        private ObservableCollection<ChatViewModel> _chatViewModels;
        public ObservableCollection<ChatViewModel> ChatViewModels
        {
            get { return _chatViewModels; }
            set
            {
                SetProperty(ref _chatViewModels, value);
            }
        }

        public ICommand ShowChatCommand { get; private set; }

        public void OnShowChat(ChatViewModel selected)
        {
            if (selected != null)
                ChatFragmentViewModel = selected;
        }

        public ChatChooserViewModel(IMessenger messenger)
        {
            ShowChatCommand = new DelegateCommand<ChatViewModel>(OnShowChat);
            System.Diagnostics.Debug.WriteLine("ChatChooserViewModel: " + Dispatcher.CurrentDispatcher.GetHashCode());
            Chats = messenger.Chats;
            var dispatcher = Dispatcher.CurrentDispatcher;
            Chats.CollectionChanged += (s,e)=>dispatcher.Invoke(()=> Chats_CollectionChanged(s, e));
            ChatViewModels = new ObservableCollection<ChatViewModel>(Chats.Select(chat => new ChatViewModel(chat)));
            this.OnPropertyChanged("ChatViewModels");
        }

        private ChatViewModel _chatFragmentViewModel;
        public ChatViewModel ChatFragmentViewModel
        {
            get { return _chatFragmentViewModel; }
            set { SetProperty(ref _chatFragmentViewModel, value); }
        }
        private void Chats_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (var old in e.OldItems)
                {
                    var oldIChat = old as IChat;
                    if (oldIChat != null)
                    {
                        string id = oldIChat.Identifier;
                        var found = ChatViewModels.FirstOrDefault((c) => c.Identifier == id);
                        if (found != null)
                            ChatViewModels.Remove(found);
                    }
                }
            if (e.NewItems != null)
                foreach (var _new in e.NewItems)
                {
                    var newIChat = _new as IChat;
                    if (newIChat != null)
                    {
                        ChatViewModels.Add(new ChatViewModel(newIChat));
                    }
                }
            this.OnPropertyChanged("ChatViewModels");
        }
    }
}
