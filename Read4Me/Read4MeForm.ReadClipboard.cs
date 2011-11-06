using System.Windows.Forms;
using SpeechLib;

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

        private void ReadClipboard(string langid, string voice)
        {
            int srate;
            string toRead;
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
            toRead = "<lang langid=\"" + langid + "\"><pitch middle='0'>" + toRead + "</pitch></lang>";
            speech.Rate = srate;
            speech.Volume = volume;
            speech.Voice = voice_sp;
            speech.Resume();
            speech.Speak(toRead, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
        }
    }
}