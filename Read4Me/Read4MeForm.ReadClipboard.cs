using System.Windows.Forms;
using SpeechLib;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using LiteMiner.classes;
using System.Globalization;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;
using System.Speech.Synthesis;

namespace Read4Me
{
    partial class Read4MeForm
    {
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static public extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        private void SendCtrlC(IntPtr hWnd)
        {
            uint KEYEVENTF_KEYUP = 2;
            byte VK_CONTROL = 0x11;
            SetForegroundWindow(hWnd);
            keybd_event(VK_CONTROL, 0, 0, 0);
            keybd_event(0x43, 0, 0, 0); //Send the C key (43 is "C")
            keybd_event(0x43, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);// 'Left Control Up
        }

        // http://www.radsoftware.com.au/articles/clipboardmonitor.aspx
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0308:
                    // MessageBox.Show(Clipboard.GetText());
                    SendMessage(_ClipboardViewerNext, m.Msg, m.WParam, m.LParam);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        // http://msdn.microsoft.com/en-us/library/jj572477(v=office.14).aspx
        private void SpeechSkip(int items)
        {
            if (CompatGlobal)
            {
                if (TTSVoiceClipboardSynth.State.ToString() == "Speaking")
                {
                    TTSVoiceClipboardSynth.SpeakAsyncCancelAll();
                    if (items < 0)
                    {
                        if (toReadSentenceNum + items >= 0)
                        {
                            toReadSentenceNum = toReadSentenceNum + items;
                        }
                    }
                    else
                    {
                        if (toReadSentenceNum + items <= toReadSentences.Length - 1)
                        {
                            toReadSentenceNum = toReadSentenceNum + items;
                        }
                    }
                    continueReading();
                }
                else
                {
                    if (items < 0)
                    {
                        if (toReadSentenceNum + items >= 0)
                        {
                            toReadSentenceNum = toReadSentenceNum + items;
                        }
                    }
                    else
                    {
                        if (toReadSentenceNum + items <= toReadSentences.Length - 1)
                        {
                            toReadSentenceNum = toReadSentenceNum + items;
                        }
                    }
                }
            }
            else
            {
                if (TTSVoiceClipboard.Status.RunningState == 0)
                {
                    // if paused
                    int volume = TTSVoiceClipboard.Volume;
                    TTSVoiceClipboard.Volume = 0;
                    TTSVoiceClipboard.Resume();
                    while (!(TTSVoiceClipboard.Status.RunningState == SpeechRunState.SRSEIsSpeaking))
                    {
                    }
                    TTSVoiceClipboard.Skip("Sentence", items);
                    TTSVoiceClipboard.Pause();
                    while (TTSVoiceClipboard.Status.RunningState == SpeechRunState.SRSEIsSpeaking)
                    {
                    }
                    TTSVoiceClipboard.Volume = volume;
                }
                else
                {
                    TTSVoiceClipboard.Skip("Sentence", items);
                }
            }
        }

        private void SpeechPause()
        {
            if (CompatGlobal)
            {
                if (TTSVoiceClipboardSynth.State.ToString() == "Speaking")
                {
                    TTSVoiceClipboardSynth.Pause();
                }
                else
                {
                    TTSVoiceClipboardSynth.Resume();
                }
            }
            else
            {
                if (TTSVoiceClipboard.Status.RunningState == SpeechRunState.SRSEIsSpeaking)
                {
                    TTSVoiceClipboard.Pause();
                }
                else
                {
                    if (TTSVoiceClipboard.Status.RunningState == 0)
                    {
                        TTSVoiceClipboard.Resume();
                    }
                }
            }
        }

        private void ChangeTTSSpeed(int speed)
        {
            if (CompatGlobal)
            {
                if (TTSVoiceClipboardSynth.State.ToString() == "Speaking")
                {
                    TTSVoiceClipboardSynth.SpeakAsyncCancelAll();
                    if (TTSVoiceClipboardSynth.Rate < 10 && speed > 0)
                    {
                        TTSVoiceClipboardSynth.Rate = TTSVoiceClipboardSynth.Rate + speed;
                    }
                    if (TTSVoiceClipboardSynth.Rate > -10 && speed < 0)
                    {
                        TTSVoiceClipboardSynth.Rate = TTSVoiceClipboardSynth.Rate + speed;
                    }
                    SetBalloonTip("TTS Speech rate", "TTS Speech rate set to " + TTSVoiceClipboardSynth.Rate.ToString(), ToolTipIcon.Info, "info");
                    continueReading();
                }
            }
            else
            {
                if (TTSVoiceClipboard.Status.RunningState == SpeechRunState.SRSEIsSpeaking)
                {
                    TTSVoiceClipboard.Pause();
                    if (TTSVoiceClipboard.Rate < 10 && speed > 0)
                    {
                        TTSVoiceClipboard.Rate = TTSVoiceClipboard.Rate + speed;
                    }
                    if (TTSVoiceClipboard.Rate > -10 && speed < 0)
                    {
                        TTSVoiceClipboard.Rate = TTSVoiceClipboard.Rate + speed;
                    }
                    SetBalloonTip("TTS Speech rate", "TTS Speech rate set to " + TTSVoiceClipboard.Rate.ToString(), ToolTipIcon.Info, "info");
                    TTSVoiceClipboard.Resume();
                }
            }
        }

        string[] toReadSentences;
        int toReadSentenceNum;
        char[] toReadSplitChar = { '.', '?', '!', ':', (char)10 }; // 10 is new line
        private void ReadWithTTS(string voice, int srate, int volume, bool compat, bool readClipboard, string toReadInput)
        {
            SpObjectToken voice_sp = null;
            int i = 0;
            bool found = false;
            string toRead = "";

            if (readClipboard)
            {
                // IDataObject OldClipboard = null;
                if (cbReadSelectedText.Checked)
                {
                    // OldClipboard = Clipboard.GetDataObject();
                    SendCtrlC(GetForegroundWindow());
                    Thread.Sleep(100); // it takes some time until the content arrives in the clipboard
                }

                // get clipboard content
                toRead = Clipboard.GetText();
            }
            else
            {
                toRead = toReadInput;
            }

            // no silence on new line
            toRead = toRead.Replace("\t", " ").Replace("\n", " ").Replace("\r", " ");
            toRead = toRead.Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ");//normalize multiple spaces

            // remove ligatures
            foreach (DictionaryEntry entry in ligatures)
            {
                toRead = toRead.Replace(entry.Key.ToString(), entry.Value.ToString());
            }

            // get all available voices
            // for some reason this doesn't work always: ISpeechObjectTokens setVoices = TTSVoiceClipboard.GetVoices("Name=" + ComboboxesVoiceCB[setVoiceNum].SelectedItem, string.Empty);
            ISpeechObjectTokens AvailableVoices = TTSVoiceClipboard.GetVoices(string.Empty, string.Empty);

            if (voice == "Detect language")
            {
                LanguageDetector ld = new LanguageDetector();
                string lanCode = ld.Detect(toRead);
                if (lanCode == null)
                {
                    SetBalloonTip("Error", "The language could not be detected.", ToolTipIcon.Error, "error");
                    return;
                }

                for (int setVoiceNum = 0; setVoiceNum < ComboboxesVoiceCB.Count; setVoiceNum++)
                {
                    i = 0;
                    foreach (ISpeechObjectToken Token in AvailableVoices)
                    {
                        if (ComboboxesVoiceCB[setVoiceNum].SelectedItem.ToString() == Token.GetDescription(0))
                        {
                            voice_sp = AvailableVoices.Item(i);

                            // for voices supporting multiple languages, they are separated by ";"
                            string[] langs = voice_sp.GetAttribute("Language").Split(';');
                            foreach (string lang in langs)
                            {
                                CultureInfo myCItrad = new CultureInfo(int.Parse(lang, System.Globalization.NumberStyles.HexNumber), false);
                                if (lanCode == myCItrad.TwoLetterISOLanguageName)
                                {
                                    volume = Int16.Parse(ComboboxesVolumeCB[setVoiceNum].SelectedItem.ToString());
                                    srate = Int16.Parse(ComboboxesRateCB[setVoiceNum].SelectedItem.ToString());
                                    compat = CheckboxesCompat[setVoiceNum].Checked;
                                    found = true;
                                    break;
                                }
                            }
                        }
                        if (found)
                        {
                            break;
                        }
                        i++;
                    }
                }

                if (!found)
                {
                    SetBalloonTip("Language detection", "The language was detected as " + ld.GetLanguageNameByCode(lanCode) + ", but no appropriate TTS voice available.", ToolTipIcon.Error, "error");
                    return;
                }
            }

            i = 0;
            foreach (ISpeechObjectToken Token in AvailableVoices)
            {
                if (voice == Token.GetDescription(0))
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

            if (compat)
            {
                TTSVoiceClipboard.Pause();

                //if (TTSVoiceClipboardSynth.State.ToString() == "Paused")
                //{
                TTSVoiceClipboardSynth.Resume();
                //}

                toReadSentenceNum = 0;
                toReadSentences = toRead.Split(toReadSplitChar);

                // start tts
                TTSVoiceClipboardSynth.SpeakAsyncCancelAll();
                TTSVoiceClipboardSynth.SelectVoice(voice_sp.GetAttribute("Name"));
                TTSVoiceClipboardSynth.Rate = srate;
                TTSVoiceClipboardSynth.Volume = volume;
                TTSVoiceClipboardSynth.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(TTSVoiceClipboardSynth_SpeakCompleted);

                TTSVoiceClipboardSynth.SpeakAsync(toReadSentences[0]);
                CompatGlobal = compat;
            }
            else
            {
                TTSVoiceClipboardSynth.Pause();

                //if (TTSVoiceClipboard.Status.RunningState == 0 || TTSVoiceClipboard.Status.RunningState == SpeechRunState.SRSEDone)
                //{
                // need to detect and get out of paused state
                // returns 0 when paused...
                TTSVoiceClipboard.Resume();
                //}

                // init TTS
                TTSVoiceClipboard.Rate = 10;
                TTSVoiceClipboard.Volume = 0;
                TTSVoiceClipboard.Voice = voice_sp;
                TTSVoiceClipboard.Speak("а", SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
                System.Threading.Thread.Sleep(100);

                //MessageBox.Show("nedone");
                TTSVoiceClipboard.Rate = srate;
                TTSVoiceClipboard.Volume = volume;
                TTSVoiceClipboard.Voice = voice_sp;
                TTSVoiceClipboard.Speak(toRead, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
                CompatGlobal = compat;
            }
        }

        void continueReading()
        {
            TTSVoiceClipboardSynth.SpeakAsync(toReadSentences[toReadSentenceNum]);
        }

        void TTSVoiceClipboardSynth_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                if (toReadSentenceNum < toReadSentences.Length - 1)
                {
                    toReadSentenceNum++;
                    continueReading();
                }
            }
        }

        private void InitLigatures()
        {
            // http://www.softerviews.org/LibreOffice.html#Remove_Ligatures
            // http://en.wikipedia.org/wiki/List_of_precomposed_Latin_characters_in_Unicode#Ligatures
            ligatures.Add("", "ck");
            ligatures.Add("", "ct");
            ligatures.Add("", "fj");
            ligatures.Add("", "fr");
            ligatures.Add("", "fft");
            ligatures.Add("", "ffy");
            ligatures.Add("", "fty");
            ligatures.Add("", "ft");
            ligatures.Add("", "fy");
            ligatures.Add("", "sp");
            ligatures.Add("", "tr");
            ligatures.Add("", "tt");
            ligatures.Add("", "ty");
            ligatures.Add("", "tz");
            ligatures.Add("", "Qu");
            ligatures.Add("", "Th");
            ligatures.Add("ﬃ", "ffi");
            ligatures.Add("ﬄ", "ffl");
            ligatures.Add("ﬁ", "fi");
            ligatures.Add("ﬂ", "fl");
            ligatures.Add("ﬀ", "ff");
            ligatures.Add("ﬅ", "ſt");
            ligatures.Add("ﬆ", "st");
            ligatures.Add("Æ", "AE");
            ligatures.Add("æ", "ae");
            ligatures.Add("ᵫ", "ue");
        }
    }
}