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

        private void ReadClipboard(string langid, string voice, int srate, int volume)
        {
            string toRead;
            SpObjectToken voice_sp = speech.GetVoices("Name=" + voice, "Language=" + langid).Item(0);
            paused = false;
            toRead = Clipboard.GetText();
            if (cbSilence.Checked)
            {
                // ad 50ms silence on new line
                toRead = toRead.Replace("\r\n", "<lang langid=\"409\"><silence msec=\"50\" /></lang>"); // new line -> pause for 50ms
            }
            else
            {
                // no silence on new line
                toRead = toRead.Replace("\r\n", " ");
            }

            foreach (DictionaryEntry entry in ligatures)
            {
                toRead = toRead.Replace(entry.Key.ToString(), entry.Value.ToString());
            }

            toRead = "<lang langid=\"" + langid + "\"><pitch middle='0'>" + toRead + "</pitch></lang>";
            speech.Rate = srate;
            speech.Volume = volume;
            speech.Voice = voice_sp;
            speech.Resume();
            speech.Speak(toRead, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
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