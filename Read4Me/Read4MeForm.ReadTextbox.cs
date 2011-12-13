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
            if (paused)
            {
                speech_cpRead.Resume();
                paused = false;
            }
            else
            {
                speech_cpRead.Rate = speechRate;
                speech_cpRead.Volume = volume_global;
                speech_cpRead.Speak(tbspeech.Text, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML);
            }
        }

        private void cmbVoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            speech_cpRead.Voice = speech_cpRead.GetVoices(string.Empty, string.Empty).Item(cmbVoices.SelectedIndex);
        }

        private void tbarRate_Scroll(object sender, EventArgs e)
        {
            speechRate = tbarRate.Value;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            speech_cpRead.Pause();
            paused = true;
        }


        private void trbVolume_Scroll(object sender, EventArgs e)
        {
            volume_global = trbVolume.Value;
        }
    }
}