// Project: MapleSeedU
// File: RegistryKeyEntry.cs
// Updated By: Jared
// 

#region usings

using System;
using System.Windows;
using Microsoft.Win32;

#endregion

namespace MapleSeedU.Models
{
    public class RegistryKeyEntry
    {
        private readonly RegistryKey _baseRegistryKey = Registry.CurrentUser;
        private readonly string _subKey = "SOFTWARE\\MapleSeed";
        private bool showError = true;

        public string Read(string keyName)
        {
            var rk = _baseRegistryKey;
            var sk1 = rk.OpenSubKey(_subKey);
            if (sk1 == null) return null;
            try {
                return (string) sk1.GetValue(keyName.ToUpper());
            }
            catch (Exception e) {
                ShowErrorMessage(e, "Reading registry " + keyName.ToUpper());
                return null;
            }
        }

        public bool Write(string keyName, object value)
        {
            try {
                var rk = _baseRegistryKey;
                var sk1 = rk.CreateSubKey(_subKey);
                sk1?.SetValue(keyName.ToUpper(), value);

                return true;
            }
            catch (Exception e) {
                ShowErrorMessage(e, "Writing registry " + keyName.ToUpper());
                return false;
            }
        }

        public bool DeleteKey(string keyName)
        {
            try {
                var rk = _baseRegistryKey;
                var sk1 = rk.CreateSubKey(_subKey);
                sk1?.DeleteValue(keyName);

                return true;
            }
            catch (Exception e) {
                ShowErrorMessage(e, "Deleting SubKey " + _subKey);
                return false;
            }
        }

        public bool DeleteSubKeyTree()
        {
            try {
                var rk = _baseRegistryKey;
                var sk1 = rk.OpenSubKey(_subKey);
                if (sk1 != null)
                    rk.DeleteSubKeyTree(_subKey);

                return true;
            }
            catch (Exception e) {
                ShowErrorMessage(e, "Deleting SubKey " + _subKey);
                return false;
            }
        }

        public int SubKeyCount()
        {
            try {
                var rk = _baseRegistryKey;
                var sk1 = rk.OpenSubKey(_subKey);
                return sk1?.SubKeyCount ?? 0;
            }
            catch (Exception e) {
                ShowErrorMessage(e, "Retriving subkeys of " + _subKey);
                return 0;
            }
        }

        public int ValueCount()
        {
            try {
                var rk = _baseRegistryKey;
                var sk1 = rk.OpenSubKey(_subKey);
                return sk1?.ValueCount ?? 0;
            }
            catch (Exception e) {
                ShowErrorMessage(e, "Retriving keys of " + _subKey);
                return 0;
            }
        }

        private void ShowErrorMessage(Exception e, string title)
        {
            if (showError)
                MessageBox.Show(e.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}