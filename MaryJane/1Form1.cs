using System;
using System.Drawing;
using System.Threading.Tasks;
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

        private async void Form1_Load(object sender, EventArgs e)
        {
            MinimumSize = MaximumSize = Size;
            await Toolbelt.Database.Update();

            foreach (var obj in Database.DbObject)
                if (!string.IsNullOrEmpty(obj.ToString()))
                    listBox1.Items.Add(obj.ToString());
        }

        public async void SetStatus(string msg, Color color = default(Color))
        {
            CheckForIllegalCrossThreadCalls = false;

            await Task.Run(delegate
            {
                status.ForeColor = color;
                status.Text = msg;
            });
        }
    }
}