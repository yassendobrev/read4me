using System.Windows.Forms;
using SpeechLib;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using LiteMiner.classes;
using System.Globalization;
using System;

namespace Read4Me
{
    partial class Read4MeForm
    {
        private void SpeechSkip(int items)
        {
            if (PausedGlobal)
            {
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

        private void SpeechPause()
        {
            if (TTSVoiceClipboard.Status.RunningState == SpeechRunState.SRSEIsSpeaking)
            {
                TTSVoiceClipboard.Pause();
                PausedGlobal = true;
            }
            else
            {
                if (PausedGlobal == true)
                {
                    TTSVoiceClipboard.Resume();
                    PausedGlobal = false;
                }
            }
        }

        private void ReadClipboard(string voice, int srate, int volume)
        {
            string toRead;
            SpObjectToken voice_sp = null;
            int i = 0;
            bool found = false;

            // get clipboard content
            toRead = Clipboard.GetText();

            // no silence on new line
            toRead = toRead.Replace("\r\n", " ");

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
                            CultureInfo myCItrad = new CultureInfo(int.Parse(voice_sp.GetAttribute("Language"), System.Globalization.NumberStyles.HexNumber), false);
                            if (lanCode == myCItrad.TwoLetterISOLanguageName)
                            {
                                found = true;
                                break;
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

            PausedGlobal = false;

            // init TTS
            TTSVoiceClipboard.Rate = 10;
            TTSVoiceClipboard.Volume = 0;
            TTSVoiceClipboard.Voice = voice_sp;
            TTSVoiceClipboard.Speak("а", SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
            System.Threading.Thread.Sleep(100);
            
            TTSVoiceClipboard.Rate = srate;
            TTSVoiceClipboard.Volume = volume;
            TTSVoiceClipboard.Voice = voice_sp;
            TTSVoiceClipboard.Resume();
            // TTSVoiceClipboard.Speak(toRead, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
            TTSVoiceClipboard.Speak(toRead, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
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