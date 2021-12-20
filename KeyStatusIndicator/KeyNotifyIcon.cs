using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace KeyStatusIndicator
{
    public class KeyNotifyIcon
    {
        public System.Windows.Forms.NotifyIcon notIcon=null;

        private System.Drawing.Icon icoOn = null;
        private System.Drawing.Icon icoOff = null;

        public KeyNotifyIcon(string caption,string description, System.Drawing.Color onColor,System.Drawing.Color offColor)
        {
            notIcon = new System.Windows.Forms.NotifyIcon();
            notIcon.Text = description;

            Bitmap bmpOn = new Bitmap(32, 32);

            using (Graphics g = Graphics.FromImage(bmpOn))
            {
                try
                {
                    //g.Clear(Color.White);

                    g.Clear(onColor);

                    Font fIcon = new Font(frmMain.Instance.Font.FontFamily, 20, FontStyle.Bold);

                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                    //SolidBrush sb = new SolidBrush(onColor);

                    SolidBrush sb = new SolidBrush(Color.Black);

                    g.DrawString(caption, fIcon, sb, new PointF(0, 0));

                    sb.Dispose();

                    fIcon.Dispose();

                }
                catch { }

            }

            icoOn = Icon.FromHandle(bmpOn.GetHicon());

            //===========

            Bitmap bmpOff = new Bitmap(32, 32);

            using (Graphics g = Graphics.FromImage(bmpOff))
            {
                try
                {
                    //g.Clear(Color.White);

                    g.Clear(offColor);

                    Font fIcon = new Font(frmMain.Instance.Font.FontFamily, 20, FontStyle.Bold);

                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                    //SolidBrush sb = new SolidBrush(offColor);

                    SolidBrush sb = new SolidBrush(Color.Black);

                    g.DrawString(caption, fIcon, sb, new PointF(0, 0));

                    sb.Dispose();

                    fIcon.Dispose();

                }
                catch { }

            }

            icoOff = Icon.FromHandle(bmpOff.GetHicon());
        }

        public void ShowOn()
        {
            this.notIcon.Icon = icoOn;
            this.notIcon.Visible = true;
        }

        public void ShowOff()
        {
            this.notIcon.Icon = icoOff;
            this.notIcon.Visible = true;
        }
    }
}
