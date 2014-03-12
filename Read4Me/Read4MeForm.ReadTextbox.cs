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
                TTSVoiceClipboard.Resume();
                TTSVoiceClipboard.Speak(tbspeech.Text, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
            }
        }

        private void cmbVoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SpeechVoiceGlobal = TTSVoiceClipboard.GetVoices(string.Empty, string.Empty).Item(cmbVoices.SelectedIndex);

            if (cmbVoices.SelectedItem.ToString() != "")
            {
                int i = 0;
                ISpeechObjectTokens AvailableVoices = TTSVoiceClipboard.GetVoices(string.Empty, string.Empty);
                foreach (ISpeechObjectToken Token in AvailableVoices)
                {
                    if (cmbVoices.SelectedItem.ToString() == Token.GetDescription(0))
                    {
                        SpeechVoiceGlobal = AvailableVoices.Item(i);
                        break;
                    }
                    i++;
                }

                if (SpeechVoiceGlobal == null)
                {
                    SetBalloonTip("Error", "Error! Voice not found!", ToolTipIcon.Error, "error");
                    return;
                }
            }
        }

        private void tbarRate_Scroll(object sender, EventArgs e)
        {
            SpeechRateGlobal = tbarRate.Value;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (!PausedGlobal)
            {
                TTSVoiceClipboard.Pause();
                PausedGlobal = true;
            }
        }

        private void trbVolume_Scroll(object sender, EventArgs e)
        {
            VolumeGlobal = trbVolume.Value;
        }
    }
}