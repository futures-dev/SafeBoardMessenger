using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessengerCLR;
using MessengerCLR.Enums;

namespace SafeboardChat.Models
{
    public class SafeboardMessageContent : IMessageContent
    {
        public object Data { get; protected set; }
        public message_content_type Type { get; protected set; }
        public bool Encrypted { get; protected set; }

        public SafeboardMessageContent(object data, message_content_type type, bool encrypted)
        {
            switch (type)
            {
                case message_content_type.Text:
                    Data = data as string ?? System.Text.Encoding.UTF8.GetString((data as List<byte>)?.ToArray() ?? new byte[] { });
                    break;
                case message_content_type.Image:
                case message_content_type.Video:
                default:
                    Data = data as byte[] ?? (data as List<byte>)?.ToArray() ?? new byte[] { };
                    break;
            }
            Type = type;
            Encrypted = encrypted;
        }

        public static explicit operator MessageContentCLR(SafeboardMessageContent v)
        {
            MessageContentCLR n = new MessageContentCLR();
            
            n.encrypted = v.Encrypted;
            // TODO: content
            switch (v.Type)
            {
                case message_content_type.Text:
                    n.data = System.Text.Encoding.UTF8.GetBytes((string) v.Data).ToList();
                    n.type = 0;
                    break;
                case message_content_type.Image:
                    n.data = ((byte[])v.Data).ToList();
                    n.type = 1;
                    break;
                case message_content_type.Video:
                    n.data = ((byte[])v.Data).ToList();
                    n.type = 2;
                    break;
                default:
                    n.type = 0;
                    break;
            }
            return n;
        }
    }
}
