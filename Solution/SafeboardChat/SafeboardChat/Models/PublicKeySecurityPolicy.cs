using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using MessengerCLR.Enums;

namespace SafeboardChat.Models
{
    public class PublicKeySecurityPolicy : IPublicKeySecurityPolicy
    {
        public const encryption_algorithm DEFAULT_ENCRYPTION_ALGORITHM = encryption_algorithm.None;
        public const string DEFAULT_PUBLIC_KEY = "";
        public encryption_algorithm EncryptionAlgorithm { get; }
        public string PublicKey { get; }

        public PublicKeySecurityPolicy(encryption_algorithm? algorithm, string publicKey)
        {
            EncryptionAlgorithm = DEFAULT_ENCRYPTION_ALGORITHM;
            PublicKey = publicKey ?? DEFAULT_PUBLIC_KEY;
        }
    }
}
