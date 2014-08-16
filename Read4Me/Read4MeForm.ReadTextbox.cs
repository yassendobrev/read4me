using System;
using System.Windows.Forms;
using SpeechLib;
using System.Threading;
using System.Collections;

namespace Read4Me
{
    partial class Read4MeForm
    {
        private void btnSpeak_Click(object sender, EventArgs e)
        {
            // start reading
            ReadWithTTS(SpeechVoiceGlobal, SpeechRateGlobal, VolumeGlobal, cbCompTB.Checked, false, tbspeech.Text);
        }

        private void cmbVoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            SpeechVoiceGlobal = cmbVoices.SelectedItem.ToString();
        }

        private void tbarRate_Scroll(object sender, EventArgs e)
        {
            SpeechRateGlobal = tbarRate.Value;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            SpeechPause();
        }

        private void trbVolume_Scroll(object sender, EventArgs e)
        {
            VolumeGlobal = trbVolume.Value;
        }
    }
}