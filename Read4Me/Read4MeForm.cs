using System.Windows.Forms;
using SpeechLib;
using System.Threading;
using System.IO;
using System.Collections;

namespace Read4Me
{
    public partial class Read4MeForm : Form
    {
        // some required members 
        static SpVoice TTSVoiceClipboard = new SpVoice();

        int SpeechRateGlobal = 4; // Ranges from -10 to 10 
        int VolumeGlobal = 80; // Range from 0 to 100.
        bool PausedGlobal = false;
        SpObjectToken SpeechVoiceGlobal;

        // program version
        string LocalVersion = "0.4";
        
        SortedList ligatures = new SortedList();

        // show/hide main window
        bool mAllowVisible;     // ContextMenu's Show command used
        bool mAllowClose;       // ContextMenu's Exit command used

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

            foreach (ISpeechObjectToken Token in TTSVoiceClipboard.GetVoices(string.Empty, string.Empty))
            {
                // Populate the ComboBox Entries ..
                cmbVoices.Items.Add(Token.GetDescription(49));
                cbVoiceBatch.Items.Add(Token.GetDescription(49));
                cbLang1.Items.Add(Token.GetDescription(49));
                cbLang2.Items.Add(Token.GetDescription(49));
                cbLang3.Items.Add(Token.GetDescription(49));
                cbLang4.Items.Add(Token.GetDescription(49));
                cbLang5.Items.Add(Token.GetDescription(49));
                cbLang6.Items.Add(Token.GetDescription(49));
            }
            cmbVoices.SelectedIndex = 0; // Select the first Index of the comboBox 
            tbarRate.Value = SpeechRateGlobal;
            trbVolume.Value = VolumeGlobal;

            // init ligatures list
            InitLigatures();

            // restore user settings
            init_lists();
            ReadSettings();

            // check for update on startup
            if (CheckForUpdate() == 1)
            {
                UpdateDialog dialog = new UpdateDialog();
                dialog.ShowDialog(this);
                dialog.Dispose();
            }
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
    }
}