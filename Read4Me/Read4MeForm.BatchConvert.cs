using System;
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
using System.Windows.Media;

namespace Read4Me
{
    partial class Read4MeForm
    {
        static Thread oThread;
        bool StopConversion = false;

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

        private void bGoStartState()
        {
            if (this.bGo.InvokeRequired)
            {
                this.bGo.Invoke(new Action(bGoStartState));
                return;
            }
            bGo.Text = "Start text to mp3 conversion";
            bGo.Enabled = true;
            StopConversion = false;
        }
        
        private void bGoStopState()
        {
            bGo.Text = "Stop conversion";
            bGo.Enabled = true;
        }

        private void bGoStoppingState()
        {
            bGo.Text = "Stopping conversion...";
            bGo.Enabled = false;
        }

        private void bGo_Click(object sender, EventArgs e)
        {
            try
            {
                if (oThread.IsAlive)
                {
                    StopConversion = true;
                    bGoStoppingState();
                    return;
                }
            }
            catch
            {
                // thread not initialized
            }

            int SpeechRate;
            int SpeechVolume;
            string SpeechVoice;
            string FilePath;
            string artist;
            string album;
            string year;

            try
            {
                SpeechRate = Int16.Parse(cbRateBatch.SelectedItem.ToString());
                SpeechVolume = Int16.Parse(cbVolumeBatch.SelectedItem.ToString());
                SpeechVoice = cbVoiceBatch.Text;
                // SpeechVoice = TTSVoiceClipboard.GetVoices("Name=" + cbVoiceBatch.Text, string.Empty).Item(0);
                FilePath = tbSource.Text;
            }
            catch
            {
                SetBalloonTip("Error", "Error setting voice.", ToolTipIcon.Error, "error");
                return;
            }

            // get data for id3 tags
            artist = tbArtist.Text;
            year = tbYear.Text;
            album = tbAlbum.Text;

            if (artist == "")
            {
                artist = "Incognito";
            }
            if (album == "")
            {
                album = "Unnamed";
            }

            // Thread oThread = new Thread(new ParameterizedThreadStart(doConvert));
            oThread = new Thread(() => this.doConvert(SpeechRate, SpeechVolume, SpeechVoice, FilePath, artist, album, year));
            bGoStopState();
            oThread.SetApartmentState(ApartmentState.STA);
            oThread.Start();
        }

        private void doConvert(int SpeechRate, int SpeechVolume, string SpeechVoice, string FilePath, string artist, string album, string year)
        {
            string outdir;
            int filename = 1;
            string line;
            int empty_lines = 0;
            bool start_new_file = false;
            string[] line_parts;
            string artist_year_album;
            StreamWriter file_writer;
            StreamReader file_reader;

            this.Invoke((MethodInvoker)delegate
            {
                sWorkingStatus.Text = "Starting conversion...";
            });

            try
            {
                file_reader = new StreamReader(FilePath, Encoding.UTF8);
            }
            catch
            {
                SetBalloonTip("Error", "Error opening files!", ToolTipIcon.Error, "error");
                bGoStartState();
                return;
            }

            // get the directory of the file
            outdir = tbSource.Text.Substring(0, tbSource.Text.LastIndexOf("\\") + 1);
            artist_year_album = artist + " - " + year + " " + album;

            // a file name can't contain any of the following characters
            artist_year_album = artist_year_album.Replace("\\", "");
            artist_year_album = artist_year_album.Replace("/", "");
            artist_year_album = artist_year_album.Replace(":", "");
            artist_year_album = artist_year_album.Replace("*", "");
            artist_year_album = artist_year_album.Replace("?", "");
            artist_year_album = artist_year_album.Replace("\"", "");
            artist_year_album = artist_year_album.Replace("<", "");
            artist_year_album = artist_year_album.Replace(">", "");
            artist_year_album = artist_year_album.Replace("|", "");

            outdir = outdir + artist_year_album;
            
            // check if old directories exist and delete them
            if (Directory.Exists(outdir))
            {
                if (MessageBox.Show("Output directory already exists, empty it?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        string[] files = Directory.GetFiles(outdir);
                        foreach (string file in files)
                        {
                            File.Delete(file);
                        }
                    }
                    catch
                    {
                        SetBalloonTip("Error", "Could not empty output directory.", ToolTipIcon.Error, "error");
                        bGoStartState();
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(outdir);
                }
                catch
                {
                    SetBalloonTip("Error", "Output directory " + outdir + " could not be created.", ToolTipIcon.Error, "error");
                    bGoStartState();
                    return;
                }
            }

            try
            {
                file_writer = new StreamWriter(outdir + "\\" + filename.ToString("000") + ".xml", false, Encoding.UTF8);
            }
            catch
            {
                SetBalloonTip("Error", "Could not open .xml file for writing.", ToolTipIcon.Error, "error");
                bGoStartState();
                return;
            }

            while (!file_reader.EndOfStream)
            {
                if (StopConversion)
                {
                    // if conversion aborted
                    file_writer.Close();
                    file_reader.Close();
                    bGoStartState();
                    return;
                }

                line = file_reader.ReadLine().Replace("\t", "");
                line = line.Replace(" ", " "); // replace Non-breaking space 0xA0 with normal space 0x20

                /*
                line = line.Replace("[", "(");
                line = line.Replace("]", ")");
                line = line.Replace("<", "(");
                line = line.Replace(">", ")");
                line = line.Replace("«", "(");
                line = line.Replace("»", ")");
                */
                line = line.Replace("[", " ");
                line = line.Replace("]", " ");
                line = line.Replace("<", " ");
                line = line.Replace(">", " ");
                line = line.Replace("«", " ");
                line = line.Replace("»", " ");
                line = line.Replace("­", ""); // there is a SOFT HYPHEN between the 1st "" (UTF-8 (hex)	0xC2 0xAD (c2ad))

                // SLOWER RATE WHEN words surrounded with _words_ -> emphasize!
                // use <rate absspeed="-7"/>
                if (cbSlowRate.Checked)
                {
                    line_parts = line.Split('_');
                    if (line_parts.Length > 1)
                    {
                        line = "";
                        for (int i = 0; i < line_parts.Length - 1; i++)
                        {
                            if (i % 2 == 0)
                            {
                                line = line + line_parts[i] + "<rate absspeed=\"-5\"/>";
                            }
                            else
                            {
                                // line = line + line_parts[i] + "<rate absspeed=\"" + speechRate.ToString() + "\"/>"; --> too fast???
                                line = line + line_parts[i] + "<rate absspeed=\"0\"/>";
                            }
                        }
                        line = line + line_parts[line_parts.Length - 1]; // add last part
                        // line = line + "<rate absspeed=\"" + speechRate + "\"/>"; // make sure normal rate continues --> too fast???
                        line = line + "<rate absspeed=\"0\"/>"; // make sure normal rate continues
                    }
                }

                if (line != "")
                {
                    empty_lines = 0;

                    if (cbCaptialLetters.Checked)
                    {
                        if (AllCapitals(line))
                        {
                            start_new_file = true;
                        }
                    }

                    if (start_new_file)
                    {
                        start_new_file = false;
                        file_writer.Write("."); // put an ending dot at the end of chapter (ofter there isn't any)
                        file_writer.Close();
                        file_writer.Dispose();
                        filename++;

                        try
                        {
                            file_writer = new StreamWriter(outdir + "\\" + filename.ToString("000") + ".xml", false, Encoding.UTF8);
                        }
                        catch
                        {
                            SetBalloonTip("Error", "Could not open .xml file for writing during xml conversion.", ToolTipIcon.Error, "error");
                            bGoStartState();
                            return;
                        }

                        file_writer.Write(line);
                        file_writer.Write(" ");
                    }
                    else
                    {
                        file_writer.Write(line);
                        file_writer.Write(" ");
                    }
                }
                else
                {
                    empty_lines++;
                    if (cbBreakFiles.Checked)
                    {
                        if (empty_lines >= 1)
                        {
                            start_new_file = true;
                        }
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
            BatchConvert(outdir, SpeechRate, SpeechVolume, SpeechVoice, artist, album, year);
            bGoStartState();
        }

        private bool AllCapitals(string inputString)
        {
            return (inputString == inputString.ToUpper());
        }

        private void BatchConvert(string folder, int SpeechRate, int SpeechVolume, string SpeechVoice, string artist, string album, string year)
        {
            string[] fileEntries;
            string title;
            string track;
            int summed_secs = 0;

            try
            {
                fileEntries = Directory.GetFiles(folder, "*.xml");
            }
            catch
            {
                SetBalloonTip("Error", "No folder for batch!", ToolTipIcon.Error, "error");
                bGoStartState();
                return;
            }

            int i = 1;
            foreach (string FileName in fileEntries)
            {
                if (StopConversion)
                {
                    // if conversion aborted
                    bGoStartState();
                    return;
                }

                Application.DoEvents();
                this.Invoke((MethodInvoker)delegate
                {
                    sWorkingStatus.Text = "Working on file " + i.ToString() + " from " + fileEntries.Length.ToString();
                });
                i++;
                Application.DoEvents();

                // convert xml to wav
                SpeakText(FileName, SpeechRate, SpeechVolume, SpeechVoice);

                if (StopConversion)
                {
                    // if conversion aborted
                    bGoStartState();
                    return;
                }

                // prepare id3 tags
                title = FileName.Replace(folder + "\\", "");
                title = title.Replace(".xml", "");
                track = title; // files are numbered 1..n

                // convert .wav to .mp3
                summed_secs = wav2mp3(FileName, title, artist, year + " " + album, track, summed_secs);
                File.Delete(FileName.Replace(".xml", ".wav")); // delete .wav file
                File.Delete(FileName); // delete .xml file
            }

            this.Invoke((MethodInvoker)delegate
            {
                sWorkingStatus.Text = "Conversion finished";
            });
            SetBalloonTip("Conversion finished", "All done successfully :)", ToolTipIcon.Info, "info");
        }

        public void SpeakText(string file, int SpeechRate, int SpeechVolume, string SpeechVoice)
        {
            StreamReader reader;
            string toSpeak;
            SpVoice TTSVoiceConvert = new SpVoice();
            
            // init TTS
            TTSVoiceConvert.Rate = 10;
            TTSVoiceConvert.Volume = 0;

            SpObjectToken voice_sp = null;
            int i = 0;
            ISpeechObjectTokens AvailableVoices = TTSVoiceClipboard.GetVoices(string.Empty, string.Empty);
            foreach (ISpeechObjectToken Token in AvailableVoices)
            {
                if (SpeechVoice == Token.GetDescription(0))
                {
                    voice_sp = AvailableVoices.Item(i);
                    break;
                }
                i++;
            }

            if (voice_sp == null)
            {
                SetBalloonTip("Error", "Error! Voice not found!", ToolTipIcon.Error, "error");
                return;
            }

            TTSVoiceConvert.Voice = voice_sp;
            TTSVoiceConvert.Speak("а", SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
            System.Threading.Thread.Sleep(100);

            reader = new StreamReader(file, Encoding.UTF8);
            toSpeak = reader.ReadToEnd();
            SpeechStreamFileMode SpFileMode = SpeechStreamFileMode.SSFMCreateForWrite;
            SpFileStream SpFileStream = new SpFileStream();
            // http://www.autoitscript.com/forum/topic/133903-need-help-with-text-to-speech-output-format/
            // http://msdn.microsoft.com/en-us/library/ms717276(VS.85).aspx
            SpFileStream.Format.Type = SpeechLib.SpeechAudioFormatType.SAFT48kHz16BitMono;
            SpFileStream.Open(file.Replace(".xml", ".wav"), SpFileMode, false);
            TTSVoiceConvert.Rate = SpeechRate;
            TTSVoiceConvert.Volume = SpeechVolume;
            TTSVoiceConvert.AudioOutputStream = SpFileStream;
            TTSVoiceConvert.Speak(toSpeak, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML);
            TTSVoiceConvert.WaitUntilDone(Timeout.Infinite);
            SpFileStream.Close();
            reader.Close();
        }

        public int wav2mp3(string FileName, string title, string artist, string album, string track, int summed_secs)
        {
            string pworkingDir = Path.GetDirectoryName(Application.ExecutablePath);
            string wavpath = FileName.Replace(".xml", ".wav");
            string mp3path = FileName.Replace(".xml", ".mp3");
            int read = 0;
            int current_secs;
            int summed_secs_new;

            WaveStream InStr = new WaveStream(wavpath);

            // calculate the length of the wav file in seconds
            //  http://stackoverflow.com/questions/82319/how-can-i-determine-the-length-of-a-wav-file-in-c
            current_secs = (int)(InStr.Length * 8 / (InStr.Format.nSamplesPerSec * InStr.Format.nChannels * InStr.Format.wBitsPerSample));
            summed_secs_new = summed_secs + current_secs;

            // add the summed length to the title of the mp3
            TimeSpan t = TimeSpan.FromSeconds(summed_secs_new);
            string position_secs = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours + t.Days * 24, t.Minutes, t.Seconds);
            title = title + " " + position_secs;

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

            // edit id3 tags: http://www.idsharp.com/
            IdSharp.Tagging.ID3v2.IID3v2Tag id3v2 = new ID3v2Tag(mp3path);
            id3v2.Header.TagVersion = ID3v2TagVersion.ID3v22;
            id3v2.Title = title;
            id3v2.Album = album;
            id3v2.Artist = artist;
            id3v2.TrackNumber = track;
            id3v2.Save(mp3path);

            return summed_secs_new;
        }

        private void bApplyBatch_Click(object sender, System.EventArgs e)
        {
            WriteSettingsIni();
        }

        // This event occurs when the user drags over the form with 
        // the mouse during a drag drop operation 
        void Form_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the Dataformat of the data can be accepted
            // (we only accept file drops from Explorer, etc.)
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy; // Okay
            else
                e.Effect = DragDropEffects.None; // Unknown data, ignore it

        }

        // Occurs when the user releases the mouse over the drop target 
        void Form_DragDrop(object sender, DragEventArgs e)
        {
            // Extract the data from the DataObject-Container into a string list
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            tbSource.Text = FileList[0].ToString();
        }
    }
}