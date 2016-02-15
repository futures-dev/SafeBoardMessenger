using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MessengerCLR;
using MessengerCLR.Enums;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using Timer = System.Timers.Timer;

namespace SafeboardChat.Models
{
    public class SafeboardMessenger : IMessenger
    {
        private const int REQUEST_USERS_DELAY = 5000;
        public ObservableCollection<IChat> Chats { get; private set; }
        public void Disconnect()
        {
            _messenger.Disconnect();
        }
        private ISettings _settings;
        private IPublicKeySecurityPolicy _securityPolicy;
        public SafeboardMessenger(ISettings settings, IPublicKeySecurityPolicy securityPolicy)
        {
            System.Diagnostics.Debug.WriteLine("SafeboardMessenger: " + Dispatcher.CurrentDispatcher.GetHashCode());
            _settings = settings;
            _securityPolicy = securityPolicy;
            Chats = new ObservableCollection<IChat>();
            _messageOwners = new Dictionary<string, string>();
        }

        public event Action<operation_result> LoginCompleted;
        public string UserId
        {
            get; private set;
        }
        private Messenger _messenger;
        public void Login(string userId, string password, params object[] args)
        {
            /*
            IPublicKeySecurityPolicy securityPolicy = null;
            if (args.Length > 0)
            {
                securityPolicy = args[0] as IPublicKeySecurityPolicy;
            }
            if (securityPolicy == null)
            {
                securityPolicy = new PublicKeySecurityPolicy();
            }
            */

            // Port & URL
            _settings = new SafeboardSettings(_settings?.ServerURL, _settings?.ServerPort);
            var sb_settings = _settings as SafeboardSettings;
            _messenger = new Messenger(new MessengerSettingsCLR(sb_settings.ServerURL, sb_settings.ServerPort));
            UserId = userId + "@" + sb_settings.ServerURL;

            // SecurityPolicy
            _securityPolicy = new PublicKeySecurityPolicy(_securityPolicy.EncryptionAlgorithm, _securityPolicy.PublicKey);
            var pub = System.Text.Encoding.UTF8.GetBytes(_securityPolicy.PublicKey).ToList();
            _messenger.Login(UserId, password, new SecurityPolicyCLR((int)_securityPolicy.EncryptionAlgorithm, ref pub), loginCallback);
        }
        private void loginCallback(operation_result operationResult)
        {
            LoginCompleted?.Invoke(operationResult);
            UpdateUsers();
            _messenger.RegisterObserver(messageReceivedCallback, statusChangedCallback);
            var timer = new Timer()
            {
                Interval = REQUEST_USERS_DELAY
            };
            timer.Elapsed += (a, b) => UpdateUsers();
            timer.Start();
        }

        private void messageReceivedCallback(string chatId, MessageCLR message)
        {
            messageReceivedCallback(chatId, chatId, message);
        }

        private void messageReceivedCallback(string chatId, string senderId, MessageCLR message)
        {
            var chat = Chats.FirstOrDefault(q => q.Identifier == chatId);
            if (chat == null)
            {
                System.Diagnostics.Debug.WriteLine("messageReceivedCallback: chat==null");
            }
            else
            {
                
                if (!_messageOwners.ContainsKey(message.identifier))
                    _messageOwners.Add(message.identifier, chatId);
                try {
                    chat.Messages.Add(new SafeboardMessage(message)
                    {
                        SenderIdentifier = senderId
                    });
                }
                catch
                {
                    Thread.Sleep(50);
                    chat.Messages.Add(new SafeboardMessage(message)
                    {
                        SenderIdentifier = senderId
                    });
                }
            }
        }

        private void statusChangedCallback(string messageId, message_status status)
        {
            string messageId_ = messageId;
            var status_ = status;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(10);
                while (true)
                {
                    try
                    {
                        var chatId_ = _messageOwners[messageId_];
                        Chats.FirstOrDefault(q => q.Identifier == chatId_)
                            ?.Messages.FirstOrDefault(q => q.Identifier == messageId_)
                            ?.OnMessageStatusChanged(status_);
                        Debug.WriteLine(status_);
                        break;
                    }
                    catch { }
                }
            });
        }

        public void UpdateUsers()
        {
            _messenger.RequestActiveUsers((a, b) => requestUsersCallback(a, b));
        }

        private void requestUsersCallback(operation_result result, List<UserCLR> users)
        {
            switch (result)
            {
                case operation_result.Ok:
                    foreach (var user in users)
                    {
                        var found = Chats.FirstOrDefault(q => q.Identifier == user.identifier);
                        if (found == null)
                        {
                            System.Diagnostics.Debug.WriteLine("user added");
                            Chats.Add(new SafeboardChat(user, SendMessage, SendMessageSeen));
                        }
                    }
                    foreach (var user in Chats)
                    {
                        var found = users.FirstOrDefault(q => q.identifier == user.Identifier);
                        if (found == null)
                        {
                            System.Diagnostics.Debug.WriteLine("user DELETED");
                            Chats.Remove(user);
                        }
                    }
                    break;
                default:
                    //RequestUsersError?.Invoke(result);
                    System.Diagnostics.Debug.WriteLine("requestUsersCallback error" + result);
                    break;
            }
        }

        private volatile Dictionary<string, string> _messageOwners;
        public void SendMessage(string receiverId, IMessageContent messageContent)
        {
            Task.Factory.StartNew(() =>
            {
                var message = _messenger.SendMessage(receiverId,
                    (MessageContentCLR)(messageContent as SafeboardMessageContent));
                _messageOwners.Add(message.identifier, receiverId);
                messageReceivedCallback(receiverId, UserId, message);
            });

            // _messenger.SendMessage(receiverId, messageContent as MessageContentCLR);
        }


        public void SendMessageSeen(string receiverId, string messageId)
        {
            statusChangedCallback(messageId, message_status.Sent);
            _messenger.SendMessageSeen(receiverId, messageId);
        }
    }
}
