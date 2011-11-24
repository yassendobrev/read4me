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
        SpVoice speech = new SpVoice();
        int speechRate = 4; // Ranges from -10 to 10 
        int volume_global = 80; // Range from 0 to 100.
        bool paused = false;
        
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
            
            foreach (ISpeechObjectToken Token in speech.GetVoices(string.Empty, string.Empty))
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

            // advertisment spoken at first call, speak it at volume 0
            speech.Rate = 10;
            speech.Volume = 0;
            speech.Speak("<lang langid=\"402\">а</lang>", SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
        }

        private void miAbout_Click(object sender, System.EventArgs e)
        {
            AboutDialog testDialog = new AboutDialog();

            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            if (testDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Read the contents of testDialog's TextBox.
                //this.txtResult.Text = testDialog.TextBox1.Text;
            }
            else
            {
                //this.txtResult.Text = "Cancelled";
            }
            testDialog.Dispose();
        }
    }
}