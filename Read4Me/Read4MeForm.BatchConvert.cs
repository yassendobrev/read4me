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

            int SpeechRate = Int16.Parse(cbRateBatch.SelectedItem.ToString());
            int SpeechVolume = Int16.Parse(cbVolumeBatch.SelectedItem.ToString());
            string LangID = (string)langids[cbLanguageBatch.SelectedItem.ToString()];
            SpObjectToken SpeechVoice = speech.GetVoices(string.Empty, string.Empty).Item(cbVoiceBatch.SelectedIndex);
            string FilePath = tbSource.Text;
            
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

            // check if old directories exist and delete them
            if (Directory.Exists(outdir + "xml"))
            {
                Directory.Delete(outdir + "xml", true);
            }
            if (Directory.Exists(outdir + "mp3"))
            {
                Directory.Delete(outdir + "mp3", true);
            }
            
            try
            {
                Directory.CreateDirectory(outdir + "xml");
            }
            catch
            {
            }

            file_writer = new StreamWriter(outdir + "xml\\" + filename.ToString("00") + ".xml", false, Encoding.UTF8);
            file_writer.Write("<lang langid=\"" + LangID + "\">");
            while (!file_reader.EndOfStream)
            {
                line = file_reader.ReadLine().Replace("\t", "");
                line = line.Replace(" ", " "); // replace Non-breaking space 0xA0 with normal space 0x20
                line = line.Replace("[", "(");
                line = line.Replace("]", ")");
                line = line.Replace("<", "(");
                line = line.Replace(">", ")");
                line = line.Replace("«", "(");
                line = line.Replace("»", ")");

                // SLOWER RATE WHEN words surrounded with _words_ -> emphasize!
                // use <rate absspeed="-7"/>
                line_parts = line.Split('_');
                if (line_parts.Length > 1)
                {
                    line = "";
                    for (int i = 0; i < line_parts.Length - 1; i++)
                    {
                        if (i % 2 == 0)
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
                    // line = line + "<rate absspeed=\"" + speechRate + "\"/>"; // make sure normal rate continues --> too fast???
                    line = line + "<rate absspeed=\"0\"/>"; // make sure normal rate continues
                }

                if (line != "")
                {
                    empty_lines = 0;
                    if (Regex.IsMatch(line, "^" + Regex.Escape(tbBefore.Text) + "[IVX0-9]+" + Regex.Escape(tbAfter.Text) + "$", RegexOptions.IgnoreCase) || start_new_file)
                    {
                        start_new_file = false;
                        file_writer.Write("<lang langid=\"409\"><silence msec=\"200\" /></lang>"); // longer pause at the end of chapter
                        file_writer.Write("</lang>");
                        file_writer.Close();
                        // filename = Regex.Replace(line, @"[\D]", "");
                        filename++;
                        file_writer = new StreamWriter(outdir + "xml\\" + filename.ToString("00") + ".xml", false, Encoding.UTF8);

                        file_writer.Write("<lang langid=\"" + LangID + "\">");
                        file_writer.Write(line);
                        file_writer.Write("<lang langid=\"409\"><silence msec=\"200\" /></lang>"); // longer pause after announcing a chapter
                    }
                    else
                    {
                        file_writer.Write(line);
                        if (cbSilenceBatch.Checked)
                        {
                            file_writer.Write("<lang langid=\"409\"><silence msec=\"50\" /></lang>");
                        }
                        else
                        {
                            file_writer.Write(" ");
                        }
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
            BatchConvert(outdir + "xml", SpeechRate, SpeechVolume, SpeechVoice);
        }

        private void BatchConvert(string folder, int SpeechRate, int SpeechVolume, SpObjectToken SpeechVoice)
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
                SpeakText(FileName, SpeechRate, SpeechVolume, SpeechVoice);

                // prepare id3 tags
                title = FileName.Replace(folder + "\\", "").Replace(".xml", "");
                track = FileName.Replace(folder + "\\", "").Replace(".xml", ""); // files are numbered 1..n

                // convert .wav to .mp3
                wav2mp3(FileName, title, artist, year + " " + album, track);
                File.Delete(FileName.Replace(".xml", ".wav"));
                Application.DoEvents();
            }
            Directory.Move(folder.Replace("xml", "mp3"), folder.Replace("xml", artist + " - " + year + " " + album));
            Directory.Delete(folder);

            MessageBox.Show("All done!");
        }

        public void SpeakText(string file, int SpeechRate, int SpeechVolume, SpObjectToken SpeechVoice)
        {
            StreamReader reader;
            string toSpeak;

            reader = new StreamReader(file, Encoding.UTF8);
            toSpeak = reader.ReadToEnd();
            SpeechStreamFileMode SpFileMode = SpeechStreamFileMode.SSFMCreateForWrite;
            SpFileStream SpFileStream = new SpFileStream();
            SpFileStream.Open(file.Replace(".xml", ".wav"), SpFileMode, false);
            speech.AudioOutputStream = SpFileStream;
            speech.Rate = SpeechRate;
            speech.Volume = SpeechVolume;
            speech.Voice = SpeechVoice;
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

            // edit id3 tags: http://www.idsharp.com/
            IdSharp.Tagging.ID3v2.IID3v2Tag id3v2 = new ID3v2Tag(mp3path);
            id3v2.Header.TagVersion = ID3v2TagVersion.ID3v22;
            id3v2.Title = title;
            id3v2.Album = album;
            id3v2.Artist = artist;
            id3v2.TrackNumber = track;
            id3v2.Save(mp3path);
            /*
             * http://stackoverflow.com/questions/82319/how-can-i-determine-the-length-of-a-wav-file-in-c
            MediaPlayer mPlayer = new MediaPlayer();
            mPlayer.Open(new Uri(mp3path));
            // mPlayer.Play();
            string a = mPlayer.Source.ToString();
            MessageBox.Show(a);
            if (mPlayer.NaturalDuration.HasTimeSpan)
            {
                MessageBox.Show(mPlayer.NaturalDuration.TimeSpan.ToString());
            }
            mPlayer.Close();
            */
        }

        private void bApplyBatch_Click(object sender, System.EventArgs e)
        {
            WriteSettings();
        }
    }
}