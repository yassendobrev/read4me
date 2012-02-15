using System;
using System.Windows.Forms;

namespace Read4Me
{
    partial class Read4MeForm
    {

        // http://social.msdn.microsoft.com/Forums/en/csharpgeneral/thread/e85c9461-394f-4391-903e-8ea1bd243075
        // shutting down does not work in XP without this code
        private const int WM_QUERYENDSESSION = 0x0011;
        protected override void WndProc( ref Message m )
        {
            if (m.Msg == WM_QUERYENDSESSION)
            {
                // You're shutting down
                mAllowClose = true;
            }
            base.WndProc(ref m);
        }

        // Minimize app to tray
        // http://stackoverflow.com/questions/1730731/how-to-start-winform-app-minimized-to-tray
        protected override void SetVisibleCore(bool value)
        {
            if (!mAllowVisible) value = false;
            base.SetVisibleCore(value);
        }

        // Close to tray
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!mAllowClose)
            {
                HideForm();
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }

        // Show form from tray strip menu
        private void showStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        // Close form from tray strip menu
        private void exitStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        // Show/hide form when icon in tray clicked
        private void mynotifyicon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ToggleForm();
            }
        }

        // Hide form when ESC key pressed
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (msg.WParam.ToInt32() == (int)Keys.Escape)
            {
                HideForm();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // Show form
        private void ShowForm()
        {
            mAllowVisible = true;
            Show();
            this.Activate();
        }

        // Hide form to tray
        private void HideForm()
        {
            mAllowVisible = false;
            this.Hide();
        }

        // Toggle form visibility
        private void ToggleForm()
        {
            if (mAllowVisible == true)
            {
                HideForm();
            }
            else
            {
                ShowForm();
            }
        }

        // Close form
        void CloseForm()
        {
            mAllowClose = true;
            Close();
        }

        private void miExit_Click(object sender, System.EventArgs e)
        {
            CloseForm();
        }
    }
}