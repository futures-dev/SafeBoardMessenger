using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessengerCLR;
using SafeboardChat.ViewModels;

namespace Safeboard.ViewModels
{
    /*
    class SafeboardChat : Chat
    {
        public SafeboardUser host { get; }
        public Sender sender { get; }
        public SecurityPolicyCLR securityPolicy { get; }
        public SafeboardChat(SafeboardUser a_host, Sender a_sender, SecurityPolicyCLR a_securityPolicy):base(a_sender.Name)
        {
            host = a_host;
            sender = a_sender;
            securityPolicy = a_securityPolicy;
        }
        public override async Task<SafeboardMessage> Send(SafeboardMessage message,object content)
        {
            Fragments.ChatContentFragment fragment = content as Fragments.ChatContentFragment;
            Messages.CollectionChanged -= fragment.UpdateMessages;
            Messages.CollectionChanged += fragment.UpdateMessages;
            Messages.Add(message);
            message.Sender = this.sender;
            var m = await Task.Factory.StartNew(() => host.Send(message));
            m.Sender = host.AsSender;
            Messages[Messages.IndexOf(message)] = m;
            return m;
        }
    }
    */
}
