using System.Windows.Forms;
using SpeechLib;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Read4Me
{
    partial class Read4MeForm
    {
        private void SpeechSkip(int items)
        {
            TTSVoiceClipboard.Skip("Sentence", items);
        }

        private void SpeechStop()
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
            ISpeechObjectTokens AvailableVoices = TTSVoiceClipboard.GetVoices(string.Empty, string.Empty);
            foreach (ISpeechObjectToken Token in AvailableVoices)
            {
                if (voice == Token.GetDescription(49))
                {
                    voice_sp = AvailableVoices.Item(i);
                    break;
                }
                i++;
            }

            if (voice_sp == null)
            {
                MessageBox.Show("Error! Voice not found!");
                return;
            }

            PausedGlobal = false;
            toRead = Clipboard.GetText();

            // init TTS
            TTSVoiceClipboard.Rate = 10;
            TTSVoiceClipboard.Volume = 0;
            TTSVoiceClipboard.Voice = voice_sp;
            TTSVoiceClipboard.Speak("а", SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
            System.Threading.Thread.Sleep(100);

            // no silence on new line
            toRead = toRead.Replace("\r\n", " ");

            foreach (DictionaryEntry entry in ligatures)
            {
                toRead = toRead.Replace(entry.Key.ToString(), entry.Value.ToString());
            }
            
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