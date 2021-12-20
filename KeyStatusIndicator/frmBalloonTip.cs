using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace KeyStatusIndicator
{
    public partial class frmBalloonTip : Form
    {
        public enum LocationEnum:int
        {
            locTopLeft=5,
            locTopRight=2,
            locTopCenter=8,
            locBottomLeft=3,
            locBottomRight=0,
            locBottomCenter=6,
            locMiddleLeft=4,
            locMiddleRight=1,
            locMiddleCenter=7
        }

        /*
         cmbScreenMessage.Items.Add(TranslateHelper.Translate("Bottom Right"));
            cmbScreenMessage.Items.Add(TranslateHelper.Translate("Middle Right"));
            cmbScreenMessage.Items.Add(TranslateHelper.Translate("Top Right"));

            cmbScreenMessage.Items.Add(TranslateHelper.Translate("Bottom Left"));
            cmbScreenMessage.Items.Add(TranslateHelper.Translate("Middle Left"));
            cmbScreenMessage.Items.Add(TranslateHelper.Translate("Top Left"));

            cmbScreenMessage.Items.Add(TranslateHelper.Translate("Bottom Middle"));
            cmbScreenMessage.Items.Add(TranslateHelper.Translate("Middle Middle"));
            cmbScreenMessage.Items.Add(TranslateHelper.Translate("Top Middle"));
         */
        private LocationEnum locEnum = LocationEnum.locBottomRight;
        private string Caption = "";
        private int OffsetX = 0;
        private int OffsetY = 0;
        private int FontSize = 14;

        private bool KeyOn = false;
        private bool FadeIn = false;

        private System.Drawing.Color OnColor = System.Drawing.Color.Empty;
        private System.Drawing.Color OffColor = System.Drawing.Color.Empty;
        private int Count = 1;        

        public frmBalloonTip(LocationEnum _locEnum,string caption,int fontSize,int offsetX,int offsetY,
            System.Drawing.Color onColor,System.Drawing.Color offColor)
        {
            InitializeComponent();                        

            locEnum = _locEnum;

            Caption = caption;

            FontSize = fontSize;

            OffsetX = offsetX;

            OffsetY = offsetY;

            OnColor = onColor;

            OffColor = offColor;

            this.TransparencyKey = Color.Black;
            this.BackColor = Color.Black;
        }

        public void ShowBalloon(bool keyon)
        {
            KeyOn = keyon;

            this.Visible = false;

            using (Graphics g = Graphics.FromHwnd(this.Handle))
            {
                g.Clear(this.BackColor);
            }

            if (pic.Image != null)
            {
                try
                {
                    pic.Image.Dispose();
                    pic.Image = null;
                }
                catch { }
            }

            this.Invalidate();
            

            //this.Opacity = 0;

            using (Graphics g = Graphics.FromHwnd(this.Handle))
            {
                float emSize = g.DpiY * FontSize / 72;

                Font fo = new Font(this.Font.FontFamily, emSize, FontStyle.Bold);
                SizeF sz = g.MeasureString(Caption, fo);
                this.Width = (int)sz.Width;
                this.Height = (int)sz.Height;
                fo.Dispose();
            }            
            
            Bitmap bmp = new Bitmap(this.Width, this.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                float emSize = g.DpiY * FontSize / 72;

                Font fo = new Font(this.Font.FontFamily, emSize, FontStyle.Bold);

                System.Drawing.Color col = (KeyOn ? OnColor : OffColor);

                System.Drawing.Color drawColor = col;

                /*
                System.Drawing.Color drawColor=System.Drawing.Color.Empty;

                if (FadeIn)
                {
                    drawColor=System.Drawing.Color.FromArgb((5-Count)*(255/5),col);

                }
                else
                {
                    drawColor=System.Drawing.Color.FromArgb(255-(5-Count)*(255/5),col);
                }
                */
                SolidBrush sb = new SolidBrush(drawColor);

                //Rectangle bounds = Screen.GetBounds(Point.Empty);                

                //g.DrawString(Caption, fo, sb,new PointF(0,0),StringFormat.GenericTypographic);

                // ========================

                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                SolidBrush brushWhite = new SolidBrush(Color.White);

                /*
                g.FillRectangle(brushWhite, 0, 0,
                    bmp.Width, bmp.Height);
                */

                //this.ClientSize.Width, this.ClientSize.Height);

                FontFamily fontFamily = new FontFamily("Arial");
                StringFormat strformat = new StringFormat();
                string szbuf = Caption;

                GraphicsPath path = new GraphicsPath();
                path.AddString(szbuf, fontFamily,
                    (int)FontStyle.Bold, emSize, new Point(10, 10), strformat);

                //Pen pen = new Pen(Color.FromArgb(0, 0, 160), 5);

                Pen pen = new Pen(drawColor, 5);

                pen.LineJoin = LineJoin.Round;
                g.DrawPath(pen, path);

                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(10, 10, 30, 70),
                    Color.FromArgb(132, 200, 251),
                    //Color.FromArgb(0, 0, 160), LinearGradientMode.Vertical);
                    drawColor, LinearGradientMode.Vertical);

                g.FillPath(brush, path);

                brushWhite.Dispose();
                fontFamily.Dispose();
                path.Dispose();
                pen.Dispose();
                brush.Dispose();
                g.Dispose();

                // ========================


                sb.Dispose();
                fo.Dispose();

            }

            pic.Left = 0;
            pic.Top = 0;
            pic.Width = bmp.Width;
            pic.Height = bmp.Height;

            if (pic.Image != null)
            {
                try
                {
                    pic.Image.Dispose();
                    pic.Image = null;
                }
                catch { }
            }
            pic.Image = bmp;

            pic.Visible = true;

            timBalloon.Enabled = false;

            Count = 5;
            FadeIn = true;
            
            timBalloon.Enabled = true;

            this.TopMost = true;
            this.Show();

            timBalloon_Tick(null, null);
        }

        private void timBalloon_Tick(object sender, EventArgs e)
        {            
            Rectangle bounds = Screen.GetWorkingArea(frmMain.Instance);

            int x = 0;
            int y = 0;

            int offset = 0;

            if (locEnum == LocationEnum.locBottomCenter)
            {
                x = bounds.Width / 2 - this.Width / 2 + OffsetX;
                y = bounds.Bottom - this.Height - OffsetY;
            }
            else if (locEnum == LocationEnum.locBottomLeft)
            {
                x = bounds.Left + OffsetX;
                y = bounds.Bottom - this.Height - OffsetY;
            }
            else if (locEnum == LocationEnum.locBottomRight)
            {
                x = bounds.Right - this.Width - OffsetX;
                y = bounds.Bottom - this.Height - OffsetY;
            }
            else if (locEnum == LocationEnum.locMiddleCenter)
            {
                x = bounds.Width / 2 - this.Width / 2 + OffsetX;
                y = bounds.Height / 2 - this.Height / 2 + OffsetY;
            }
            else if (locEnum == LocationEnum.locMiddleLeft)
            {
                x = bounds.Left + OffsetX;
                y = bounds.Height / 2 - this.Height / 2 + OffsetY;
            }
            else if (locEnum == LocationEnum.locMiddleRight)
            {
                x = bounds.Right - this.Width - OffsetX;
                y = bounds.Height / 2 - this.Height / 2 + OffsetY;
            }
            else if (locEnum == LocationEnum.locTopCenter)
            {
                x = bounds.Width / 2 - this.Width / 2 + OffsetX;
                y = bounds.Top + OffsetY;
            }
            else if (locEnum == LocationEnum.locTopLeft)
            {
                x = bounds.Left + OffsetX;
                y = bounds.Top + OffsetY;
            }
            else if (locEnum == LocationEnum.locTopRight)
            {
                x = bounds.Right - this.Width - offset;
                y = bounds.Top + OffsetY;
            }            

            this.Left = x;
            this.Top = y;

            /*1
            this.Show();
            this.Visible = true;
            this.BringToFront();
            this.TopMost = true;
            */

            /*
            if (FadeIn)
            {
                this.Opacity = Count * 255 / 5;
            }
            else
            {
                this.Opacity = 255 - Count * 255 / 5;
            }
            */

            Count--;

            if (Count < 0)
            {       
                if (FadeIn)
                {
                    FadeIn=false;
                }
                else
                {
                    FadeIn=true;
                    timBalloon.Enabled=false;

                    pic.Visible = false;

                    this.Hide();
                }

                Count=5;
            }
        }

        private void DrawGradient()
        {
            
        }        
    }
}
