using MessengerCLR;
using MessengerCLR.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeboardChat.Models
{
    class SafeboardMessage : IMessage,IMessageTime
    {
        public event Action<message_status> MessageStatusChanged;

        public void OnMessageStatusChanged(message_status status)
        {
            Status = status;
            MessageStatusChanged?.Invoke(status);
        }
        public IMessageContent MessageContent { get; private set; }
        public string Identifier { get; private set; }
        public string SenderIdentifier { get;  set; }
        public long Time { get; private set; }
        public message_status Status { get; private set; }
        public SafeboardMessage(MessageCLR message)
        {
            Identifier = message.identifier;
            MessageContent = new SafeboardMessageContent(message.content.data, (message_content_type)message.content.type, message.content.encrypted);
            Time = message.time;
            Status = message_status.Sending;
        }
    }
}
