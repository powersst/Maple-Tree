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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.status = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.titleList = new System.Windows.Forms.ListBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.fullTitle = new System.Windows.Forms.CheckBox();
            this.updateBtn = new System.Windows.Forms.Button();
            this.fullScreen = new System.Windows.Forms.CheckBox();
            this.userList = new System.Windows.Forms.ListBox();
            this.chatInput = new System.Windows.Forms.TextBox();
            this.username = new System.Windows.Forms.TextBox();
            this.shareBtn = new System.Windows.Forms.Button();
            this.playBtn = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.chatbox = new System.Windows.Forms.RichTextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.search = new System.Windows.Forms.TextBox();
            this.dataGrid1 = new System.Windows.Forms.DataGridView();
            this.myUploads = new System.Windows.Forms.Button();
            this.connectBtn = new System.Windows.Forms.Button();
            this.sendChat = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 638);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1125, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 0;
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.status.Location = new System.Drawing.Point(9, 664);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(108, 13);
            this.status.TabIndex = 2;
            this.status.Text = "GitHub.com/Tsumes";
            this.status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(12, 622);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1125, 10);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // titleList
            // 
            this.titleList.FormattingEnabled = true;
            this.titleList.Location = new System.Drawing.Point(12, 36);
            this.titleList.Name = "titleList";
            this.titleList.Size = new System.Drawing.Size(250, 550);
            this.titleList.Sorted = true;
            this.titleList.TabIndex = 10;
            this.titleList.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextBox1.DetectUrls = false;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.richTextBox1.Location = new System.Drawing.Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.ShortcutsEnabled = false;
            this.richTextBox1.Size = new System.Drawing.Size(855, 490);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // fullTitle
            // 
            this.fullTitle.AutoSize = true;
            this.fullTitle.Location = new System.Drawing.Point(12, 599);
            this.fullTitle.Name = "fullTitle";
            this.fullTitle.Size = new System.Drawing.Size(126, 17);
            this.fullTitle.TabIndex = 11;
            this.fullTitle.Text = "Download Full Title";
            this.fullTitle.UseVisualStyleBackColor = true;
            this.fullTitle.CheckedChanged += new System.EventHandler(this.fullTitle_CheckedChanged);
            // 
            // updateBtn
            // 
            this.updateBtn.Location = new System.Drawing.Point(101, 7);
            this.updateBtn.Name = "updateBtn";
            this.updateBtn.Size = new System.Drawing.Size(74, 23);
            this.updateBtn.TabIndex = 12;
            this.updateBtn.Text = "Update";
            this.updateBtn.UseVisualStyleBackColor = true;
            this.updateBtn.Click += new System.EventHandler(this.updateBtn_Click);
            // 
            // fullScreen
            // 
            this.fullScreen.AutoSize = true;
            this.fullScreen.Location = new System.Drawing.Point(145, 599);
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
            this.userList.Location = new System.Drawing.Point(1143, 86);
            this.userList.Name = "userList";
            this.userList.Size = new System.Drawing.Size(109, 472);
            this.userList.TabIndex = 14;
            // 
            // chatInput
            // 
            this.chatInput.Location = new System.Drawing.Point(268, 564);
            this.chatInput.Name = "chatInput";
            this.chatInput.Size = new System.Drawing.Size(865, 22);
            this.chatInput.TabIndex = 0;
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(1143, 58);
            this.username.MaxLength = 12;
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(109, 22);
            this.username.TabIndex = 15;
            this.username.TextChanged += new System.EventHandler(this.username_TextChanged);
            // 
            // shareBtn
            // 
            this.shareBtn.Location = new System.Drawing.Point(187, 7);
            this.shareBtn.Name = "shareBtn";
            this.shareBtn.Size = new System.Drawing.Size(75, 23);
            this.shareBtn.TabIndex = 16;
            this.shareBtn.Text = "Share";
            this.shareBtn.UseVisualStyleBackColor = true;
            this.shareBtn.Click += new System.EventHandler(this.shareBtn_Click);
            // 
            // playBtn
            // 
            this.playBtn.Location = new System.Drawing.Point(12, 7);
            this.playBtn.Name = "playBtn";
            this.playBtn.Size = new System.Drawing.Size(75, 23);
            this.playBtn.TabIndex = 17;
            this.playBtn.Text = "Play!";
            this.playBtn.UseVisualStyleBackColor = true;
            this.playBtn.Click += new System.EventHandler(this.playBtn_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(268, 36);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(869, 522);
            this.tabControl1.TabIndex = 18;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.chatbox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(861, 496);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Chat";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // chatbox
            // 
            this.chatbox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.chatbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chatbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatbox.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.chatbox.Location = new System.Drawing.Point(0, 0);
            this.chatbox.Name = "chatbox";
            this.chatbox.ReadOnly = true;
            this.chatbox.Size = new System.Drawing.Size(861, 496);
            this.chatbox.TabIndex = 0;
            this.chatbox.Text = "";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(861, 496);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Console";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.search);
            this.tabPage2.Controls.Add(this.dataGrid1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(861, 496);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Uploads";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(117, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Search by filename";
            // 
            // search
            // 
            this.search.Location = new System.Drawing.Point(6, 6);
            this.search.MaxLength = 12;
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(105, 22);
            this.search.TabIndex = 2;
            this.search.TextChanged += new System.EventHandler(this.search_TextChanged);
            // 
            // dataGrid1
            // 
            this.dataGrid1.AllowUserToDeleteRows = false;
            this.dataGrid1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGrid1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGrid1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGrid1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGrid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid1.Location = new System.Drawing.Point(3, 34);
            this.dataGrid1.MultiSelect = false;
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGrid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid1.ShowCellErrors = false;
            this.dataGrid1.Size = new System.Drawing.Size(855, 483);
            this.dataGrid1.TabIndex = 0;
            this.dataGrid1.DoubleClick += new System.EventHandler(this.dataGrid1_DoubleClick);
            // 
            // myUploads
            // 
            this.myUploads.Location = new System.Drawing.Point(268, 7);
            this.myUploads.Name = "myUploads";
            this.myUploads.Size = new System.Drawing.Size(86, 23);
            this.myUploads.TabIndex = 19;
            this.myUploads.Text = "My Uploads";
            this.myUploads.UseVisualStyleBackColor = true;
            this.myUploads.Click += new System.EventHandler(this.refreshBtn_Click);
            // 
            // connectBtn
            // 
            this.connectBtn.Location = new System.Drawing.Point(1143, 29);
            this.connectBtn.Name = "connectBtn";
            this.connectBtn.Size = new System.Drawing.Size(108, 23);
            this.connectBtn.TabIndex = 20;
            this.connectBtn.Text = "Connect";
            this.connectBtn.UseVisualStyleBackColor = true;
            this.connectBtn.Click += new System.EventHandler(this.connectBtn_Click);
            // 
            // sendChat
            // 
            this.sendChat.Location = new System.Drawing.Point(1143, 564);
            this.sendChat.Name = "sendChat";
            this.sendChat.Size = new System.Drawing.Size(109, 23);
            this.sendChat.TabIndex = 21;
            this.sendChat.Text = "Send";
            this.sendChat.UseVisualStyleBackColor = true;
            this.sendChat.Click += new System.EventHandler(this.sendChat_Click);
            // 
            // Form1
            // 
            this.AcceptButton = this.sendChat;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.sendChat);
            this.Controls.Add(this.myUploads);
            this.Controls.Add(this.connectBtn);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.playBtn);
            this.Controls.Add(this.shareBtn);
            this.Controls.Add(this.username);
            this.Controls.Add(this.chatInput);
            this.Controls.Add(this.userList);
            this.Controls.Add(this.fullScreen);
            this.Controls.Add(this.updateBtn);
            this.Controls.Add(this.fullTitle);
            this.Controls.Add(this.titleList);
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
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.GroupBox groupBox2;
        internal System.Windows.Forms.ListBox titleList;
        internal System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        internal System.Windows.Forms.CheckBox fullTitle;
        private System.Windows.Forms.Button updateBtn;
        private System.Windows.Forms.CheckBox fullScreen;
        private System.Windows.Forms.ListBox userList;
        private System.Windows.Forms.TextBox chatInput;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Button shareBtn;
        private System.Windows.Forms.Button playBtn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button myUploads;
        private System.Windows.Forms.DataGridView dataGrid1;
        private System.Windows.Forms.TextBox search;
        private System.Windows.Forms.Button connectBtn;
        private System.Windows.Forms.Button sendChat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox chatbox;
    }
}

