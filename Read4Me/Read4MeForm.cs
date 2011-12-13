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
        SpVoice speech_Convert = new SpVoice();
        SpVoice speech_cpRead = new SpVoice();

        int speechRate = 4; // Ranges from -10 to 10 
        int volume_global = 80; // Range from 0 to 100.
        bool paused = false;

        // program version
        string local_version = "0.3.4";
        
        SortedList langids = new SortedList();
        SortedList ligatures = new SortedList();
        
        // declare writes & reader and folder
        StreamReader file_reader;
        StreamWriter file_writer;

        // show/hide main window
        bool mAllowVisible;     // ContextMenu's Show command used
        bool mAllowClose;       // ContextMenu's Exit command used

        public Read4MeForm()
        {
            InitializeComponent();
            this.Text = this.Text + local_version;

            lLinkEspeak.Links.Add(0, lLinkEspeak.Text.Length, lLinkEspeak.Text);

            // http://www.jonasjohn.de/snippets/csharp/drag-and-drop-example.htm
            // Enable drag and drop for this form
            // (this can also be applied to any controls)
            this.AllowDrop = true;

            // Add event handlers for the drag & drop functionality
            this.DragEnter += new DragEventHandler(Form_DragEnter);
            this.DragDrop += new DragEventHandler(Form_DragDrop);
            
            foreach (ISpeechObjectToken Token in speech_Convert.GetVoices(string.Empty, string.Empty))
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
            tbarRate.Value = speechRate;
            trbVolume.Value = volume_global;

            // init ligatures list
            InitLigatures();

            // restore user settings
            init_lists();
            ReadSettings();

            // init TTS
            speech_Convert.Rate = 10;
            speech_Convert.Volume = 0;
            speech_Convert.Speak("<lang langid=\"402\">а</lang>", SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);

            speech_cpRead.Rate = 10;
            speech_cpRead.Volume = 0;
            speech_cpRead.Speak("<lang langid=\"402\">а</lang>", SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
        }

        private void miAbout_Click(object sender, System.EventArgs e)
        {
            AboutDialog dialog = new AboutDialog(local_version);
            dialog.ShowDialog(this);
            dialog.Dispose();
        }

        private void lLinkEspeak_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void whatsNewToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            WhatsNewDialog dialog = new WhatsNewDialog(local_version);
            dialog.ShowDialog(this);
            dialog.Dispose();
        }
    }
}