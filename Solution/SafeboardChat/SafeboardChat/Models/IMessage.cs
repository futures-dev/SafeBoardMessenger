using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessengerCLR.Enums;

namespace SafeboardChat.Models
{
    public interface IMessage
    {
        event Action<message_status> MessageStatusChanged;
        void OnMessageStatusChanged(message_status status);
        IMessageContent MessageContent { get; }
        string Identifier { get; }
        string SenderIdentifier { get; }
        message_status Status { get; }
        long Time { get; }

    }

    public interface IMessageTime : IMessage
    {
    }
}
