using System;
using System.Windows.Forms;

namespace Read4Me
{
    partial class Read4MeForm
    {
        // Minimize app to tray
        // http://stackoverflow.com/questions/1730731/how-to-start-winform-app-minimized-to-tray
        protected override void SetVisibleCore(bool value)
        {
           if (MinToTray)
           {
               if (!mAllowVisible) value = false;
           }
           base.SetVisibleCore(value);
       }

        // Show form from tray strip menu
        private void showStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        // Close form from tray strip menu
        private void exitStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
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
            if (MinToTray)
            {
                mAllowVisible = true;
                Show();
                this.Activate();
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        // Hide form to tray
        private void HideForm()
        {
            if (MinToTray)
            {
                mAllowVisible = false;
                this.Hide();
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        // Toggle form visibility
        private void ToggleForm()
        {
            if (MinToTray)
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
            else
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Normal;
                }
                else
                {
                    this.WindowState = FormWindowState.Minimized;
                }
            }
        }

        private void miExit_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void Form1_SizeChanged(object sender, System.EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized && MinToTray)
            {
                HideForm();
                this.WindowState = FormWindowState.Normal;
            }
        }
    }
}