namespace Read4Me
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
            this.tbspeech = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSpeak = new System.Windows.Forms.Button();
            this.btnToWAV = new System.Windows.Forms.Button();
            this.cmbVoices = new System.Windows.Forms.ComboBox();
            this.tbarRate = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnPause = new System.Windows.Forms.Button();
            this.trbVolume = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.mynotifyicon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bBrowse = new System.Windows.Forms.Button();
            this.bBatch = new System.Windows.Forms.Button();
            this.sStrip = new System.Windows.Forms.StatusStrip();
            this.sWorkingStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbAlbum = new System.Windows.Forms.TextBox();
            this.tbArtist = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbEmptyLines = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbAfter = new System.Windows.Forms.TextBox();
            this.tbBefore = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tbSource = new System.Windows.Forms.TextBox();
            this.bSource = new System.Windows.Forms.Button();
            this.bGo = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.tbYear = new System.Windows.Forms.TextBox();
            this.bExit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tbarRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbVolume)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.sStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbspeech
            // 
            this.tbspeech.Location = new System.Drawing.Point(12, 43);
            this.tbspeech.Multiline = true;
            this.tbspeech.Name = "tbspeech";
            this.tbspeech.Size = new System.Drawing.Size(213, 104);
            this.tbspeech.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Please Enter the text below to speak! ";
            // 
            // btnSpeak
            // 
            this.btnSpeak.Location = new System.Drawing.Point(89, 163);
            this.btnSpeak.Name = "btnSpeak";
            this.btnSpeak.Size = new System.Drawing.Size(75, 23);
            this.btnSpeak.TabIndex = 22;
            this.btnSpeak.Text = "Speak";
            this.btnSpeak.UseVisualStyleBackColor = true;
            this.btnSpeak.Click += new System.EventHandler(this.btnSpeak_Click);
            // 
            // btnToWAV
            // 
            this.btnToWAV.Location = new System.Drawing.Point(8, 163);
            this.btnToWAV.Name = "btnToWAV";
            this.btnToWAV.Size = new System.Drawing.Size(75, 23);
            this.btnToWAV.TabIndex = 21;
            this.btnToWAV.Text = "To Wav";
            this.btnToWAV.UseVisualStyleBackColor = true;
            this.btnToWAV.Click += new System.EventHandler(this.btnToWAV_Click);
            // 
            // cmbVoices
            // 
            this.cmbVoices.FormattingEnabled = true;
            this.cmbVoices.Location = new System.Drawing.Point(304, 46);
            this.cmbVoices.Name = "cmbVoices";
            this.cmbVoices.Size = new System.Drawing.Size(121, 21);
            this.cmbVoices.TabIndex = 25;
            this.cmbVoices.SelectedIndexChanged += new System.EventHandler(this.cmbVoices_SelectedIndexChanged);
            // 
            // tbarRate
            // 
            this.tbarRate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbarRate.Location = new System.Drawing.Point(253, 43);
            this.tbarRate.Minimum = -10;
            this.tbarRate.Name = "tbarRate";
            this.tbarRate.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbarRate.Size = new System.Drawing.Size(45, 104);
            this.tbarRate.TabIndex = 24;
            this.tbarRate.Scroll += new System.EventHandler(this.tbarRate_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(228, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = " 10";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(228, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "- 10";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(253, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Rate:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(301, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Select Person:";
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(170, 163);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(75, 23);
            this.btnPause.TabIndex = 23;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // trbVolume
            // 
            this.trbVolume.Cursor = System.Windows.Forms.Cursors.Hand;
            this.trbVolume.LargeChange = 20;
            this.trbVolume.Location = new System.Drawing.Point(446, 43);
            this.trbVolume.Maximum = 100;
            this.trbVolume.Name = "trbVolume";
            this.trbVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trbVolume.Size = new System.Drawing.Size(45, 104);
            this.trbVolume.SmallChange = 10;
            this.trbVolume.TabIndex = 26;
            this.trbVolume.TickFrequency = 5;
            this.trbVolume.Scroll += new System.EventHandler(this.trbVolume_Scroll);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(469, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = " 100";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(480, 124);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(443, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 30;
            this.label8.Text = "Volume:";
            // 
            // mynotifyicon
            // 
            this.mynotifyicon.ContextMenuStrip = this.contextMenuStrip1;
            this.mynotifyicon.Icon = ((System.Drawing.Icon)(resources.GetObject("mynotifyicon.Icon")));
            this.mynotifyicon.Text = "TTS";
            this.mynotifyicon.Visible = true;
            this.mynotifyicon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mynotifyicon_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showStripMenuItem,
            this.exitStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(104, 48);
            // 
            // showStripMenuItem
            // 
            this.showStripMenuItem.Name = "showStripMenuItem";
            this.showStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.showStripMenuItem.Text = "Show";
            this.showStripMenuItem.Click += new System.EventHandler(this.showStripMenuItem_Click);
            // 
            // exitStripMenuItem
            // 
            this.exitStripMenuItem.Name = "exitStripMenuItem";
            this.exitStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitStripMenuItem.Text = "Exit";
            this.exitStripMenuItem.Click += new System.EventHandler(this.exitStripMenuItem_Click);
            // 
            // bBrowse
            // 
            this.bBrowse.Location = new System.Drawing.Point(416, 315);
            this.bBrowse.Name = "bBrowse";
            this.bBrowse.Size = new System.Drawing.Size(75, 23);
            this.bBrowse.TabIndex = 10;
            this.bBrowse.Text = "Browse";
            this.bBrowse.UseVisualStyleBackColor = true;
            this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
            // 
            // bBatch
            // 
            this.bBatch.Location = new System.Drawing.Point(416, 341);
            this.bBatch.Name = "bBatch";
            this.bBatch.Size = new System.Drawing.Size(75, 23);
            this.bBatch.TabIndex = 11;
            this.bBatch.Text = "Batch";
            this.bBatch.UseVisualStyleBackColor = true;
            this.bBatch.Click += new System.EventHandler(this.bBatch_Click);
            // 
            // sStrip
            // 
            this.sStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sWorkingStatus});
            this.sStrip.Location = new System.Drawing.Point(0, 412);
            this.sStrip.Name = "sStrip";
            this.sStrip.Size = new System.Drawing.Size(511, 22);
            this.sStrip.TabIndex = 19;
            // 
            // sWorkingStatus
            // 
            this.sWorkingStatus.Name = "sWorkingStatus";
            this.sWorkingStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // tbAlbum
            // 
            this.tbAlbum.Location = new System.Drawing.Point(64, 327);
            this.tbAlbum.Name = "tbAlbum";
            this.tbAlbum.Size = new System.Drawing.Size(100, 20);
            this.tbAlbum.TabIndex = 4;
            // 
            // tbArtist
            // 
            this.tbArtist.Location = new System.Drawing.Point(64, 353);
            this.tbArtist.Name = "tbArtist";
            this.tbArtist.Size = new System.Drawing.Size(100, 20);
            this.tbArtist.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 304);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 13);
            this.label9.TabIndex = 43;
            this.label9.Text = "Empty lines:";
            // 
            // tbEmptyLines
            // 
            this.tbEmptyLines.Location = new System.Drawing.Point(86, 301);
            this.tbEmptyLines.Name = "tbEmptyLines";
            this.tbEmptyLines.Size = new System.Drawing.Size(26, 20);
            this.tbEmptyLines.TabIndex = 3;
            this.tbEmptyLines.Text = "2";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(17, 278);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 13);
            this.label10.TabIndex = 42;
            this.label10.Text = "After:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 252);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 13);
            this.label11.TabIndex = 41;
            this.label11.Text = "Before:";
            // 
            // tbAfter
            // 
            this.tbAfter.Location = new System.Drawing.Point(64, 275);
            this.tbAfter.Name = "tbAfter";
            this.tbAfter.Size = new System.Drawing.Size(100, 20);
            this.tbAfter.TabIndex = 2;
            // 
            // tbBefore
            // 
            this.tbBefore.Location = new System.Drawing.Point(64, 249);
            this.tbBefore.Name = "tbBefore";
            this.tbBefore.Size = new System.Drawing.Size(100, 20);
            this.tbBefore.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 330);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(39, 13);
            this.label12.TabIndex = 44;
            this.label12.Text = "Album:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(17, 356);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(33, 13);
            this.label13.TabIndex = 45;
            this.label13.Text = "Artist:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(17, 226);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 13);
            this.label14.TabIndex = 40;
            this.label14.Text = "Source File (txt)";
            // 
            // tbSource
            // 
            this.tbSource.Location = new System.Drawing.Point(103, 223);
            this.tbSource.Name = "tbSource";
            this.tbSource.Size = new System.Drawing.Size(195, 20);
            this.tbSource.TabIndex = 8;
            this.tbSource.Text = "C:\\Users\\yassendobrev\\Desktop";
            // 
            // bSource
            // 
            this.bSource.Location = new System.Drawing.Point(304, 221);
            this.bSource.Name = "bSource";
            this.bSource.Size = new System.Drawing.Size(85, 23);
            this.bSource.TabIndex = 0;
            this.bSource.Text = "Browse";
            this.bSource.UseVisualStyleBackColor = true;
            this.bSource.Click += new System.EventHandler(this.bSource_Click);
            // 
            // bGo
            // 
            this.bGo.Location = new System.Drawing.Point(304, 246);
            this.bGo.Name = "bGo";
            this.bGo.Size = new System.Drawing.Size(85, 23);
            this.bGo.TabIndex = 7;
            this.bGo.Text = "Go!";
            this.bGo.UseVisualStyleBackColor = true;
            this.bGo.Click += new System.EventHandler(this.bGo_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(17, 382);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 13);
            this.label15.TabIndex = 46;
            this.label15.Text = "Year:";
            // 
            // tbYear
            // 
            this.tbYear.Location = new System.Drawing.Point(64, 379);
            this.tbYear.Name = "tbYear";
            this.tbYear.Size = new System.Drawing.Size(100, 20);
            this.tbYear.TabIndex = 6;
            // 
            // bExit
            // 
            this.bExit.Location = new System.Drawing.Point(416, 370);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(75, 23);
            this.bExit.TabIndex = 47;
            this.bExit.Text = "Exit";
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 434);
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.tbYear);
            this.Controls.Add(this.bGo);
            this.Controls.Add(this.bSource);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.tbSource);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbEmptyLines);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tbAfter);
            this.Controls.Add(this.tbBefore);
            this.Controls.Add(this.tbArtist);
            this.Controls.Add(this.tbAlbum);
            this.Controls.Add(this.sStrip);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.bBrowse);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.bBatch);
            this.Controls.Add(this.trbVolume);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbarRate);
            this.Controls.Add(this.cmbVoices);
            this.Controls.Add(this.btnToWAV);
            this.Controls.Add(this.btnSpeak);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbspeech);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Read4Me";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.tbarRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbVolume)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.sStrip.ResumeLayout(false);
            this.sStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbspeech;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSpeak;
        private System.Windows.Forms.Button btnToWAV;
        private System.Windows.Forms.ComboBox cmbVoices;
        private System.Windows.Forms.TrackBar tbarRate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.TrackBar trbVolume;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NotifyIcon mynotifyicon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitStripMenuItem;
        private System.Windows.Forms.Button bBrowse;
        private System.Windows.Forms.Button bBatch;
        private System.Windows.Forms.StatusStrip sStrip;
        private System.Windows.Forms.ToolStripStatusLabel sWorkingStatus;
        private System.Windows.Forms.TextBox tbAlbum;
        private System.Windows.Forms.TextBox tbArtist;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbEmptyLines;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbAfter;
        private System.Windows.Forms.TextBox tbBefore;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbSource;
        private System.Windows.Forms.Button bSource;
        private System.Windows.Forms.Button bGo;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbYear;
        private System.Windows.Forms.Button bExit;
    }
}

