namespace PS4Extreme
{
    partial class ps4ipform
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ps4ipform));
            this.setip = new DevExpress.XtraEditors.SimpleButton();
            this.ps4iptextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // setip
            // 
            this.setip.Location = new System.Drawing.Point(161, 10);
            this.setip.Name = "setip";
            this.setip.Size = new System.Drawing.Size(75, 23);
            this.setip.TabIndex = 0;
            this.setip.Text = "Change";
            this.setip.Click += new System.EventHandler(this.setip_Click);
            // 
            // ps4iptextBox
            // 
            this.ps4iptextBox.Location = new System.Drawing.Point(27, 12);
            this.ps4iptextBox.Name = "ps4iptextBox";
            this.ps4iptextBox.Size = new System.Drawing.Size(128, 21);
            this.ps4iptextBox.TabIndex = 1;
            // 
            // ps4ipform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 46);
            this.Controls.Add(this.ps4iptextBox);
            this.Controls.Add(this.setip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ps4ipform";
            this.Text = "PS4Extreme {-} IP";
            this.Load += new System.EventHandler(this.ps4ipform_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton setip;
        private System.Windows.Forms.TextBox ps4iptextBox;
    }
}