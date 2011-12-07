using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.IO;

namespace Read4Me
{
    public partial class Read4MeForm
    {
        private void checkForUpdateToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            string current_version;

            WebClient webClient = new WebClient();
            string version_txt_path = Environment.CurrentDirectory + "\\current_version.txt";
            if (File.Exists(version_txt_path))
            {
                try
                {
                    File.Delete(version_txt_path);
                }
                catch
                {
                }
            }

            StreamReader file_reader;
            try
            {
                webClient.DownloadFile("http://sourceforge.net/p/read4mecbr/code/ci/master/tree/current_version.txt?format=raw", version_txt_path);
                try
                {
                    file_reader = new StreamReader(version_txt_path, Encoding.UTF8);
                    current_version = file_reader.ReadLine();
                    if (current_version != local_version)
                    {
                        UpdateDialog dialog = new UpdateDialog();
                        dialog.ShowDialog(this);
                        dialog.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("No new version available.");
                    }
                    file_reader.Close();
                    if (File.Exists(version_txt_path))
                    {
                        try
                        {
                            File.Delete(version_txt_path);
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Error while determining version.");
                }

            }
            catch
            {
                MessageBox.Show("Couldn't connect to server.");
            }
        }
    }
}
