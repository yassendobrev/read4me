using System.Windows.Forms;
using SpeechLib;
using System.IO;

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
                toRead = toRead.Replace("\r\n", "");
            }
            toRead = "<lang langid=\"" + langid + "\"><pitch middle='0'>" + toRead + "</pitch></lang>";
            speech.Rate = srate;
            speech.Volume = volume;
            speech.Voice = voice_sp;
            speech.Resume();
            speech.Speak(toRead, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
        }
    }
}