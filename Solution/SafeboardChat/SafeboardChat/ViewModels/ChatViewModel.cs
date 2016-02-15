using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using MessengerCLR;
using MessengerCLR.Enums;
using SafeboardChat.Models;
using System.Windows.Input;
using Prism.Commands;

namespace SafeboardChat.ViewModels
{
    public class ChatViewModel : BindableBase
    {
        private IChat _chat;

        public ChatViewModel(IChat chat)
        {
            _chat = chat;
            SendMessageCommand = new DelegateCommand<object>(OnSendMessage, CanSendMessage);
            SendSeenCommand = new DelegateCommand<object>(OnSendSeen, CanSendSeen);
            chat.Messages.CollectionChanged += (e, args) => OnPropertyChanged("Messages");
            chat.Messages.CollectionChanged += (e, args) => OnPropertyChanged("Seen");
        }

        public string Identifier => _chat.Identifier;
        public bool Seen => Messages.FirstOrDefault(m => m.Sending && m.Sender==Identifier) != null;

        public IEnumerable<MessageViewModel> Messages => _chat.Messages.Select(message => new MessageViewModel(message));

        public ICommand SendMessageCommand { get; private set; }

        public void OnSendMessage(object arg)
        {
            var arr = arg as object[];
            if (_canSendMessage)
            {
                _canSendMessage = false;
                // TODO: content
                _chat.SendMessage(new SafeboardMessageContent(arr[0], arr[1] as message_content_type? ?? message_content_type.Text, false));
                _canSendMessage = true;
            }
        }
        private bool _canSendMessage = true;
        public bool CanSendMessage(object arg)
        {
            return _canSendMessage;
        }

        public ICommand SendSeenCommand { get; private set; }

        public void OnSendSeen(object arg)
        {
            if (_canSendSeen)
            {
                _canSendSeen = false;
                // TODO: content
                var message = arg as MessageViewModel;
                if (message.Sending)
                {
                    _chat.SendSeen(_chat.Identifier, message.Identifier);
                }
                _canSendSeen = true;
                OnPropertyChanged("Seen");
            }
        }
        private bool _canSendSeen = true;
        public bool CanSendSeen(object arg)
        {
            return _canSendSeen;
        }

        /*
        public override bool Equals(object obj)
        {
            var other = obj as ChatViewModel;
            if (other != null)
                return _name.Equals(other.Name);
            var otherCLR = obj as UserCLR;
            if (otherCLR != null)
                return _name.Equals(otherCLR.identifier);
            return false;
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }*/

    }
}
