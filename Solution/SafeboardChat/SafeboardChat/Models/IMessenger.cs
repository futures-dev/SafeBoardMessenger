using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessengerCLR.Enums;
using System.Collections.ObjectModel;

namespace SafeboardChat.Models
{
    public interface IMessenger
    {
        event Action<operation_result> LoginCompleted;
        ObservableCollection<IChat> Chats { get; }
        void Disconnect();
        void Login(string userId,string password, params object[] args);
        void SendMessage(string receiverId, IMessageContent messageContent);
        void SendMessageSeen(string receiverId , string messageId);
        string UserId { get; }
    }
}
