// Project: MapleRoot
// File: HandleUserList.cs
// Updated By: Jared
// 

#region usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

#endregion

namespace MapleRoot.Network.Messages
{
    public static class HandleUserList
    {
        public static void Init(byte[] buffer, ListBox userList)
        {
            var json = Encoding.UTF8.GetString(buffer);
            var names = JsonConvert.DeserializeObject<List<string>>(json);
            userList.Invoke(new Action(() => { userList.Items.Clear(); }));

            foreach (var name in names) {
                userList.Invoke(new Action(() => {
                    if (!string.IsNullOrEmpty(name))
                        userList.Items.Add(name);
                }));
            }
        }
    }
}