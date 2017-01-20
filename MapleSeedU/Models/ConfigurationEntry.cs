// Project: MapleSeedU
// File: ConfigurationEntry.cs
// Updated By: Jared
// 

namespace MapleSeedU.Models
{
    public class ConfigurationEntry : RegistryKeyEntry
    {
        private readonly string _keyName;

        public ConfigurationEntry(string _keyname)
        {
            _keyName = _keyname;
        }

        public string Value {
            get { return Read(_keyName); }
            set { Write(_keyName, value); }
        }
    }
}