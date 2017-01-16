// Project: MapleRoot
// File: Extensions.cs
// Updated By: Jared
// 

using System.Drawing;
using System.Windows.Forms;

namespace MapleRoot.Common
{
    public static class Extensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}