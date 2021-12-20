namespace KeyPressed
{
    partial class frmKeyPressed
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
            this.timKey = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timKey
            // 
            this.timKey.Enabled = true;
            this.timKey.Interval = 300;
            this.timKey.Tick += new System.EventHandler(this.timKey_Tick);
            // 
            // frmKeyPressed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 395);
            this.Name = "frmKeyPressed";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Activated += new System.EventHandler(this.frmKeyPressed_Activated);
            this.Shown += new System.EventHandler(this.frmKeyPressed_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timKey;
    }
}

