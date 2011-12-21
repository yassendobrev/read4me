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
        Thread oThread;

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
            try
            {
                if (oThread.IsAlive)
                {
                    MessageBox.Show("Current conversion still running. Please wait.");
                    return;
                }
            }
            catch
            {
                // thread not initialized
            }

            int SpeechRate;
            int SpeechVolume;
            string LangID;
            SpObjectToken SpeechVoice;
            string FilePath;
            string artist;
            string album;
            string year;

            try
            {
                SpeechRate = Int16.Parse(cbRateBatch.SelectedItem.ToString());
                SpeechVolume = Int16.Parse(cbVolumeBatch.SelectedItem.ToString());
                LangID = (string)langids[cbLanguageBatch.SelectedItem.ToString()];
                SpeechVoice = speech_cpRead.GetVoices(string.Empty, string.Empty).Item(cbVoiceBatch.SelectedIndex);
                FilePath = tbSource.Text;
            }
            catch
            {
                MessageBox.Show("Error setting voice.");
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
            oThread = new Thread(() => this.doConvert(SpeechRate, SpeechVolume, LangID, SpeechVoice, FilePath, artist, album, year));
            oThread.Start();
        }

        private void doConvert(int SpeechRate, int SpeechVolume, string LangID, SpObjectToken SpeechVoice, string FilePath, string artist, string album, string year)
        {
            string outdir;
            int filename = 1;
            string line;
            int empty_lines = 0;
            bool start_new_file = false;
            string[] line_parts;
            string artist_year_album;

            try
            {
                file_reader = new StreamReader(FilePath, Encoding.UTF8);
            }
            catch
            {
                MessageBox.Show("Error opening files!");
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
                try
                {
                    Directory.Delete(outdir, true);
                }
                catch
                {
                    MessageBox.Show("Output directory exists and could not be deleted.");
                    return;
                }
            }

            try
            {
                Directory.CreateDirectory(outdir);
            }
            catch
            {
                MessageBox.Show("Output directory " + outdir + " could not be created.");
                return;
            }

            try
            {
                file_writer = new StreamWriter(outdir + "\\" + filename.ToString("000") + ".xml", false, Encoding.UTF8);
            }
            catch
            {
                MessageBox.Show("Could not open .xml file for writing.");
                return;
            }

            file_writer.Write("<lang langid=\"" + LangID + "\">");
            while (!file_reader.EndOfStream)
            {
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
                    if (start_new_file)
                    {
                        start_new_file = false;
                        file_writer.Write("<lang langid=\"409\"><silence msec=\"200\" /></lang>"); // longer pause at the end of chapter
                        file_writer.Write("</lang>");
                        file_writer.Close();
                        filename++;

                        try
                        {
                            file_writer = new StreamWriter(outdir + "\\" + filename.ToString("000") + ".xml", false, Encoding.UTF8);
                        }
                        catch
                        {
                            MessageBox.Show("Could not open .xml file for writing during xml conversion.");
                            return;
                        }

                        file_writer.Write("<lang langid=\"" + LangID + "\">");
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
                    if (empty_lines >= 1 || cbBreakFiles.Checked)
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
            BatchConvert(outdir, SpeechRate, SpeechVolume, SpeechVoice, artist, album, year);
        }

        private void BatchConvert(string folder, int SpeechRate, int SpeechVolume, SpObjectToken SpeechVoice, string artist, string album, string year)
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
                MessageBox.Show("No folder for batch!");
                return;
            }

            int i = 1;
            foreach (string FileName in fileEntries)
            {
                Application.DoEvents();
                this.Invoke((MethodInvoker)delegate
                {
                    sWorkingStatus.Text = "Working on file " + i.ToString() + " from " + fileEntries.Length.ToString();
                });
                i++;
                Application.DoEvents();

                // convert xml to wav
                SpeakText(FileName, SpeechRate, SpeechVolume, SpeechVoice);

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
            MessageBox.Show("All done!");
        }

        public void SpeakText(string file, int SpeechRate, int SpeechVolume, SpObjectToken SpeechVoice)
        {
            StreamReader reader;
            string toSpeak;
            SpVoice speech_Convert = new SpVoice();

            // init TTS
            speech_Convert.Rate = 10;
            speech_Convert.Volume = 0;
            speech_Convert.Speak("<lang langid=\"402\">а</lang>", SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);

            reader = new StreamReader(file, Encoding.UTF8);
            toSpeak = reader.ReadToEnd();
            SpeechStreamFileMode SpFileMode = SpeechStreamFileMode.SSFMCreateForWrite;
            SpFileStream SpFileStream = new SpFileStream();
            SpFileStream.Open(file.Replace(".xml", ".wav"), SpFileMode, false);
            speech_Convert.Rate = SpeechRate;
            speech_Convert.Volume = SpeechVolume;
            speech_Convert.Voice = SpeechVoice;
            speech_Convert.AudioOutputStream = SpFileStream;
            // speech.Speak(FileName, SpeechVoiceSpeakFlags.SVSFIsFilename); // not working properly
            speech_Convert.Speak(toSpeak, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML);
            speech_Convert.WaitUntilDone(Timeout.Infinite);
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
            string position_secs = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds);
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
            WriteSettings();
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