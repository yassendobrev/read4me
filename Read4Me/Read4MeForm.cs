using System.Windows.Forms;
using SpeechLib;
using System.Threading;
using System.IO;
using System.Collections;
using System;
using System.Speech.Synthesis;

namespace Read4Me
{
    public partial class Read4MeForm : Form
    {
        // some required members 
        static SpVoice TTSVoiceClipboard = new SpVoice();
        static SpeechSynthesizer TTSVoiceClipboardSynth = new SpeechSynthesizer();

        int SpeechRateGlobal = 4; // Ranges from -10 to 10 
        int VolumeGlobal = 80; // Range from 0 to 100.
        bool PausedGlobal = false;
        SpObjectToken SpeechVoiceGlobal;

        static string textTTS;
        static int posTTS;
        static string toRead;

        // program version
        string LocalVersion = "0.5.5";

        SortedList ligatures = new SortedList();
        PressedKeys pKeys;

        // show/hide main window
        bool mAllowVisible;     // ContextMenu's Show command used
        bool MinToTray = false; // minimize program or hide program
        bool ReadSelectedText = false; // read selected text or read clipboard

        MenuItem mi;
        ContextMenu cm;

        IntPtr _ClipboardViewerNext;

        public Read4MeForm()
        {
            InitializeComponent();
            this.Text = this.Text + LocalVersion;

            lLinkDiscussion.Links.Add(0, lLinkDiscussion.Text.Length, lLinkDiscussion.Text);
            lLinkEspeak.Links.Add(0, lLinkEspeak.Text.Length, lLinkEspeak.Text);

            // http://www.jonasjohn.de/snippets/csharp/drag-and-drop-example.htm
            // Enable drag and drop for this form
            // (this can also be applied to any controls)
            this.AllowDrop = true;

            // Add event handlers for the drag & drop functionality
            this.DragEnter += new DragEventHandler(Form_DragEnter);
            this.DragDrop += new DragEventHandler(Form_DragDrop);

            // Add empty value
            cmbVoices.Items.Add("");
            cbVoiceBatch.Items.Add("");
            cbLang1.Items.Add("");
            cbLang2.Items.Add("");
            cbLang3.Items.Add("");
            cbLang4.Items.Add("");
            cbLang5.Items.Add("");
            cbLang6.Items.Add("");

            // Add language detection
            cmbVoices.Items.Add("Detect language");
            cbLang1.Items.Add("Detect language");
            cbLang2.Items.Add("Detect language");
            cbLang3.Items.Add("Detect language");
            cbLang4.Items.Add("Detect language");
            cbLang5.Items.Add("Detect language");
            cbLang6.Items.Add("Detect language");

            // add cut copy paste menu to textbox
            cm = new ContextMenu();
            mi = new System.Windows.Forms.MenuItem("Cut");
            mi.Click += new EventHandler(mi_Cut);
            cm.MenuItems.Add(mi);
            mi = new System.Windows.Forms.MenuItem("Copy");
            mi.Click += new EventHandler(mi_Copy);
            cm.MenuItems.Add(mi);
            mi = new System.Windows.Forms.MenuItem("Paste");
            mi.Click += new EventHandler(mi_Paste);
            cm.MenuItems.Add(mi);
            tbspeech.ContextMenu = cm;

            // add available TTS voices
            foreach (ISpeechObjectToken Token in TTSVoiceClipboard.GetVoices(string.Empty, string.Empty))
            {
                cmbVoices.Items.Add(Token.GetDescription(0));
                cbVoiceBatch.Items.Add(Token.GetDescription(0));
                cbLang1.Items.Add(Token.GetDescription(0));
                cbLang2.Items.Add(Token.GetDescription(0));
                cbLang3.Items.Add(Token.GetDescription(0));
                cbLang4.Items.Add(Token.GetDescription(0));
                cbLang5.Items.Add(Token.GetDescription(0));
                cbLang6.Items.Add(Token.GetDescription(0));
            }
            cmbVoices.SelectedIndex = 0; // Select the first Index of the comboBox 
            tbarRate.Value = SpeechRateGlobal;
            trbVolume.Value = VolumeGlobal;

            // init ligatures list
            InitLigatures();

            // restore user settings
            InitLists();
            ReadSettings();

            // set minimize / hide settings
            mAllowVisible = !cbMinToTray.Checked;

            // check for update at startup
            ThreadedUpdateChecker(true);

            // create tbKey PressedKeys object
            pKeys = new PressedKeys();

            // register clipboard viewer
            // _ClipboardViewerNext = SetClipboardViewer(this.Handle); // register if necessary
        }

        private void miAbout_Click(object sender, System.EventArgs e)
        {
            AboutDialog dialog = new AboutDialog(LocalVersion);
            dialog.ShowDialog(this);
            dialog.Dispose();
        }

        private void lLinkEspeak_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void whatsNewToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            WhatsNewDialog dialog = new WhatsNewDialog(LocalVersion);
            dialog.ShowDialog(this);
            dialog.Dispose();
        }

        private void lLinkDiscussion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void bIvona_Click(object sender, System.EventArgs e)
        {
            // http://www.gorancic.com/blog/net/c-paypal-donate-button
            string url = "http://affiliate.ivona.com/l/32/23620";
            System.Diagnostics.Process.Start(url);
        }

        private void SetBalloonTip(string title, string text, ToolTipIcon icon, string type)
        {
            mynotifyicon.BalloonTipTitle = title;
            mynotifyicon.BalloonTipText = text;
            mynotifyicon.BalloonTipIcon = icon;

            if (type == "info")
            {
                mynotifyicon.ShowBalloonTip(500);
            }
            else
            {
                mynotifyicon.ShowBalloonTip(5000);
            }

            if (type == "update")
            {
                mynotifyicon.BalloonTipClicked += new EventHandler(UpdateBalloonNotificationClick);
            }
            else
            {
                sWorkingStatus.Text = text;
                mynotifyicon.BalloonTipClicked += new EventHandler(BalloonNotificationClick);
            }
        }

        void BalloonNotificationClick(object sender, EventArgs e)
        {
            ShowForm();
        }

        void UpdateBalloonNotificationClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://sourceforge.net/projects/read4mecbr/files/latest/download?source=files");
        }

        void mi_Cut(object sender, EventArgs e)
        {
            tbspeech.Cut();
        }
        void mi_Copy(object sender, EventArgs e)
        {
            //Clipboard.SetData(DataFormats.Rtf, tbspeech.SelectedRtf);
            tbspeech.Copy();
        }
        void mi_Paste(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText(TextDataFormat.Rtf))
            {
                tbspeech.SelectedRtf = Clipboard.GetData(DataFormats.Rtf).ToString();
            }
        }
    }
}