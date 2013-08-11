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
            switch (CheckForUpdate())
            {
                case 0:
                    SetBalloonTip("Update", "No new version available.", ToolTipIcon.Info);
                    break;
                case 1:
                    UpdateDialog dialog = new UpdateDialog();
                    dialog.ShowDialog(this);
                    dialog.Dispose();
                    break;
                case 2:
                    SetBalloonTip("Error", "Couldn't connect to server.", ToolTipIcon.Error);
                    break;
            }
        }

        private uint CheckForUpdate()
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
                    return 2;
                }

                if (LocalVersion != CurrentVersion)
                {
                    return 1;                    
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 2;
            }
        }
    }
}
