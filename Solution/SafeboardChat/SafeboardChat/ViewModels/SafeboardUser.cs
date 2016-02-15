using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessengerCLR;
using SafeboardChat.ViewModels;

namespace Safeboard.ViewModels
{
    /*
    class SafeboardUser : IUser
    {
        public event Action<int> LoginEvent;
        private int SERVER_PORT = 5222;
        private string SERVER_URL = "127.0.0.1";
        public Sender AsSender => _sender;

        public ObservableCollection<Chat> Chats { get; } = new ObservableCollection<Chat>();

        public string Username { get; }
        private Sender _sender;
        private SafeboardMessenger messenger;
        private MessengerSettingsCLR settings;
        private System.Windows.Threading.Dispatcher dispatcher;
        public bool LoggedIn { get; set; } = false;
        private string pass;
        private Dictionary<string, Tuple<SafeboardMessage, Chat>> dictionary = new Dictionary<string, Tuple<SafeboardMessage, Chat>>();
        ~SafeboardUser()
        {
            messenger.Disconnect();
        }
        public void LoadChats()
        {
            if (LoggedIn)
            {
                messenger.RequestActiveUsers(
                           (resultCode, usersList) =>
                           {
                               Debug.WriteLine("Users requested, resultcode = " + resultCode);
                               foreach (var user in usersList)
                                   if (Chats.Count((Chat ch) => ch.Name == user.identifier) == 0)
                                   {
                                       dispatcher.Invoke(() =>
                                   {
                                       Chats.Add(new SafeboardChat(this, new Sender(user.identifier), user.securityPolicy));
                                   });
                                   }
                           });
            }
            else
            {
                messenger.Login(Username, pass, new SecurityPolicyCLR(), (a) =>
                {
                    LoginEvent?.Invoke(a);
                    LoggedIn = true;
                    LoadChats();

                    messenger.RegisterObserver(
                        (senderName, message) =>
                        {
                            Debug.WriteLine("SafeboardMessage Received");
                            var chat = Chats.First(ch => ch.Name == senderName);
                            // time and seen
                            var m = new SafeboardMessage(Encoding.UTF8.GetString(message.content.data.ToArray()), new Sender(senderName), false, message.time);
                            if (dictionary.ContainsKey(message.identifier))
                            {
                                dictionary[message.identifier] = new Tuple<SafeboardMessage, Chat>(dictionary[message.identifier].Item1, chat);
                            }
                            dispatcher.Invoke(() => chat.Messages.Add(m));
                            messenger.SendMessageSeen(senderName, message.identifier);
                        },
                        (str, integer) =>
                        {
                            if (dictionary.ContainsKey(str))
                            {
                                dictionary[str].Item1.Seen = true;
                                dispatcher.Invoke(() => {
                                    try {
                                        foreach (var ch in Chats)
                                            ch.Messages[0] = ch.Messages[0];                                        
                                    }
                                    catch { }
                                });
                            }
                            Debug.WriteLine("StatusReceived");
                        });
                });
            }
        }
        public SafeboardUser(string username, string password, System.Windows.Threading.Dispatcher dispatcher, string url = "127.0.0.1", int port = 5222)
        {
            SERVER_URL = url;
            SERVER_PORT = port;
            this.dispatcher = dispatcher;
            settings = new MessengerSettingsCLR();
            settings.serverPort = (int)SERVER_PORT;
            settings.serverURL = SERVER_URL;
            messenger = new SafeboardMessenger(settings);

            Username = username + "@" + settings.serverURL;
            pass = password;
            _sender = new Sender(Username);
        }

        public SafeboardMessage Send(SafeboardMessage message)
        {
            Debug.WriteLine("Send");
            var content = new MessageContentCLR();
            content.data = Encoding.UTF8.GetBytes(message.MessageText).ToList();
            System.Threading.Thread.Sleep(2000);
            var messageCLR = messenger.SendMessage(message.Sender.Name, content);
            if (!dictionary.ContainsKey(messageCLR.identifier))
            {
                dictionary.Add(messageCLR.identifier, new Tuple<SafeboardMessage, Chat>(message, null));
            }
            return message;
        }
    }
    */
}
