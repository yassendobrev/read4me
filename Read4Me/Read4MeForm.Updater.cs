using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Read4Me
{
    public partial class Read4MeForm
    {
        private void checkForUpdateToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            ThreadedUpdateChecker(false);
        }

        private void ThreadedUpdateChecker(bool silent)
        {
            Thread updateThread;
            updateThread = new Thread(() => this.CheckForUpdate(silent));
            updateThread.Start();
        }

        private void CheckForUpdate(bool silent)
        {
            try
            {
                // HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://sourceforge.net/p/read4mecbr/code/ci/master/tree/current_version.txt?format=raw");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://antday.com/r4m/r4m_updater.php");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader input = new StreamReader(response.GetResponseStream());
                string CurrentVersion = input.ReadLine();
                input.Close();

                if (CurrentVersion == "0")
                {
                    if (!silent)
                    {
                        UpdateError();
                    }
                }

                if (LocalVersion != CurrentVersion)
                {
                    UpdateFound();                
                }
                else
                {
                    if (!silent)
                    {
                        UpdateNotFound();
                    }
                }
            }
            catch
            {
                if (!silent)
                {
                    UpdateError();
                }
            }
        }

        private void UpdateNotFound()
        {
            SetBalloonTip("Update", "No new version available.", ToolTipIcon.Info, "info");
        }

        private void UpdateFound()
        {
            SetBalloonTip("Update available", "A new version is available, click here to download it.", ToolTipIcon.Info, "update");
        }

        private void UpdateError()
        {
            SetBalloonTip("Error", "Couldn't connect to update server.", ToolTipIcon.Error, "error");
        }
    }
}
