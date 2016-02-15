using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using MessengerCLR.Enums;
using MessengerCLR;

namespace SafeboardChat.Models
{
    public class SafeboardChat : IChat, IPublicKeySecurityPolicy
    {
        public ObservableCollection<IMessage> Messages { get; private set; }
        public string Identifier { get; private set; }
        private Action<string, IMessageContent> _sendMessageAction;
        private Action<string, string> _sendSeenAction;
        public void SendMessage(IMessageContent content)
        {
            _sendMessageAction?.Invoke(Identifier, content);
        }

        public void SendSeen(string userId, string messageId)
        {
            _sendSeenAction?.Invoke(userId/*==Identifier*/, messageId);
        }

        public int SeenCount { get; }

        public encryption_algorithm EncryptionAlgorithm { get; protected set; }

        public string PublicKey { get; protected set; }

        public SafeboardChat(UserCLR userCLR, Action<string, IMessageContent> sendMessageAction, Action<string, string> sendSeenAction)
        {
            System.Diagnostics.Debug.WriteLine("SafeboardChat: " + Dispatcher.CurrentDispatcher.GetHashCode());
            Identifier = userCLR.identifier;
            EncryptionAlgorithm = (encryption_algorithm)userCLR.securityPolicy.encryptionAlgo;
            PublicKey = System.Text.Encoding.UTF8.GetString(userCLR.securityPolicy.encryptionPubKey.ToArray());
            Messages = new ObservableCollection<IMessage>();
            _sendMessageAction = sendMessageAction;
            _sendSeenAction = sendSeenAction;
        }

        public override bool Equals(object obj)
        {
            var asChat = obj as IChat;
            if (asChat != null)
            {
                return Identifier.Equals(asChat.Identifier);
            }
            var asUserCLR = obj as UserCLR;
            if (asUserCLR != null)
            {
                return Identifier.Equals(asUserCLR.identifier);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }
        
    }
}
