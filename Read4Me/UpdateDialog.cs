using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Read4Me
{
    public partial class UpdateDialog : Form
    {

        public UpdateDialog()
        {
            InitializeComponent();
            lLinkDownload.Links.Add(0, lLinkDownload.Text.Length, lLinkDownload.Text);
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lLinkDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }
    }
}
