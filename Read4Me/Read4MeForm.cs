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
using System.Xml;

namespace Read4Me
{
    public partial class Read4MeForm : Form
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
        StreamReader file_reader;
        StreamWriter file_writer;
        string folder_for_batch;

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
                cbLang1.Items.Add(Token.GetDescription(49));
                cbLang2.Items.Add(Token.GetDescription(49));
                cbLang3.Items.Add(Token.GetDescription(49));
                cbLang4.Items.Add(Token.GetDescription(49));
                cbLang5.Items.Add(Token.GetDescription(49));
                cbLang6.Items.Add(Token.GetDescription(49));
            }
            cmbVoices.SelectedIndex = 0; // Select the first Index of the comboBox 
            tbarRate.Value = speechRate;
            trbVolume.Value = volume;

            // RegisterHotkeys();
            init_lists();
            ReadSettings();

            // advertisment spoken at first call, speak it at volume 0
            speech.Rate = 10;
            speech.Volume = 0;
            speech.Speak("<lang langid=\"402\">а</lang>", SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
        }

        List<ComboBox> comboboxes_lang = new List<ComboBox>();
        List<CheckBox> checkboxes_ctrl_lang = new List<CheckBox>();
        List<CheckBox> checkboxes_winkey_lang = new List<CheckBox>();
        List<CheckBox> checkboxes_alt_lang = new List<CheckBox>();
        List<TextBox> textboxes_lang = new List<TextBox>();
        private void init_lists()
        {
            comboboxes_lang.Add(cbLang1);
            comboboxes_lang.Add(cbLang2);
            comboboxes_lang.Add(cbLang3);
            comboboxes_lang.Add(cbLang4);
            comboboxes_lang.Add(cbLang5);
            comboboxes_lang.Add(cbLang6);

            checkboxes_ctrl_lang.Add(lCtrllang1);
            checkboxes_ctrl_lang.Add(lCtrllang2);
            checkboxes_ctrl_lang.Add(lCtrllang3);
            checkboxes_ctrl_lang.Add(lCtrllang4);
            checkboxes_ctrl_lang.Add(lCtrllang5);
            checkboxes_ctrl_lang.Add(lCtrllang6);

            checkboxes_winkey_lang.Add(lWinKeylang1);
            checkboxes_winkey_lang.Add(lWinKeylang2);
            checkboxes_winkey_lang.Add(lWinKeylang3);
            checkboxes_winkey_lang.Add(lWinKeylang4);
            checkboxes_winkey_lang.Add(lWinKeylang5);
            checkboxes_winkey_lang.Add(lWinKeylang6);

            checkboxes_alt_lang.Add(lAltlang1);
            checkboxes_alt_lang.Add(lAltlang2);
            checkboxes_alt_lang.Add(lAltlang3);
            checkboxes_alt_lang.Add(lAltlang4);
            checkboxes_alt_lang.Add(lAltlang5);
            checkboxes_alt_lang.Add(lAltlang6);

            textboxes_lang.Add(tbHKlang1);
            textboxes_lang.Add(tbHKlang2);
            textboxes_lang.Add(tbHKlang3);
            textboxes_lang.Add(tbHKlang4);
            textboxes_lang.Add(tbHKlang5);
            textboxes_lang.Add(tbHKlang6);   
        }
        
        private void ReadSettings()
        {
            string type;
            bool ctrl;
            bool winkey;
            bool alt;
            string langid;
            string voice;
            char key;
            int lang_num = 0;
            
            try
            {
                XmlReader reader = XmlReader.Create(Path.GetDirectoryName(Application.ExecutablePath) + "\\Read4Me.ini");
                while (reader.Read())
                {
                    // Only detect start elements.
                    if (reader.IsStartElement())
                    {
                        // Get element name and switch on it.
                        switch (reader.Name)
                        {
                            case "hotkey_general":
                                type = reader["type"];
                                ctrl = (reader["ctrl"] == "1") ? true : false;
                                winkey = (reader["winkey"] == "1") ? true : false;
                                alt = (reader["alt"] == "1") ? true : false;
                                if (reader["key"] != "")
                                {
                                    key = reader["key"][0];
                                }
                                else
                                {
                                    key = '\0';
                                }
                                SetViewSettings(type, ctrl, winkey, alt, key, "", "", 0);
                                RegisterHotkey(type, ctrl, winkey, alt, key, "", "");
                            break;

                            case "hotkey_speech":
                                type = reader["type"];
                                ctrl = (reader["ctrl"] == "1") ? true : false;
                                winkey = (reader["winkey"] == "1") ? true : false;
                                alt = (reader["alt"] == "1") ? true : false;
                                if (reader["key"] != "")
                                {
                                    key = reader["key"][0];
                                }
                                else
                                {
                                    key = '\0';
                                }
                                langid = reader["langid"];
                                voice = reader["voice"];
                                SetViewSettings(type, ctrl, winkey, alt, key, langid, voice, lang_num);
                                RegisterHotkey(type, ctrl, winkey, alt, key, langid, voice);
                                lang_num++;
                            break;

                            default:
                            break;
                        }
                    }
                }
                reader.Close();
            }
            catch
            {
                sWorkingStatus.Text = "No settings file found!";
            }
        }

        private void SetViewSettings(string type, bool ctrl, bool winkey, bool alt, char key, string langid, string voice, int lang_num)
        {
            switch (type)
            {
                case "show_hide":
                    lCtrl0.Checked = ctrl;
                    lWinKey0.Checked = winkey;
                    lAlt0.Checked = alt;
                    tbHK0.Text = key.ToString();
                break;

                case "pause_resume":
                    lCtrl1.Checked = ctrl;
                    lWinKey1.Checked = winkey;
                    lAlt1.Checked = alt;
                    tbHK1.Text = key.ToString();
                break;

                case "previous_sentence":
                    lCtrl2.Checked = ctrl;
                    lWinKey2.Checked = winkey;
                    lAlt2.Checked = alt;
                    tbHK2.Text = key.ToString();
                break;

                case "next_sentence":
                    lCtrl3.Checked = ctrl;
                    lWinKey3.Checked = winkey;
                    lAlt3.Checked = alt;
                    tbHK3.Text = key.ToString();
                break;

                case "speech":
                    checkboxes_ctrl_lang[lang_num].Checked = ctrl;
                    checkboxes_winkey_lang[lang_num].Checked = winkey;
                    checkboxes_alt_lang[lang_num].Checked = alt;
                    textboxes_lang[lang_num].Text = key.ToString();
                    comboboxes_lang[lang_num].SelectedIndex = cbLang1.FindStringExact(voice);
                break;

                default:
                break;
            }
        }

        List<Hotkey> hotkeys = new List<Hotkey>();
        List<Keys> keycodes = new List<Keys>();
        List<Action> actions = new List<Action>();
        private bool RegisterHotkey(string type, bool ctrl, bool winkey, bool alt, char key, string langid, string voice)
        {
            // register hotkeys
            // http://bloggablea.wordpress.com/2007/05/01/global-hotkeys-with-net/

            // langid: http://www.usb.org/developers/docs/USB_LANGIDs.pdf
            hotkeys.Add(new Hotkey());
            int current_hotkey = hotkeys.Count - 1;
            hotkeys[current_hotkey].Control = ctrl;
            hotkeys[current_hotkey].Windows = winkey;
            hotkeys[current_hotkey].Alt = alt;
            hotkeys[current_hotkey].KeyCode = (Keys)(byte)char.ToUpper(key);
            int delegate_num = current_hotkey;

            Action todo_action = delegate { return; };
            switch (type)
            {
                case "show_hide":
                    todo_action = () => ToggleForm();
                break;

                case "pause_resume":
                    todo_action = () => SpeechStop();
                break;

                case "previous_sentence":
                    todo_action = () => SpeechSkip(-1);
                break;

                case "next_sentence":
                    todo_action = () => SpeechSkip(1);
                break;

                case "speech":
                    todo_action = () => ReadClipboard(langid, voice);
                break;
            }
            hotkeys[current_hotkey].Pressed += delegate { todo_action(); };

            if (!hotkeys[current_hotkey].GetCanRegister(this))
            {
                return false;
            }
            else
            {
                hotkeys[current_hotkey].Register(this);
                return true;
            }
        }

        private void UnregisterHotkeys()
        {
            for (int i = 0; i < hotkeys.Count; i++)
            {
                hotkeys[i].Unregister();
            }
            hotkeys.Clear();
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            file_writer = new StreamWriter(Path.GetDirectoryName(Application.ExecutablePath) + "\\Read4Me.ini", false, Encoding.UTF8);
            file_writer.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
            file_writer.Write("<settings>\r\n");
            file_writer.Write("    <hotkey_general type=\"show_hide\" ctrl=\"" + ((lCtrl0.Checked == true) ? "1" : "0") + "\" winkey=\"" + ((lWinKey0.Checked == true) ? "1" : "0") + "\" alt=\"" + ((lAlt0.Checked == true) ? "1" : "0") + "\" key=\"" + tbHK0.Text + "\"></hotkey_general>\r\n");
            file_writer.Write("    <hotkey_general type=\"pause_resume\" ctrl=\"" + ((lCtrl1.Checked == true) ? "1" : "0") + "\" winkey=\"" + ((lWinKey1.Checked == true) ? "1" : "0") + "\" alt=\"" + ((lAlt1.Checked == true) ? "1" : "0") + "\" key=\"" + tbHK1.Text + "\"></hotkey_general>\r\n");
            file_writer.Write("    <hotkey_general type=\"previous_sentence\" ctrl=\"" + ((lCtrl2.Checked == true) ? "1" : "0") + "\" winkey=\"" + ((lWinKey2.Checked == true) ? "1" : "0") + "\" alt=\"" + ((lAlt2.Checked == true) ? "1" : "0") + "\" key=\"" + tbHK2.Text + "\"></hotkey_general>\r\n");
            file_writer.Write("    <hotkey_general type=\"next_sentence\" ctrl=\"" + ((lCtrl3.Checked == true) ? "1" : "0") + "\" winkey=\"" + ((lWinKey3.Checked == true) ? "1" : "0") + "\" alt=\"" + ((lAlt3.Checked == true) ? "1" : "0") + "\" key=\"" + tbHK3.Text + "\"></hotkey_general>\r\n");
            for (int i = 0; i < comboboxes_lang.Count; i++)
            {
                if ((textboxes_lang[i].Text != "") && ((string)comboboxes_lang[i].SelectedItem != ""))
                {
                    file_writer.Write("    <hotkey_speech type=\"speech\" ctrl=\"" + ((checkboxes_ctrl_lang[i].Checked == true) ? "1" : "0") + "\" winkey=\"" + ((checkboxes_winkey_lang[i].Checked == true) ? "1" : "0") + "\" alt=\"" + ((checkboxes_alt_lang[i].Checked == true) ? "1" : "0") + "\" key=\"" + textboxes_lang[i].Text + "\" langid=\"402\" voice=\"" + comboboxes_lang[i].SelectedItem + "\"></hotkey_speech>\r\n");
                }
            }
            file_writer.Write("</settings>\r\n");
            file_writer.Close();
            UnregisterHotkeys();
            ReadSettings();
        }

        private void RegisterHotkeys()
        {
            // register hotkeys
            // http://bloggablea.wordpress.com/2007/05/01/global-hotkeys-with-net/

            // langid: http://www.usb.org/developers/docs/USB_LANGIDs.pdf

            List<Hotkey> hotkeys = new List<Hotkey>();
            
            List<Keys> keycodes = new List<Keys>();
            keycodes.Add(Keys.O);
            keycodes.Add(Keys.S);
            keycodes.Add(Keys.V);
            keycodes.Add(Keys.N);
            keycodes.Add(Keys.B);
            keycodes.Add(Keys.D);
            keycodes.Add(Keys.E);

            List<Action> actions = new List<Action>();
            actions.Add(() => this.ToggleForm());
            actions.Add(() => this.SpeechStop());
            actions.Add(() => this.SpeechSkip(-1));
            actions.Add(() => this.SpeechSkip(1));
            actions.Add(() => this.ReadClipboard("402", ""));
            actions.Add(() => this.ReadClipboard("407", ""));
            actions.Add(() => this.ReadClipboard("409", ""));
            
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
        private void ReadClipboard(string langid, string voice)
        {
            int srate;
            SpObjectToken voice_sp;
            if (langid == "402")
            {// BG
                srate = speechRate_bg;
                voice_sp = speech.GetVoices("Name=Gergana", "Language=" + langid).Item(0);
            }
            else if (langid == "407")
            { // DE
                srate = speechRate_ge;
                voice_sp = speech.GetVoices("Name=ScanSoft Steffi_Full_22KHz", "Language=" + langid).Item(0);
            }
            // else if (langid == "409")
            else
            { // EN
                srate = speechRate_en;
                voice_sp = speech.GetVoices("Name=VW Julie", "Language=" + langid).Item(0);
            }

            paused = false;
            toRead = Clipboard.GetText();
            toRead.Replace("\r\n", "<lang langid=\"409\"><silence msec=\"50\" /></lang>"); // new line -> pause for 50ms
            toRead = "<lang langid=\""+ langid + "\"><pitch middle='0'>" + toRead + "</pitch></lang>";
            speech.Rate = srate;
            speech.Volume = volume;
            speech.Voice = voice_sp;
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
                file_reader = new StreamReader(tbSource.Text, Encoding.UTF8);
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

            file_writer = new StreamWriter(outdir + "xml\\" + filename.ToString("00") + ".xml", false, Encoding.UTF8);
            file_writer.Write("<lang langid=\"402\">");
            while (!file_reader.EndOfStream)
            {
                line = file_reader.ReadLine().Replace("\t", "");
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
                        file_writer.Write("<lang langid=\"409\"><silence msec=\"200\" /></lang>"); // longer pause at the end of chapter
                        file_writer.Write("</lang>");
                        file_writer.Close();
                        // filename = Regex.Replace(line, @"[\D]", "");
                        filename++;
                        file_writer = new StreamWriter(outdir + "xml\\" + filename.ToString("00") + ".xml", false, Encoding.UTF8);

                        file_writer.Write("<lang langid=\"402\">");
                        file_writer.Write(line);
                        file_writer.Write("<lang langid=\"409\"><silence msec=\"200\" /></lang>"); // longer pause after announcing a chapter
                    }
                    else
                    {
                        file_writer.Write(line);
                        file_writer.Write("<lang langid=\"409\"><silence msec=\"50\" /></lang>");
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
            file_reader.Close();
            file_writer.Close();
            this.Invoke((MethodInvoker)delegate
            {
                sWorkingStatus.Text = "Done with txt2xml!";
            });

            // batch convert xml to mp3
            BatchConvert(outdir + "xml");
        }
    }
}