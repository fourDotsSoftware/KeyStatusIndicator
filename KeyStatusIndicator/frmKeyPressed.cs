using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KeyStatusIndicator
{
    public partial class frmKeyPressed : KeyStatusIndicator.CustomForm
    {
        public static frmKeyPressed Instance = null;

        public frmKeyPressed()
        {
            InitializeComponent();

            Instance = this;
        }

        private void frmKeyPressed_FormClosing(object sender, FormClosingEventArgs e)
        {
            Instance = null;
        }

        private void frmKeyPressed_Activated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
