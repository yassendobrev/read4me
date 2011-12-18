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
    public partial class WhatsNewDialog : Form
    {

        public WhatsNewDialog(string local_version)
        {
            InitializeComponent();
            lWhatsNew.Text += local_version;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
