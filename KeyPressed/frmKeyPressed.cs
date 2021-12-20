using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace KeyPressed
{
    public partial class frmKeyPressed : Form
    {
        private string Caption = null;

        private bool Beep = false;

        private string OnSound = "";

        private string OffSound = "";

        private static Bitmap bmpOn = null;
        private static Icon IconOn = null;

        private static Bitmap bmpOff = null;
        private static Icon IconOff = null;

        //private KeyPressed.MciAudio MCIOn = new KeyPressed.MciAudio();
        //private KeyPressed.MciAudio MCIOff = new KeyPressed.MciAudio();

        private int PreviousValue = -1;

        public frmKeyPressed(string caption,System.Drawing.Color onColor,System.Drawing.Color offColor,bool beep,
            string onsound,string offsound)
        {
            InitializeComponent();

            Caption = caption;

            Beep = beep;

            OnSound = onsound;

            OffSound = offsound;

            bmpOn = new Bitmap(48, 48);            

            using (Graphics g = Graphics.FromImage(bmpOn))
            {
                try
                {
                    //g.Clear(Color.White);

                    g.Clear(onColor);

                    Font fIcon = new Font(Font.FontFamily, 14, FontStyle.Bold);

                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                    //SolidBrush sb = new SolidBrush(onColor);

                    SolidBrush sb = new SolidBrush(Color.Black);

                    g.DrawString(caption, fIcon, sb, new PointF(0, 12));

                    sb.Dispose();

                    fIcon.Dispose();

                }
                catch { }

            }

            IconOn = Icon.FromHandle(bmpOn.GetHicon());

            //===========

            bmpOff = new Bitmap(48, 48);

            using (Graphics g = Graphics.FromImage(bmpOff))
            {
                try
                {
                    //g.Clear(Color.White);

                    g.Clear(offColor);

                    Font fIcon = new Font(Font.FontFamily, 14, FontStyle.Bold);

                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                    //SolidBrush sb = new SolidBrush(offColor);

                    SolidBrush sb = new SolidBrush(Color.Black);

                    g.DrawString(caption, fIcon, sb, new PointF(0, 12));

                    sb.Dispose();

                    fIcon.Dispose();

                }
                catch { }

            }

            IconOff = Icon.FromHandle(bmpOff.GetHicon());

            this.Icon = IconOff;
        }

        private void frmKeyPressed_Activated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }        

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.WindowState != FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        private bool InTimer = false;

        private void timKey_Tick(object sender, EventArgs e)
        {
            if (InTimer) return;

            try
            {
                int x = 0;

                if (Caption == "NUM")
                {
                    //x = GetAsyncKeyState(Keys.NumLock);

                    x = Control.IsKeyLocked(Keys.NumLock) ? 1 : 0;
                }
                else if (Caption == "SCR")
                {
                    //x = GetAsyncKeyState(Keys.Scroll);

                    x = Control.IsKeyLocked(Keys.Scroll) ? 1 : 0;
                }
                else if (Caption == "INS")
                {
                    //x = GetAsyncKeyState(Keys.Insert);

                    x = Control.IsKeyLocked(Keys.Insert) ? 1 : 0;
                }
                else if (Caption == "CAP")
                {
                    //x = GetAsyncKeyState(Keys.CapsLock);

                    x = Control.IsKeyLocked(Keys.CapsLock) ? 1 : 0;
                }

                if ((x == 1) || (x == Int16.MinValue)) //Use constants (0x8000 and -32768 is Int16.MaxValue)
                {
                    if (PreviousValue == -1 || PreviousValue == 0)
                    {
                        this.Icon = IconOn;

                        PreviousValue = 1;

                    }
                }
                else
                {
                    if (PreviousValue == -1 || PreviousValue == 1)
                    {
                        this.Icon = IconOff;

                        PreviousValue = 0;
                    }
                }
            }
            finally
            {
                InTimer = false;
            }
        }

        private void frmKeyPressed_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
