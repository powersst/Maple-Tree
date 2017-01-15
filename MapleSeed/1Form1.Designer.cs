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
            this.fullTitle = new MaterialSkin.Controls.MaterialCheckBox();
            this.updateBtn = new MaterialSkin.Controls.MaterialFlatButton();
            this.fullScreen = new MaterialSkin.Controls.MaterialCheckBox();
            this.userList = new System.Windows.Forms.ListBox();
            this.chatInput = new System.Windows.Forms.TextBox();
            this.username = new System.Windows.Forms.TextBox();
            this.shareBtn = new MaterialSkin.Controls.MaterialFlatButton();
            this.playBtn = new MaterialSkin.Controls.MaterialFlatButton();
            this.tabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.search = new System.Windows.Forms.TextBox();
            this.dataGrid1 = new System.Windows.Forms.DataGridView();
            this.myUploads = new MaterialSkin.Controls.MaterialFlatButton();
            this.connectBtn = new MaterialSkin.Controls.MaterialFlatButton();
            this.sendChat = new MaterialSkin.Controls.MaterialFlatButton();
            this.materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            this.tabControl1.SuspendLayout();
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
            this.titleList.Location = new System.Drawing.Point(12, 101);
            this.titleList.Name = "titleList";
            this.titleList.Size = new System.Drawing.Size(250, 485);
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
            this.richTextBox1.Size = new System.Drawing.Size(855, 397);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // fullTitle
            // 
            this.fullTitle.AutoSize = true;
            this.fullTitle.Depth = 0;
            this.fullTitle.Font = new System.Drawing.Font("Roboto", 10F);
            this.fullTitle.Location = new System.Drawing.Point(12, 599);
            this.fullTitle.Margin = new System.Windows.Forms.Padding(0);
            this.fullTitle.MouseLocation = new System.Drawing.Point(-1, -1);
            this.fullTitle.MouseState = MaterialSkin.MouseState.HOVER;
            this.fullTitle.Name = "fullTitle";
            this.fullTitle.Ripple = true;
            this.fullTitle.Size = new System.Drawing.Size(147, 30);
            this.fullTitle.TabIndex = 11;
            this.fullTitle.Text = "Download Full Title";
            this.fullTitle.UseVisualStyleBackColor = true;
            this.fullTitle.CheckedChanged += new System.EventHandler(this.fullTitle_CheckedChanged);
            // 
            // updateBtn
            // 
            this.updateBtn.AutoSize = true;
            this.updateBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.updateBtn.Depth = 0;
            this.updateBtn.Icon = global::MapleSeed.Properties.Resources.updatebtn;
            this.updateBtn.Location = new System.Drawing.Point(208, 40);
            this.updateBtn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.updateBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.updateBtn.Name = "updateBtn";
            this.updateBtn.Primary = false;
            this.updateBtn.Size = new System.Drawing.Size(103, 46);
            this.updateBtn.TabIndex = 12;
            this.updateBtn.Text = "Update";
            this.updateBtn.UseVisualStyleBackColor = true;
            this.updateBtn.Click += new System.EventHandler(this.updateBtn_Click);
            // 
            // fullScreen
            // 
            this.fullScreen.AutoSize = true;
            this.fullScreen.Depth = 0;
            this.fullScreen.Font = new System.Drawing.Font("Roboto", 10F);
            this.fullScreen.Location = new System.Drawing.Point(168, 599);
            this.fullScreen.Margin = new System.Windows.Forms.Padding(0);
            this.fullScreen.MouseLocation = new System.Drawing.Point(-1, -1);
            this.fullScreen.MouseState = MaterialSkin.MouseState.HOVER;
            this.fullScreen.Name = "fullScreen";
            this.fullScreen.Ripple = true;
            this.fullScreen.Size = new System.Drawing.Size(137, 30);
            this.fullScreen.TabIndex = 13;
            this.fullScreen.Text = "Full Screen Mode";
            this.fullScreen.UseVisualStyleBackColor = true;
            this.fullScreen.CheckedChanged += new System.EventHandler(this.fullScreen_CheckedChanged);
            // 
            // userList
            // 
            this.userList.FormattingEnabled = true;
            this.userList.Location = new System.Drawing.Point(1143, 125);
            this.userList.Name = "userList";
            this.userList.Size = new System.Drawing.Size(109, 433);
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
            this.username.Location = new System.Drawing.Point(1143, 96);
            this.username.MaxLength = 12;
            this.username.Name = "username";
            this.username.ReadOnly = true;
            this.username.Size = new System.Drawing.Size(109, 22);
            this.username.TabIndex = 15;
            this.username.TextChanged += new System.EventHandler(this.username_TextChanged);
            // 
            // shareBtn
            // 
            this.shareBtn.AutoSize = true;
            this.shareBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.shareBtn.Depth = 0;
            this.shareBtn.Icon = global::MapleSeed.Properties.Resources.sharebtn;
            this.shareBtn.Location = new System.Drawing.Point(106, 40);
            this.shareBtn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.shareBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.shareBtn.Name = "shareBtn";
            this.shareBtn.Primary = false;
            this.shareBtn.Size = new System.Drawing.Size(94, 46);
            this.shareBtn.TabIndex = 16;
            this.shareBtn.Text = "Share";
            this.shareBtn.UseVisualStyleBackColor = true;
            this.shareBtn.Click += new System.EventHandler(this.shareBtn_Click);
            // 
            // playBtn
            // 
            this.playBtn.AutoSize = true;
            this.playBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.playBtn.BackColor = System.Drawing.Color.Red;
            this.playBtn.Depth = 0;
            this.playBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.playBtn.Icon = global::MapleSeed.Properties.Resources.icon1;
            this.playBtn.Location = new System.Drawing.Point(13, 40);
            this.playBtn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.playBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.playBtn.Name = "playBtn";
            this.playBtn.Primary = false;
            this.playBtn.Size = new System.Drawing.Size(85, 46);
            this.playBtn.TabIndex = 17;
            this.playBtn.Text = "Play";
            this.playBtn.UseVisualStyleBackColor = false;
            this.playBtn.Click += new System.EventHandler(this.playBtn_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Depth = 0;
            this.tabControl1.Location = new System.Drawing.Point(268, 129);
            this.tabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(869, 429);
            this.tabControl1.TabIndex = 18;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(861, 403);
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
            this.tabPage2.Size = new System.Drawing.Size(861, 403);
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
            this.myUploads.AutoSize = true;
            this.myUploads.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.myUploads.Depth = 0;
            this.myUploads.Icon = global::MapleSeed.Properties.Resources.uploadbtn;
            this.myUploads.Location = new System.Drawing.Point(319, 40);
            this.myUploads.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.myUploads.MouseState = MaterialSkin.MouseState.HOVER;
            this.myUploads.Name = "myUploads";
            this.myUploads.Primary = false;
            this.myUploads.Size = new System.Drawing.Size(136, 46);
            this.myUploads.TabIndex = 19;
            this.myUploads.Text = "My Uploads";
            this.myUploads.UseVisualStyleBackColor = true;
            this.myUploads.Click += new System.EventHandler(this.refreshBtn_Click);
            // 
            // connectBtn
            // 
            this.connectBtn.AutoSize = true;
            this.connectBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.connectBtn.Depth = 0;
            this.connectBtn.Icon = null;
            this.connectBtn.Location = new System.Drawing.Point(1143, 625);
            this.connectBtn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.connectBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.connectBtn.Name = "connectBtn";
            this.connectBtn.Primary = false;
            this.connectBtn.Size = new System.Drawing.Size(84, 36);
            this.connectBtn.TabIndex = 20;
            this.connectBtn.Text = "Connect";
            this.connectBtn.UseVisualStyleBackColor = true;
            this.connectBtn.Click += new System.EventHandler(this.connectBtn_Click);
            // 
            // sendChat
            // 
            this.sendChat.AutoSize = true;
            this.sendChat.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.sendChat.Depth = 0;
            this.sendChat.Icon = null;
            this.sendChat.Location = new System.Drawing.Point(1143, 564);
            this.sendChat.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.sendChat.MouseState = MaterialSkin.MouseState.HOVER;
            this.sendChat.Name = "sendChat";
            this.sendChat.Primary = false;
            this.sendChat.Size = new System.Drawing.Size(56, 36);
            this.sendChat.TabIndex = 21;
            this.sendChat.Text = "Send";
            this.sendChat.UseVisualStyleBackColor = true;
            this.sendChat.Click += new System.EventHandler(this.sendChat_Click);
            // 
            // materialTabSelector1
            // 
            this.materialTabSelector1.BaseTabControl = this.tabControl1;
            this.materialTabSelector1.Depth = 0;
            this.materialTabSelector1.Location = new System.Drawing.Point(269, 90);
            this.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabSelector1.Name = "materialTabSelector1";
            this.materialTabSelector1.Size = new System.Drawing.Size(868, 36);
            this.materialTabSelector1.TabIndex = 22;
            this.materialTabSelector1.Text = "materialTabSelector1";
            // 
            // Form1
            // 
            this.AcceptButton = this.sendChat;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.materialTabSelector1);
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
        internal MaterialSkin.Controls.MaterialCheckBox fullTitle;
        private MaterialSkin.Controls.MaterialFlatButton updateBtn;
        private MaterialSkin.Controls.MaterialCheckBox fullScreen;
        private System.Windows.Forms.ListBox userList;
        private System.Windows.Forms.TextBox chatInput;
        private System.Windows.Forms.TextBox username;
        private MaterialSkin.Controls.MaterialFlatButton shareBtn;
        private MaterialSkin.Controls.MaterialFlatButton playBtn;
        private MaterialSkin.Controls.MaterialTabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private MaterialSkin.Controls.MaterialFlatButton myUploads;
        private System.Windows.Forms.DataGridView dataGrid1;
        private System.Windows.Forms.TextBox search;
        private MaterialSkin.Controls.MaterialFlatButton connectBtn;
        private MaterialSkin.Controls.MaterialFlatButton sendChat;
        private System.Windows.Forms.Label label1;
        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector1;
    }
}

