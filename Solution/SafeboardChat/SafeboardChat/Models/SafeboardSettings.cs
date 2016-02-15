using MessengerCLR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeboardChat.Models
{
    public class SafeboardSettings : ISettings
    {
        public const ushort DEFAULT_SERVER_PORT = 5222;
        public const string DEFAULT_SERVER_URL = "127.0.0.1";
        public ushort ServerPort { get; set; }
        public string ServerURL { get; set; }
        public SafeboardSettings(string url, ushort? port)
        {
            ServerURL = url ?? DEFAULT_SERVER_URL;
            ServerPort = port ?? DEFAULT_SERVER_PORT;
        }
    }
}
