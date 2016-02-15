using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeboardChat.ViewModels
{
    public class Sender
    {
        public string Name { get; }

        public Sender(string name)
        {
            Name = name;
        }

        private static Sender _default = new Sender("");
        public static Sender Default => _default;

        public override bool Equals(object obj)
        {
            var otherSender = obj as Sender;
            if (otherSender == null)
                return false;
            if (otherSender.Name == null && Name == null)
                return true;
            if (otherSender.Name == null || Name == null)
                return false;
            return Name.Equals(otherSender.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
