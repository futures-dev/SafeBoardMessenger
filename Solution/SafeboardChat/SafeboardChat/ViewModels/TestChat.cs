using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SafeboardChat.ViewModels;

namespace Safeboard.ViewModels
{
    /*
    class TestChat : Chat
    {
        /*
        public override Task<SafeboardMessage> Send(SafeboardMessage message)
        {
            Messages.Add(message);
            return Task.Factory.StartNew(()=>send(message));
        }
        

        public override async Task<SafeboardMessage> Send(SafeboardMessage message, object args = null)
        {
            Messages.Add(message);
            var m = await Task.Factory.StartNew(() => send(message));
            Messages[Messages.IndexOf(message)] = m;
            return m;
        }

        static Random ran = new Random();
        private SafeboardMessage send(SafeboardMessage message)
        {
            Thread.Sleep(ran.Next(5,15)*1000);
            message.MessageText += "Sent :)";
            return message;
        }

        public TestChat(string name) : base(name)
        {
            Sender sender = new Sender("Cool sender "+name);
            Send(new SafeboardMessage("o-la-la",sender));
            Send(new SafeboardMessage(name + " ",MainWindow.CurrentUser.AsSender));
            Send(new SafeboardMessage("Hello, maaan!!!! How've you been doing??? I am from chat",sender));
        }
    }
*/
}
