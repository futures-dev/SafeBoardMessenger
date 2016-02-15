using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessengerCLR.Enums;

namespace SafeboardChat.Models
{
    public interface IPublicKeySecurityPolicy
    {
        encryption_algorithm EncryptionAlgorithm { get; }
        string PublicKey { get; }
    }
}
