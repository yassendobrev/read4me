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
                speech_cpRead.Resume();
                PausedGlobal = false;
            }
            else
            {
                speech_cpRead.Voice = SpeechVoiceGlobal;
                speech_cpRead.Rate = SpeechRateGlobal;
                speech_cpRead.Volume = VolumeGlobal;
                speech_cpRead.Speak(tbspeech.Text, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML);
            }
        }

        private void cmbVoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            SpeechVoiceGlobal = speech_cpRead.GetVoices(string.Empty, string.Empty).Item(cmbVoices.SelectedIndex);
        }

        private void tbarRate_Scroll(object sender, EventArgs e)
        {
            SpeechRateGlobal = tbarRate.Value;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            speech_cpRead.Pause();
            PausedGlobal = true;
        }

        private void trbVolume_Scroll(object sender, EventArgs e)
        {
            VolumeGlobal = trbVolume.Value;
        }
    }
}