using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MaryJane
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Toolbelt.Form1 = this;
        }

        private static List<string> Library { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            MinimumSize = MaximumSize = Size;

            Database.Initialize();

            GetLibrary();

            status.Text = Settings.Instance.TitleDirectory;
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            var item = listBox1.SelectedItem as string;
            var title = Database.Find(item);
            if (item != null)
                Toolbelt.Database.UpdateGame(title.TitleID, Path.Combine(Toolbelt.Settings.TitleDirectory, item));

            listBox1.Enabled = false;
        }

        private void GetLibrary()
        {
            Library = new List<string>(Directory.GetDirectories(Toolbelt.Settings.TitleDirectory));

            foreach (var item in Library)
                if (!string.IsNullOrEmpty(item))
                    ListBoxAddItem(new FileInfo(item).Name);
        }

        private void ListBoxAddItem(object obj)
        {
            if (listBox1.InvokeRequired)
                listBox1.BeginInvoke(new Action(() => { listBox1.Items.Add(obj); }));
            else
                listBox1.Items.Add(obj);
        }

        public void AppendLog(string msg, Color color = default(Color))
        {
            msg += '\n';
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.BeginInvoke(new Action(() =>
                {
                    richTextBox1.AppendText(msg);
                    richTextBox1.ScrollToCaret();
                }));
            }
            else
            {
                richTextBox1.AppendText(msg);
                richTextBox1.ScrollToCaret();
            }
        }

        public void SetStatus(string msg, Color color = default(Color))
        {
            msg += '\n';
            if (status.InvokeRequired)
            {
                status.BeginInvoke(new Action(() =>
                {
                    status.Text = msg;
                    status.ForeColor = color;
                }));
            }
            else
            {
                status.Text = msg;
                status.ForeColor = color;
            }
        }
    }
}