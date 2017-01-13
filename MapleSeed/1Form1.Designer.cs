namespace MapleSeed
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.status = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.privateShareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.fullTitle = new System.Windows.Forms.CheckBox();
            this.updateBtn = new System.Windows.Forms.Button();
            this.fullScreen = new System.Windows.Forms.CheckBox();
            this.userList = new System.Windows.Forms.ListBox();
            this.chatInput = new System.Windows.Forms.TextBox();
            this.username = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 424);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(731, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 0;
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.status.Location = new System.Drawing.Point(9, 450);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(108, 13);
            this.status.TabIndex = 2;
            this.status.Text = "GitHub.com/Tsumes";
            this.status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(12, 408);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(731, 10);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // listBox1
            // 
            this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 9);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(250, 355);
            this.listBox1.Sorted = true;
            this.listBox1.TabIndex = 10;
            this.listBox1.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.privateShareToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(129, 32);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(125, 6);
            // 
            // privateShareToolStripMenuItem
            // 
            this.privateShareToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shareToolStripMenuItem,
            this.uploadToolStripMenuItem});
            this.privateShareToolStripMenuItem.Name = "privateShareToolStripMenuItem";
            this.privateShareToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.privateShareToolStripMenuItem.Text = "Cached Shader";
            // 
            // shareToolStripMenuItem
            // 
            this.shareToolStripMenuItem.Name = "shareToolStripMenuItem";
            this.shareToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.shareToolStripMenuItem.Text = "Share";
            this.shareToolStripMenuItem.Click += new System.EventHandler(this.shareToolStripMenuItem_Click);
            // 
            // uploadToolStripMenuItem
            // 
            this.uploadToolStripMenuItem.Name = "uploadToolStripMenuItem";
            this.uploadToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.uploadToolStripMenuItem.Text = "Upload";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextBox1.DetectUrls = false;
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.richTextBox1.Location = new System.Drawing.Point(268, 9);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.ShortcutsEnabled = false;
            this.richTextBox1.Size = new System.Drawing.Size(475, 355);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // fullTitle
            // 
            this.fullTitle.AutoSize = true;
            this.fullTitle.Location = new System.Drawing.Point(12, 375);
            this.fullTitle.Name = "fullTitle";
            this.fullTitle.Size = new System.Drawing.Size(126, 17);
            this.fullTitle.TabIndex = 11;
            this.fullTitle.Text = "Download Full Title";
            this.fullTitle.UseVisualStyleBackColor = true;
            this.fullTitle.CheckedChanged += new System.EventHandler(this.fullTitle_CheckedChanged);
            // 
            // updateBtn
            // 
            this.updateBtn.Location = new System.Drawing.Point(749, 424);
            this.updateBtn.Name = "updateBtn";
            this.updateBtn.Size = new System.Drawing.Size(109, 23);
            this.updateBtn.TabIndex = 12;
            this.updateBtn.Text = "Update";
            this.updateBtn.UseVisualStyleBackColor = true;
            this.updateBtn.Click += new System.EventHandler(this.updateBtn_Click);
            // 
            // fullScreen
            // 
            this.fullScreen.AutoSize = true;
            this.fullScreen.Location = new System.Drawing.Point(145, 375);
            this.fullScreen.Name = "fullScreen";
            this.fullScreen.Size = new System.Drawing.Size(115, 17);
            this.fullScreen.TabIndex = 13;
            this.fullScreen.Text = "Full Screen Mode";
            this.fullScreen.UseVisualStyleBackColor = true;
            this.fullScreen.CheckedChanged += new System.EventHandler(this.fullScreen_CheckedChanged);
            // 
            // userList
            // 
            this.userList.FormattingEnabled = true;
            this.userList.Location = new System.Drawing.Point(749, 9);
            this.userList.Name = "userList";
            this.userList.Size = new System.Drawing.Size(109, 355);
            this.userList.TabIndex = 14;
            // 
            // chatInput
            // 
            this.chatInput.Location = new System.Drawing.Point(268, 370);
            this.chatInput.Name = "chatInput";
            this.chatInput.Size = new System.Drawing.Size(475, 22);
            this.chatInput.TabIndex = 0;
            this.chatInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.chatInput_KeyPress);
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(749, 370);
            this.username.MaxLength = 12;
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(108, 22);
            this.username.TabIndex = 15;
            this.username.TextChanged += new System.EventHandler(this.username_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 472);
            this.Controls.Add(this.username);
            this.Controls.Add(this.chatInput);
            this.Controls.Add(this.userList);
            this.Controls.Add(this.fullScreen);
            this.Controls.Add(this.updateBtn);
            this.Controls.Add(this.fullTitle);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.status);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Maple Seed";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.GroupBox groupBox2;
        internal System.Windows.Forms.ListBox listBox1;
        internal System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        internal System.Windows.Forms.CheckBox fullTitle;
        private System.Windows.Forms.Button updateBtn;
        private System.Windows.Forms.CheckBox fullScreen;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem privateShareToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem shareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadToolStripMenuItem;
        private System.Windows.Forms.ListBox userList;
        private System.Windows.Forms.TextBox chatInput;
        private System.Windows.Forms.TextBox username;
    }
}

