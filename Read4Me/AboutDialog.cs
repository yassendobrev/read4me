using System;
using System.Windows.Forms;

namespace Read4Me
{
    public partial class AboutDialog : Form
    {
        private Label label24;
        private GroupBox gbAbout;
        private GroupBox gbThanks;
        private Label lAbout1;
        private LinkLabel lLinkDiscussion;
        private LinkLabel lLinkIDSharp;
        private LinkLabel linkMP3Compressor;
        private LinkLabel linkLAME;
        private GroupBox gbDisclaimer;
        private Label lDisclaimer;
        private LinkLabel linkHotkey;
        private LinkLabel linkSAPI;
        private Button bOK;
        private Button bDonate;
        private GroupBox groupBox1;
        private LinkLabel linkLanguageDetection;

        string local_version;
    
        public AboutDialog(string version)
        {
            local_version = version;
            InitializeComponent();
            lAbout1.Text = "Read4Me Clipboard Reader and Text to mp3 Converter v" + local_version + "\n© 2011-" + DateTime.Now.Year.ToString() + " Yassen Dobr" +
                "ev\nLicensed under GNU GPLv3 or later";

            lLinkDiscussion.Links.Add(0, lLinkDiscussion.Text.Length, lLinkDiscussion.Text);
            lLinkIDSharp.Links.Add(0, lLinkIDSharp.Text.Length, lLinkIDSharp.Text);
            linkMP3Compressor.Links.Add(0, linkMP3Compressor.Text.Length, linkMP3Compressor.Text);
            linkLAME.Links.Add(0, linkLAME.Text.Length, linkLAME.Text);
            linkHotkey.Links.Add(0, linkHotkey.Text.Length, linkHotkey.Text);
            linkSAPI.Links.Add(0, linkSAPI.Text.Length, linkSAPI.Text);
            linkLanguageDetection.Links.Add(0, linkLanguageDetection.Text.Length, linkLanguageDetection.Text);
        }

        // Hide form when ESC key pressed
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (msg.WParam.ToInt32() == (int)Keys.Escape)
            {
                this.Close();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            this.bOK = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.lLinkDiscussion = new System.Windows.Forms.LinkLabel();
            this.gbAbout = new System.Windows.Forms.GroupBox();
            this.lAbout1 = new System.Windows.Forms.Label();
            this.gbThanks = new System.Windows.Forms.GroupBox();
            this.linkSAPI = new System.Windows.Forms.LinkLabel();
            this.linkHotkey = new System.Windows.Forms.LinkLabel();
            this.linkMP3Compressor = new System.Windows.Forms.LinkLabel();
            this.linkLAME = new System.Windows.Forms.LinkLabel();
            this.lLinkIDSharp = new System.Windows.Forms.LinkLabel();
            this.gbDisclaimer = new System.Windows.Forms.GroupBox();
            this.lDisclaimer = new System.Windows.Forms.Label();
            this.bDonate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.linkLanguageDetection = new System.Windows.Forms.LinkLabel();
            this.gbAbout.SuspendLayout();
            this.gbThanks.SuspendLayout();
            this.gbDisclaimer.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bOK
            // 
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bOK.Location = new System.Drawing.Point(161, 353);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 0;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(6, 70);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(164, 13);
            this.label24.TabIndex = 55;
            this.label24.Text = "Bug reports and feature requests:";
            // 
            // lLinkDiscussion
            // 
            this.lLinkDiscussion.AutoSize = true;
            this.lLinkDiscussion.Location = new System.Drawing.Point(6, 83);
            this.lLinkDiscussion.Name = "lLinkDiscussion";
            this.lLinkDiscussion.Size = new System.Drawing.Size(248, 13);
            this.lLinkDiscussion.TabIndex = 54;
            this.lLinkDiscussion.TabStop = true;
            this.lLinkDiscussion.Text = "https://sourceforge.net/p/read4mecbr/discussion/";
            this.lLinkDiscussion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lLinkDiscussion_LinkClicked);
            // 
            // gbAbout
            // 
            this.gbAbout.Controls.Add(this.lAbout1);
            this.gbAbout.Controls.Add(this.label24);
            this.gbAbout.Controls.Add(this.lLinkDiscussion);
            this.gbAbout.Location = new System.Drawing.Point(12, 12);
            this.gbAbout.Name = "gbAbout";
            this.gbAbout.Size = new System.Drawing.Size(361, 104);
            this.gbAbout.TabIndex = 56;
            this.gbAbout.TabStop = false;
            this.gbAbout.Text = "About";
            // 
            // lAbout1
            // 
            this.lAbout1.AutoSize = true;
            this.lAbout1.Location = new System.Drawing.Point(9, 16);
            this.lAbout1.Name = "lAbout1";
            this.lAbout1.Size = new System.Drawing.Size(0, 13);
            this.lAbout1.TabIndex = 56;
            // 
            // gbThanks
            // 
            this.gbThanks.Controls.Add(this.linkLanguageDetection);
            this.gbThanks.Controls.Add(this.linkSAPI);
            this.gbThanks.Controls.Add(this.linkHotkey);
            this.gbThanks.Controls.Add(this.linkMP3Compressor);
            this.gbThanks.Controls.Add(this.linkLAME);
            this.gbThanks.Controls.Add(this.lLinkIDSharp);
            this.gbThanks.Location = new System.Drawing.Point(12, 185);
            this.gbThanks.Name = "gbThanks";
            this.gbThanks.Size = new System.Drawing.Size(361, 106);
            this.gbThanks.TabIndex = 57;
            this.gbThanks.TabStop = false;
            this.gbThanks.Text = "Third-party software used";
            // 
            // linkSAPI
            // 
            this.linkSAPI.AutoSize = true;
            this.linkSAPI.Location = new System.Drawing.Point(6, 68);
            this.linkSAPI.Name = "linkSAPI";
            this.linkSAPI.Size = new System.Drawing.Size(350, 13);
            this.linkSAPI.TabIndex = 58;
            this.linkSAPI.TabStop = true;
            this.linkSAPI.Text = "http://www.codeproject.com/KB/audio-video/TTSFeaturesOfSAPI.aspx";
            this.linkSAPI.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkSAPI_LinkClicked);
            // 
            // linkHotkey
            // 
            this.linkHotkey.AutoSize = true;
            this.linkHotkey.Location = new System.Drawing.Point(6, 55);
            this.linkHotkey.Name = "linkHotkey";
            this.linkHotkey.Size = new System.Drawing.Size(340, 13);
            this.linkHotkey.TabIndex = 57;
            this.linkHotkey.TabStop = true;
            this.linkHotkey.Text = "http://bloggablea.wordpress.com/2007/05/01/global-hotkeys-with-net";
            this.linkHotkey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkHotkey_LinkClicked);
            // 
            // linkMP3Compressor
            // 
            this.linkMP3Compressor.AutoSize = true;
            this.linkMP3Compressor.Location = new System.Drawing.Point(6, 42);
            this.linkMP3Compressor.Name = "linkMP3Compressor";
            this.linkMP3Compressor.Size = new System.Drawing.Size(330, 13);
            this.linkMP3Compressor.TabIndex = 56;
            this.linkMP3Compressor.TabStop = true;
            this.linkMP3Compressor.Text = "http://www.codeproject.com/KB/audio-video/MP3Compressor.aspx";
            this.linkMP3Compressor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkMP3Compressor_LinkClicked);
            // 
            // linkLAME
            // 
            this.linkLAME.AutoSize = true;
            this.linkLAME.Location = new System.Drawing.Point(6, 29);
            this.linkLAME.Name = "linkLAME";
            this.linkLAME.Size = new System.Drawing.Size(137, 13);
            this.linkLAME.TabIndex = 55;
            this.linkLAME.TabStop = true;
            this.linkLAME.Text = "http://lame.sourceforge.net";
            this.linkLAME.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLAME_LinkClicked);
            // 
            // lLinkIDSharp
            // 
            this.lLinkIDSharp.AutoSize = true;
            this.lLinkIDSharp.Location = new System.Drawing.Point(6, 16);
            this.lLinkIDSharp.Name = "lLinkIDSharp";
            this.lLinkIDSharp.Size = new System.Drawing.Size(229, 13);
            this.lLinkIDSharp.TabIndex = 54;
            this.lLinkIDSharp.TabStop = true;
            this.lLinkIDSharp.Text = "http://www.idsharp.com/products/tagging.php";
            this.lLinkIDSharp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lLinkIDSharp_LinkClicked);
            // 
            // linkLanguageDetection
            // 
            this.linkLanguageDetection.AutoSize = true;
            this.linkLanguageDetection.Location = new System.Drawing.Point(6, 81);
            this.linkLanguageDetection.Name = "linkLanguageDetection";
            this.linkLanguageDetection.Size = new System.Drawing.Size(261, 13);
            this.linkLanguageDetection.TabIndex = 59;
            this.linkLanguageDetection.TabStop = true;
            this.linkLanguageDetection.Text = "http://idsyst.hu/development/language_detector.html";
            this.linkLanguageDetection.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLanguageDetection_LinkClicked);
            // 
            // gbDisclaimer
            // 
            this.gbDisclaimer.Controls.Add(this.lDisclaimer);
            this.gbDisclaimer.Location = new System.Drawing.Point(12, 122);
            this.gbDisclaimer.Name = "gbDisclaimer";
            this.gbDisclaimer.Size = new System.Drawing.Size(361, 57);
            this.gbDisclaimer.TabIndex = 58;
            this.gbDisclaimer.TabStop = false;
            this.gbDisclaimer.Text = "Disclaimer";
            // 
            // lDisclaimer
            // 
            this.lDisclaimer.AutoSize = true;
            this.lDisclaimer.Location = new System.Drawing.Point(9, 20);
            this.lDisclaimer.Name = "lDisclaimer";
            this.lDisclaimer.Size = new System.Drawing.Size(203, 26);
            this.lDisclaimer.TabIndex = 0;
            this.lDisclaimer.Text = "The author shall NOT be held responsible\nfor any illegal use of this program.";
            // 
            // bDonate
            // 
            this.bDonate.Image = ((System.Drawing.Image)(resources.GetObject("bDonate.Image")));
            this.bDonate.Location = new System.Drawing.Point(136, 19);
            this.bDonate.Name = "bDonate";
            this.bDonate.Size = new System.Drawing.Size(92, 26);
            this.bDonate.TabIndex = 59;
            this.bDonate.UseVisualStyleBackColor = true;
            this.bDonate.Click += new System.EventHandler(this.bDonate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bDonate);
            this.groupBox1.Location = new System.Drawing.Point(14, 297);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(361, 50);
            this.groupBox1.TabIndex = 59;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Support the development of Read4Me TTS Clipboard Reader";
            // 
            // AboutDialog
            // 
            this.CancelButton = this.bOK;
            this.ClientSize = new System.Drawing.Size(387, 387);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbDisclaimer);
            this.Controls.Add(this.gbThanks);
            this.Controls.Add(this.gbAbout);
            this.Controls.Add(this.bOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.gbAbout.ResumeLayout(false);
            this.gbAbout.PerformLayout();
            this.gbThanks.ResumeLayout(false);
            this.gbThanks.PerformLayout();
            this.gbDisclaimer.ResumeLayout(false);
            this.gbDisclaimer.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void bOK_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void lLinkDiscussion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void lLinkIDSharp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void linkLAME_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void linkMP3Compressor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void linkHotkey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void linkSAPI_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void linkLanguageDetection_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void bDonate_Click(object sender, System.EventArgs e)
        {
            // http://www.gorancic.com/blog/net/c-paypal-donate-button
            string url = "";

            string button_ID = "DL7L79SGNURAL";

            url += "https://www.paypal.com/cgi-bin/webscr" +
                "?cmd=" + "_s-xclick" +
                "&hosted_button_id=" + button_ID;

            System.Diagnostics.Process.Start(url);
        }
    }
}
