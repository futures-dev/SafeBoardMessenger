using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Windows.Documents;
using MessengerCLR;
using MessengerCLR.Enums;
using Prism.Mvvm;
using Safeboard.ViewModels;
using SafeboardChat.Models;

namespace SafeboardChat.ViewModels
{
    public class MessageViewModel : BindableBase
    {
        private IMessage _message;

        public string Identifier => _message?.Identifier;

        public string Content
        {
            get
            {
                string s = "";
                switch (_message.MessageContent.Type)
                {
                        case message_content_type.Text:
                        return _message?.MessageContent.Data as string;
                    case message_content_type.Image:
                    default:
                        try
                        {
                            File.WriteAllBytes(s = (Path.GetTempFileName()),
                                (byte[]) _message.MessageContent.Data);
                        }
                        catch
                        {
                            Debug.WriteLine("Could not write bytes");
                        }
                        break;
                    case message_content_type.Video:
                        try
                        {
                            File.WriteAllBytes(s = (Path.GetTempPath()+ _message.GetHashCode() + DateTime.Now.Ticks + ".mp4"),
                                (byte[])_message.MessageContent.Data);
                        }
                        catch
                        {
                            Debug.WriteLine("Could not write bytes");
                        }
                        break;
                }
                return s;
            }
        }

        public string Sender => _message?.SenderIdentifier;
        public message_content_type ContentType => _message?.MessageContent.Type ?? 0;

        private DateTime _time;

        public DateTime Time
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
        }
        public bool Seen => _message?.Status == message_status.Seen;
        public bool Delivered => _message?.Status == message_status.Delivered;
        public bool Failed => _message?.Status == message_status.FailedToSend;
        public bool Sent => _message?.Status == message_status.Sent;
        public bool Sending => _message?.Status == message_status.Sending;
        public MessageViewModel(IMessage message)
        {
            if (message != null)
            {
                _message = message;
                message.MessageStatusChanged += MessageStatusChangedHandler;
                Time = new DateTime(1970, 1, 1).AddSeconds(message.Time);
                MessageStatusChangedHandler(message.Status);
            }
            else
            {
                Debug.WriteLine("WHY??");
            }
        }

        private void MessageStatusChangedHandler(message_status status)
        {
            /*
            switch (status)
            {
                case message_status.FailedToSend:
                    Failed = true;
                    break;
                case message_status.Delivered:
                    Delivered = true;
                    Time = DateTime.Now;
                    break;
                case message_status.Seen:
                    Seen = true;
                    break;
                case message_status.Sent:
                case message_status.Sending:
                default:
                    break;
            }
            */
            OnPropertyChanged("Seen");
            OnPropertyChanged("Delivered");
            OnPropertyChanged("Failed");
            OnPropertyChanged("Sent");
            OnPropertyChanged("Sending");
        }
    }
}
