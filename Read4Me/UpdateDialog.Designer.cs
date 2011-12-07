namespace Read4Me
{
    partial class UpdateDialog
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
            this.lLinkDownload = new System.Windows.Forms.LinkLabel();
            this.lPlsDwn = new System.Windows.Forms.Label();
            this.bOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lLinkDownload
            // 
            this.lLinkDownload.AutoSize = true;
            this.lLinkDownload.Location = new System.Drawing.Point(12, 40);
            this.lLinkDownload.Name = "lLinkDownload";
            this.lLinkDownload.Size = new System.Drawing.Size(381, 13);
            this.lLinkDownload.TabIndex = 55;
            this.lLinkDownload.TabStop = true;
            this.lLinkDownload.Text = "http://sourceforge.net/projects/read4mecbr/files/latest/download?source=files";
            this.lLinkDownload.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lLinkDownload_LinkClicked);
            // 
            // lPlsDwn
            // 
            this.lPlsDwn.AutoSize = true;
            this.lPlsDwn.Location = new System.Drawing.Point(12, 27);
            this.lPlsDwn.Name = "lPlsDwn";
            this.lPlsDwn.Size = new System.Drawing.Size(241, 13);
            this.lPlsDwn.TabIndex = 56;
            this.lPlsDwn.Text = "New version available. You can download it here:";
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(162, 66);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 1;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // UpdateDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 101);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.lPlsDwn);
            this.Controls.Add(this.lLinkDownload);
            this.Name = "UpdateDialog";
            this.Text = "Update available";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lLinkDownload;
        private System.Windows.Forms.Label lPlsDwn;
        private System.Windows.Forms.Button bOK;
    }
}