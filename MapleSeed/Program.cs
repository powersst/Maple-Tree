// Project: MapleSeed
// File: Program.cs
// Updated By: Jared
// 

#region usings

using System;
using System.Windows.Forms;

#endregion

namespace MapleSeed
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}