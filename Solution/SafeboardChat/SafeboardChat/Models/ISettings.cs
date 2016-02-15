using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeboardChat.Models
{
    public interface ISettings
    {
        ushort ServerPort { get; set; }
        string ServerURL { get; set; }
    }
}
