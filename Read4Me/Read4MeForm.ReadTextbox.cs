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

                // get clipboard content
                toRead = tbspeech.Text;
                
                // no silence on new line
                toRead = toRead.Replace("\t", " ").Replace("\n", " ").Replace("\r", " ");
                toRead = toRead.Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ");//normalize multiple spaces

                // remove ligatures
                foreach (DictionaryEntry entry in ligatures)
                {
                    toRead = toRead.Replace(entry.Key.ToString(), entry.Value.ToString());
                }

                TTSVoiceClipboard.Speak(toRead, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
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