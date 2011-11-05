using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SpeechLib;
using System.Threading;
using System.IO;
using IdSharp.Tagging.ID3v2;
using System.Text.RegularExpressions;
using Yeti.MMedia;
using Yeti.MMedia.Mp3;
using WaveLib;

namespace Read4Me
{
    public partial class Form1 : Form
    {
        // some required members 
        SpVoice speech = new SpVoice();
        int speechRate = 4; // Ranges from -10 to 10 
        int speechRate_bg = 4; // Ranges from -10 to 10
        int speechRate_ge = 1; // Ranges from -10 to 10
        int speechRate_en = 1; // Ranges from -10 to 10
        int volume = 80; // Range from 0 to 100.
        bool paused = false;
        
        // declare writes & reader and folder
        StreamReader map_reader;
        StreamWriter map_writer;
        string folder_for_batch;

        // show/hide main window
        bool mAllowVisible;     // ContextMenu's Show command used
        bool mAllowClose;       // ContextMenu's Exit command used
        
        public Form1()
        {
            InitializeComponent();
            register_hotkeys();

            foreach (ISpeechObjectToken Token in speech.GetVoices(string.Empty, string.Empty))
            {
                // Populate the ComboBox Entries ..
                cmbVoices.Items.Add(Token.GetDescription(49));
            }

            cmbVoices.SelectedIndex = 0;    // Select the first Index of the comboBox 
            tbarRate.Value = speechRate;
            trbVolume.Value = volume;

            // advertisment spoken at first call, speak it at volume 0
            speech.Rate = 10;
            speech.Volume = 0;
            speech.Speak("<lang langid=\"402\">а</lang>", SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
        }

        private void register_hotkeys()
        {
            // register hotkeys
            // http://bloggablea.wordpress.com/2007/05/01/global-hotkeys-with-net/

            // langid: http://www.usb.org/developers/docs/USB_LANGIDs.pdf

            List<Hotkey> hotkeys = new List<Hotkey>();
            
            List<Keys> keycodes = new List<Keys>();
            keycodes.Add(Keys.O);
            keycodes.Add(Keys.B);
            keycodes.Add(Keys.D);
            keycodes.Add(Keys.E);
            keycodes.Add(Keys.V);
            keycodes.Add(Keys.N);
            keycodes.Add(Keys.S);

            List<Action> actions = new List<Action>();
            actions.Add(() => this.ShowForm());
            actions.Add(() => this.readClipboard("402"));
            actions.Add(() => this.readClipboard("407"));
            actions.Add(() => this.readClipboard("409"));
            actions.Add(() => this.SpeechSkip(-1));
            actions.Add(() => this.SpeechSkip(1));
            actions.Add(() => this.SpeechStop());
            
            for (int i = 0; i < keycodes.Count; i++)
            {
                hotkeys.Add(new Hotkey());
                hotkeys[i].KeyCode = keycodes[i];
                hotkeys[i].Control = true;
                hotkeys[i].Windows = true;
                int delegate_num = i;
                hotkeys[i].Pressed += delegate { actions[delegate_num](); };

                if (!hotkeys[i].GetCanRegister(this))
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        sWorkingStatus.Text = "Error registering one or more hotkeys!";
                    });
                }
                else
                {
                    hotkeys[i].Register(this);
                }
            }
        }

        // Minimize app to tray
        // http://stackoverflow.com/questions/1730731/how-to-start-winform-app-minimized-to-tray
        protected override void SetVisibleCore(bool value)
        {
            if (!mAllowVisible) value = false;
            base.SetVisibleCore(value);
        }

        // Close to tray
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!mAllowClose)
            {
                HideForm();
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }

        // Show form from tray strip menu
        private void showStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        // Close form from tray strip menu
        private void exitStripMenuItem_Click(object sender, EventArgs e)
        {
            mAllowClose = true;
            Close();
        }

        private void ShowForm()
        {
            mAllowVisible = true;
            Show();
            this.Activate();
        }

        private void HideForm()
        {
            mAllowVisible = false;
            this.Hide();
        }

        void Form1_Resize(object sender, System.EventArgs e)
        {
            HideForm();
        }

        private void mynotifyicon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (mAllowVisible == true)
                {
                    HideForm();
                }
                else
                {
                    ShowForm();
                }
            }
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            mAllowClose = mAllowVisible = true;
            Close();
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            try
            {
                if (msg.WParam.ToInt32() == (int)Keys.Escape)
                {
                    this.Hide();
                }
                else
                {
                    return base.ProcessCmdKey(ref msg, keyData);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Key Overrided Events Error:" + Ex.Message);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }




        // Speech // Speech // Speech // Speech // Speech // Speech // Speech // Speech // Speech // Speech // Speech
        // Speech // Speech // Speech // Speech // Speech // Speech // Speech // Speech // Speech // Speech // Speech
        // Speech // Speech // Speech // Speech // Speech // Speech // Speech // Speech // Speech // Speech // Speech

        private void SpeechSkip(int items)
        {
            speech.Skip("Sentence", items);
        }

        private void SpeechStop()
        {
            if (speech.Status.RunningState == SpeechRunState.SRSEIsSpeaking)
            {
                speech.Pause();
                paused = true;
            }
            else
            {
                if (paused == true)
                {
                    speech.Resume();
                    paused = false;
                }
            }
        }

        string toRead;
        private void readClipboard(string langid)
        {
            int srate;
            SpObjectToken voice;
            if (langid == "402")
            {// BG
                srate = speechRate_bg;
                voice = speech.GetVoices("Name=Gergana", "Language=" + langid).Item(0);
            }
            else if (langid == "407")
            { // DE
                srate = speechRate_ge;
                voice = speech.GetVoices("Name=ScanSoft Steffi_Full_22KHz", "Language=" + langid).Item(0);
            }
            // else if (langid == "409")
            else
            { // EN
                srate = speechRate_en;
                voice = speech.GetVoices("Name=VW Julie", "Language=" + langid).Item(0);
            }

            paused = false;
            toRead = Clipboard.GetText();
            toRead.Replace("\r\n", "<lang langid=\"409\"><silence msec=\"50\" /></lang>"); // new line -> pause for 50ms
            toRead = "<lang langid=\""+ langid + "\"><pitch middle='0'>" + toRead + "</pitch></lang>";
            speech.Rate = srate;
            speech.Volume = volume;
            speech.Voice = voice;
            speech.Resume();
            speech.Speak(toRead, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
        }

        private void btnSpeak_Click(object sender, EventArgs e)
        {
            if (paused)
            {
                speech.Resume();
                paused = false;
            }
            else
            {
                speech.Rate = speechRate;
                speech.Volume = volume;
                speech.Speak(tbspeech.Text, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML);
            }
        }

        private void btnToWAV_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "All files (*.*)|*.*|wav files (*.wav)|*.wav";
                sfd.Title = "Save to a wave file";
                sfd.FilterIndex = 2;
                sfd.RestoreDirectory = true;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SpeechStreamFileMode SpFileMode = SpeechStreamFileMode.SSFMCreateForWrite;
                    SpFileStream SpFileStream = new SpFileStream();
                    SpFileStream.Open(sfd.FileName, SpFileMode, false);
                    speech.AudioOutputStream = SpFileStream;
                    speech.Rate = speechRate;
                    speech.Volume = volume;
                    speech.Speak(tbspeech.Text, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML);
                    speech.WaitUntilDone(Timeout.Infinite);
                    SpFileStream.Close();
                }
            }
            catch
            {
                MessageBox.Show("There is some error in converting to Wav file.");
            }
        }

        private void cmbVoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            speech.Voice = speech.GetVoices(string.Empty, string.Empty).Item(cmbVoices.SelectedIndex);
        }

        private void tbarRate_Scroll(object sender, EventArgs e)
        {
            speechRate = tbarRate.Value;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            speech.Pause();
            paused = true;
        }


        private void trbVolume_Scroll(object sender, EventArgs e)
        {
            volume = trbVolume.Value;
        }

        private void bBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                folder_for_batch = folderDialog.SelectedPath;
            }
        }

        private void bBatch_Click(object sender, EventArgs e)
        {
            BatchConvert(folder_for_batch);
        }

        private void BatchConvert(string folder)
        {
            string[] fileEntries;
            string title;
            string artist;
            string album;
            string track;
            string year;

            // get data for id3 tags
            year = tbYear.Text;
            artist = tbArtist.Text;
            album = tbAlbum.Text;

            try
            {
                fileEntries = Directory.GetFiles(folder, "*.xml");
                Directory.CreateDirectory(folder.Replace("xml", "mp3"));
            }
            catch
            {
                MessageBox.Show("No folder for batch!");
                return;
            }

            foreach (string FileName in fileEntries)
            {
                Application.DoEvents();
                this.Invoke((MethodInvoker)delegate
                {
                    sWorkingStatus.Text = "Working on " + FileName;
                });

                // convert xml to wav
                SpeakText(FileName);

                // prepare id3 tags
                title = FileName.Replace(folder + "\\", "").Replace(".xml", "");
                track = FileName.Replace(folder + "\\", "").Replace(".xml", ""); // files are numbered 1..n

                // convert .wav to .mp3
                wav2mp3(FileName, title, artist, year + " " + album, track);
                File.Delete(FileName.Replace(".xml", ".wav"));
                Application.DoEvents();
            }
            Directory.Move(folder.Replace("xml", "mp3"), folder.Replace("xml", artist + " - " + year + " " + album));

            MessageBox.Show("All done!");
        }

        public void SpeakText(string file)
        {
            StreamReader reader;
            string toSpeak;

            reader = new StreamReader(file, Encoding.UTF8);
            toSpeak = reader.ReadToEnd();
            SpeechStreamFileMode SpFileMode = SpeechStreamFileMode.SSFMCreateForWrite;
            SpFileStream SpFileStream = new SpFileStream();
            SpFileStream.Open(file.Replace(".xml", ".wav"), SpFileMode, false);
            speech.AudioOutputStream = SpFileStream;
            speech.Rate = speechRate;
            speech.Volume = volume;
            // speech.Speak(FileName, SpeechVoiceSpeakFlags.SVSFIsFilename); // not working properly
            speech.Speak(toSpeak, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML);
            speech.WaitUntilDone(Timeout.Infinite);
            SpFileStream.Close();
            reader.Close();
        }

        public void wav2mp3(string FileName, string title, string artist, string album, string track)
        {
            string pworkingDir = Path.GetDirectoryName(Application.ExecutablePath);
            string wavpath = FileName.Replace(".xml", ".wav");
            string mp3path = FileName.Replace("xml", "mp3");
            int read = 0;

            WaveStream InStr = new WaveStream(wavpath);
            Yeti.Lame.BE_CONFIG cfg = new Yeti.Lame.BE_CONFIG(InStr.Format, 64); // 64kbps
            Mp3Writer writer = new Mp3Writer(new FileStream(mp3path, FileMode.Create), InStr.Format, cfg);
            byte[] buff = new byte[writer.OptimalBufferSize];
            while ((read = InStr.Read(buff, 0, buff.Length)) > 0)
            {
                writer.Write(buff, 0, read);
                Application.DoEvents();
            }
            InStr.Close();
            writer.Close();

            /*
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            psi.FileName = "\"" + pworkingDir + "\\ffmpeg\\ffmpeg.exe" + "\"";
            psi.Arguments = "-i \"" + wavpath + "\" ";
            psi.Arguments += " -y \"" + mp3path + "\"";
            psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            System.Diagnostics.Process p = System.Diagnostics.Process.Start(psi);
            p.WaitForExit();
            */

            // edit id3 tags: http://www.idsharp.com/
            IdSharp.Tagging.ID3v2.IID3v2Tag id3v2 = new ID3v2Tag(mp3path);
            id3v2.Header.TagVersion = ID3v2TagVersion.ID3v22;
            id3v2.Title = title;
            id3v2.Album = album;
            id3v2.Artist = artist;
            id3v2.TrackNumber = track;
            id3v2.Save(mp3path);
        }

        private void bSource_Click(object sender, EventArgs e)
        {
            OpenFileDialog sourceDialog = new OpenFileDialog();
            sourceDialog.Title = "Source File";
            sourceDialog.Filter = "TXT files (*.txt)|*.txt";
            sourceDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (sourceDialog.ShowDialog() == DialogResult.OK)
            {
                tbSource.Text = sourceDialog.FileName.ToString();
            }
        }

        private void bGo_Click(object sender, EventArgs e)
        {
            string outdir;
            int filename = 1;
            string line;
            int empty_lines = 0;
            bool start_new_file = false;
            string[] line_parts;

            try
            {
                map_reader = new StreamReader(tbSource.Text, Encoding.UTF8);
            }
            catch
            {
                MessageBox.Show("Error opening files!");
                return;
            }

            outdir = tbSource.Text.Substring(0, tbSource.Text.LastIndexOf("\\") + 1);
            try
            {
                Directory.CreateDirectory(outdir + "xml");
            }
            catch
            {
            }

            map_writer = new StreamWriter(outdir + "xml\\" + filename.ToString("00") + ".xml", false, Encoding.UTF8);
            map_writer.Write("<lang langid=\"402\">");
            while (!map_reader.EndOfStream)
            {
                line = map_reader.ReadLine().Replace("\t", "");
                line = line.Replace(" ", " "); // replace Non-breaking space 0xA0 with normal space 0x20
                line = line.Replace("]", "");
                line = line.Replace("[", "");
                
                // IMPLEMENT SLOWER RATE WHEN words surrounded with _words_ -> emphasize!
                // use <rate absspeed="-7"/>
                line_parts = line.Split('_');
                if (line_parts.Length > 1)
                {
                    line = "";
                    for (int i = 0; i < line_parts.Length - 1; i++)
                    {
                        if(i % 2 == 0)
                        {
                            line = line + line_parts[i] + "<rate absspeed=\"-7\"/>";
                        }
                        else
                        {
                            // line = line + line_parts[i] + "<rate absspeed=\"" + speechRate.ToString() + "\"/>"; --> too fast???
                            line = line + line_parts[i] + "<rate absspeed=\"0\"/>";
                        }
                    }
                    line = line + line_parts[line_parts.Length - 1]; // add last part
                    // line = line + "<rate absspeed=\"" + speechRate + "\"/>"; // make sure normal rate continues
                    line = line + "<rate absspeed=\"0\"/>"; // make sure normal rate continues
                }

                if (line != "" && line != "Kodirane UTF-8")
                {
                    empty_lines = 0;
                    // if (Regex.IsMatch(line, "Глава [IVX1-9]+", RegexOptions.IgnoreCase))
                    // if (Regex.IsMatch(line, tbBefore.Text + "[IVX1-9]+" + tbAfter.Text, RegexOptions.IgnoreCase))
                    if (Regex.IsMatch(line, "^" + Regex.Escape(tbBefore.Text) + "[IVX0-9]+" + Regex.Escape(tbAfter.Text) + "$", RegexOptions.IgnoreCase) || start_new_file)
                    {
                        start_new_file = false;
                        map_writer.Write("<lang langid=\"409\"><silence msec=\"200\" /></lang>"); // longer pause at the end of chapter
                        map_writer.Write("</lang>");
                        map_writer.Close();
                        // filename = Regex.Replace(line, @"[\D]", "");
                        filename++;
                        map_writer = new StreamWriter(outdir + "xml\\" + filename.ToString("00") + ".xml", false, Encoding.UTF8);

                        map_writer.Write("<lang langid=\"402\">");
                        map_writer.Write(line);
                        map_writer.Write("<lang langid=\"409\"><silence msec=\"200\" /></lang>"); // longer pause after announcing a chapter
                    }
                    else
                    {
                        map_writer.Write(line);
                        map_writer.Write("<lang langid=\"409\"><silence msec=\"50\" /></lang>");
                    }
                }
                else
                {
                    empty_lines++;
                    if (empty_lines >= Int32.Parse(tbEmptyLines.Text))
                    {
                        start_new_file = true;
                    }
                }
            }
            map_reader.Close();
            map_writer.Close();
            this.Invoke((MethodInvoker)delegate
            {
                sWorkingStatus.Text = "Done with txt2xml!";
            });

            // batch convert xml to mp3
            BatchConvert(outdir + "xml");
        }
    }
}