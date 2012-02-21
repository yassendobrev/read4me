using System;
using System.Windows.Forms;
using SpeechLib;
using System.Threading;

namespace Read4Me
{
    partial class Read4MeForm
    {
        private void btnSpeak_Click(object sender, EventArgs e)
        {
            if (PausedGlobal)
            {
                TTSVoiceClipboard.Resume();
                PausedGlobal = false;
            }
            else
            {
                TTSVoiceClipboard.Voice = SpeechVoiceGlobal;
                TTSVoiceClipboard.Rate = SpeechRateGlobal;
                TTSVoiceClipboard.Volume = VolumeGlobal;
                TTSVoiceClipboard.Speak(tbspeech.Text, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML);
            }
        }

        private void cmbVoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            SpeechVoiceGlobal = TTSVoiceClipboard.GetVoices(string.Empty, string.Empty).Item(cmbVoices.SelectedIndex);
        }

        private void tbarRate_Scroll(object sender, EventArgs e)
        {
            SpeechRateGlobal = tbarRate.Value;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            TTSVoiceClipboard.Pause();
            PausedGlobal = true;
        }

        private void trbVolume_Scroll(object sender, EventArgs e)
        {
            VolumeGlobal = trbVolume.Value;
        }
    }
}