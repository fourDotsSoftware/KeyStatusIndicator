namespace KeyStatusIndicator
{
    partial class frmBalloonTip
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
            this.components = new System.ComponentModel.Container();
            this.timBalloon = new System.Windows.Forms.Timer(this.components);
            this.pic = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).BeginInit();
            this.SuspendLayout();
            // 
            // timBalloon
            // 
            this.timBalloon.Interval = 300;
            this.timBalloon.Tick += new System.EventHandler(this.timBalloon_Tick);
            // 
            // pic
            // 
            this.pic.Location = new System.Drawing.Point(69, 42);
            this.pic.Name = "pic";
            this.pic.Size = new System.Drawing.Size(132, 123);
            this.pic.TabIndex = 0;
            this.pic.TabStop = false;
            // 
            // frmBalloonTip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.pic);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBalloonTip";
            this.ShowInTaskbar = false;
            this.Text = "frmBalloonTip";
            ((System.ComponentModel.ISupportInitialize)(this.pic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timBalloon;
        private System.Windows.Forms.PictureBox pic;
    }
}