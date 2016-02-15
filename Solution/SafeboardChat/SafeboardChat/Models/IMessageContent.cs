using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using MessengerCLR.Enums;

namespace SafeboardChat.Models
{
    public interface IMessageContent
    {
        object Data { get; }
        message_content_type Type { get; }
        bool Encrypted { get; }
    }
    
}
