namespace Read4Me
{
    partial class WhatsNewDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WhatsNewDialog));
            this.lPlsDwn = new System.Windows.Forms.Label();
            this.bOK = new System.Windows.Forms.Button();
            this.lWhatsNew = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lPlsDwn
            // 
            this.lPlsDwn.AutoSize = true;
            this.lPlsDwn.Location = new System.Drawing.Point(15, 26);
            this.lPlsDwn.Name = "lPlsDwn";
            this.lPlsDwn.Size = new System.Drawing.Size(184, 13);
            this.lPlsDwn.TabIndex = 56;
            this.lPlsDwn.Text = "- Cleanup unnecessary combo boxes.";
            // 
            // bOK
            // 
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bOK.Location = new System.Drawing.Point(18, 117);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 1;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // lWhatsNew
            // 
            this.lWhatsNew.AutoSize = true;
            this.lWhatsNew.Location = new System.Drawing.Point(15, 13);
            this.lWhatsNew.Name = "lWhatsNew";
            this.lWhatsNew.Size = new System.Drawing.Size(114, 13);
            this.lWhatsNew.TabIndex = 57;
            this.lWhatsNew.Text = "What\'s new in version ";
            // 
            // WhatsNewDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bOK;
            this.ClientSize = new System.Drawing.Size(393, 152);
            this.Controls.Add(this.lWhatsNew);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.lPlsDwn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WhatsNewDialog";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "What\'s new";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lPlsDwn;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Label lWhatsNew;
    }
}