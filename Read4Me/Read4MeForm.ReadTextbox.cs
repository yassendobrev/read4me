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
                speech.Resume();
                paused = false;
            }
            else
            {
                speech.Rate = speechRate;
                speech.Volume = volume_global;
                speech.Speak(tbspeech.Text, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML);
            }
        }

        private void btnToWAV_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "All files (*.*)|*.*|wav files (*.wav)|*.wav";
                sfd.Title = "Save to a wave file";
                sfd.FilterIndex = 2;
                sfd.RestoreDirectory = true;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SpeechStreamFileMode SpFileMode = SpeechStreamFileMode.SSFMCreateForWrite;
                    SpFileStream SpFileStream = new SpFileStream();
                    SpFileStream.Open(sfd.FileName, SpFileMode, false);
                    speech.AudioOutputStream = SpFileStream;
                    speech.Rate = speechRate;
                    speech.Volume = volume_global;
                    speech.Speak(tbspeech.Text, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML);
                    speech.WaitUntilDone(Timeout.Infinite);
                    SpFileStream.Close();
                }
            }
            catch
            {
                MessageBox.Show("There is some error in converting to Wav file.");
            }
        }

        private void cmbVoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            speech.Voice = speech.GetVoices(string.Empty, string.Empty).Item(cmbVoices.SelectedIndex);
        }

        private void tbarRate_Scroll(object sender, EventArgs e)
        {
            speechRate = tbarRate.Value;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            speech.Pause();
            paused = true;
        }


        private void trbVolume_Scroll(object sender, EventArgs e)
        {
            volume_global = trbVolume.Value;
        }
    }
}