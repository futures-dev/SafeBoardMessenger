using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessengerCLR.Enums;

namespace SafeboardChat.Models
{
    public interface IChat
    {
        ObservableCollection<IMessage> Messages { get; }
        string Identifier { get; }
        void SendMessage(IMessageContent content );
        void SendSeen(string userId, string messageId);
        int SeenCount { get; }
    }
    
}
