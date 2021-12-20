using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;
        
namespace KeyStatusIndicator
{
    public partial class frmMain : KeyStatusIndicator.CustomForm
    {
        public static frmMain Instance = null;

        private static frmKeyPressed frmCapsLock = null;
        private static frmKeyPressed frmNumLock = null;
        private static frmKeyPressed frmScrollLock = null;
        private static frmKeyPressed frmInsert = null;

        private static Bitmap bmpCapsLockOn = null;
        private static Bitmap bmpNumLockOn = null;
        private static Bitmap bmpScrollLockOn = null;
        private static Bitmap bmpInsertOn = null;

        private static Bitmap bmpCapsLockOff = null;
        private static Bitmap bmpNumLockOff = null;
        private static Bitmap bmpScrollLockOff = null;
        private static Bitmap bmpInsertOff = null;        

        private static Icon IconCapsLockOn = null;
        private static Icon IconNumLockOn = null;
        private static Icon IconScrollLockOn = null;
        private static Icon IconInsertOn = null;

        private static Icon IconCapsLockOff = null;
        private static Icon IconNumLockOff = null;
        private static Icon IconScrollLockOff = null;
        private static Icon IconInsertOff = null;

        private frmBalloonTip frmBalloonNum = null;
        private frmBalloonTip frmBalloonCaps = null;
        private frmBalloonTip frmBalloonScroll = null;
        private frmBalloonTip frmBalloonInsert = null;

        private List<frmBalloonTip> lstfrmBaloonTip = new List<frmBalloonTip>();

        private List<KeyNotifyIcon> lstKeyNotifyIcon = new List<KeyNotifyIcon>();

        private List<MciAudio> lstMCIOn = new List<MciAudio>();
        private List<MciAudio> lstMCIOff = new List<MciAudio>();

        private bool[] IndicateKey = new bool[4];
        private bool[] BeepKey = new bool[4];

        [DllImport("kernel32.dll")]
        public static extern bool Beep(int freq, int duration);

        public static void TestBeeps()
        {
            Beep(1000, 1600); //low frequency, longer sound
            Beep(2000, 400); //high frequency, short sound
        }

        public frmMain()
        {
            InitializeComponent();

            Instance = this;            

            dt.Columns.Add("filename");
            dt.Columns.Add("slideranges");
            dt.Columns.Add("sizekb");
            dt.Columns.Add("fullfilepath");
            dt.Columns.Add("filedate");
            dt.Columns.Add("rootfolder");

            for (int k = 0; k <= 3; k++)
            {
                PreviousValues[k] = -1;
                IndicateKey[k] = false;
                BeepKey[k] = false;
            }
            
            if (Module.HideMode)
            {
                this.Visible = false;
                this.WindowState = FormWindowState.Minimized;
            }             
        }

        public DataTable dt = new DataTable("table");                
        
        /*
        #region Share

        private void tsiFacebook_Click(object sender, EventArgs e)
        {
            ShareHelper.ShareFacebook();
        }

        private void tsiTwitter_Click(object sender, EventArgs e)
        {
            ShareHelper.ShareTwitter();
        }

        private void tsiGooglePlus_Click(object sender, EventArgs e)
        {
            ShareHelper.ShareGooglePlus();
        }

        private void tsiLinkedIn_Click(object sender, EventArgs e)
        {
            ShareHelper.ShareLinkedIn();
        }

        private void tsiEmail_Click(object sender, EventArgs e)
        {
            ShareHelper.ShareEmail();
        }

        #endregion
        */

        private void LoadSettings()
        {
            btnOnColor.BackColor = Properties.Settings.Default.OnColor;

            btnOffColor.BackColor = Properties.Settings.Default.OffColor;

            cmbOnSound.Text = Properties.Settings.Default.OnFilepath;

            cmbOffSound.Text = Properties.Settings.Default.OffFilepath;

            nudFontSize.Value = Properties.Settings.Default.ScreenMsgFontSize;

            chkBeep.Checked = Properties.Settings.Default.Beep;

            chkCapsLock.Checked = Properties.Settings.Default.CapsLock;

            chkNumLock.Checked = Properties.Settings.Default.NumLock;

            chkScrollLock.Checked = Properties.Settings.Default.ScrollLock;

            chkInsert.Checked = Properties.Settings.Default.Insert;

            nudOffsetX.Value = Properties.Settings.Default.OffsetX;

            nudOffsetY.Value = Properties.Settings.Default.OffsetY;

            nudFontSize.Value = Properties.Settings.Default.ScreenMsgFontSize;

            cmbScreenMessage.SelectedIndex = Properties.Settings.Default.ScreenMsgLocation;

            chkIcon.Checked = Properties.Settings.Default.Icon;

            chkSystemTray.Checked = Properties.Settings.Default.SystemTrayIcon;

            chkScreenMessage.Checked = Properties.Settings.Default.ScreenMessage;

            cmbCapsOffS.Text = Properties.Settings.Default.CapsLockOffSound;
            cmbCapsOnS.Text = Properties.Settings.Default.CapsLockOnSound;

            cmbInsOffS.Text = Properties.Settings.Default.InsertOffSound;
            cmbInsOn.Text = Properties.Settings.Default.InsertOnSound;

            cmbNumOffS.Text = Properties.Settings.Default.NumLockOffSound;
            cmbNumOnS.Text = Properties.Settings.Default.NumLockOnSound;

            cmbScrollOffS.Text = Properties.Settings.Default.ScrollLockOffSound;
            cmbScrollOnS.Text = Properties.Settings.Default.ScrollLockOnSound;

            chkNumSpecific.Checked = Properties.Settings.Default.SpecialNum;
            chkCapsSpecific.Checked = Properties.Settings.Default.SpecialCaps;
            chkScrollSpecific.Checked = Properties.Settings.Default.SpecialScroll;
            chkInsSpecific.Checked = Properties.Settings.Default.SpecialInsert;

            btnNumColOff.BackColor = Properties.Settings.Default.NumLockOffColor;
            btnNumColOn.BackColor = Properties.Settings.Default.NumLockOnColor;

            btnCapsColOff.BackColor = Properties.Settings.Default.CapsLockOffColor;
            btnCapsColOn.BackColor = Properties.Settings.Default.CapsLockOnColor;

            btnScrollColOff.BackColor = Properties.Settings.Default.ScrollLockOffColor;
            btnScrollColOn.BackColor = Properties.Settings.Default.ScrollLockOnColor;

            btnInsColOff.BackColor = Properties.Settings.Default.InsertOffColor;
            btnInsColOn.BackColor = Properties.Settings.Default.InsertOnColor;

            chkNumBeep.Checked = Properties.Settings.Default.BeepNum;

            chkCapsBeep.Checked = Properties.Settings.Default.BeepCaps;

            chkScrollBeep.Checked = Properties.Settings.Default.BeepScroll;

            chkInsBeep.Checked = Properties.Settings.Default.BeepInsert;

            chkOnSound.Checked = Properties.Settings.Default.OnSound;

            chkOffSound.Checked = Properties.Settings.Default.OffSound;

            chkNumSpecific.Checked = Properties.Settings.Default.SpecialNum;

            chkCapsSpecific.Checked = Properties.Settings.Default.SpecialCaps;

            chkScrollSpecific.Checked = Properties.Settings.Default.SpecialScroll;

            chkInsSpecific.Checked = Properties.Settings.Default.SpecialInsert;

            minimizeToSystemTrayToolStripMenuItem.Checked = Properties.Settings.Default.MinimizeSystemTray;

            showApplicationTrayIconToolStripMenuItem.Checked = Properties.Settings.Default.ShowTrayIcon;

            notMain.Visible = Properties.Settings.Default.ShowTrayIcon;

            notMain.Text = Module.ApplicationName;

            nudOffsetX.Value = Properties.Settings.Default.OffsetX;

            nudOffsetY.Value = Properties.Settings.Default.OffsetY;

            chkCapsOffS.Checked = Properties.Settings.Default.chkCapsOffS;

            chkCapsOnS.Checked = Properties.Settings.Default.chkCapsOnS;

            chkInsOffS.Checked = Properties.Settings.Default.chkInsOffS;

            chkInsOns.Checked = Properties.Settings.Default.chkInsOnS;

            chkNumOnS.Checked = Properties.Settings.Default.chkNumOnS;

            chkNumOffS.Checked = Properties.Settings.Default.chkNumOffS;

            chkScrollOffS.Checked = Properties.Settings.Default.chkScrollOffS;

            chkScrollOnS.Checked = Properties.Settings.Default.chkScrollOnS;

            chkMinimizeOnOK.Checked = Properties.Settings.Default.MinimizeOnOK;
        }

        private void SetupOnLoad()
        {            
            //3this.Icon = Properties.Resources.pdf_compress_48;

            this.Text = Module.ApplicationTitle;
            //this.Width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            //this.Left = 0;
            AddLanguageMenuItems();

            //3DownloadSuggestionsHelper ds = new DownloadSuggestionsHelper();
            //3ds.SetupDownloadMenuItems(downloadToolStripMenuItem);

            AdjustSizeLocation();

            //3SetupOutputFolders();

            //3keepFolderStructureToolStripMenuItem.Checked = Properties.Settings.Default.KeepFolderStructure;           

            /*
            //3
            buyToolStripMenuItem.Visible = frmPurchase.RenMove;

            if (Properties.Settings.Default.Price != string.Empty && !buyApplicationToolStripMenuItem.Text.EndsWith(Properties.Settings.Default.Price))
            {
                buyApplicationToolStripMenuItem.Text = buyApplicationToolStripMenuItem.Text + " " + Properties.Settings.Default.Price;
            }
            */

            cmbScreenMessage.Items.Clear();

            cmbScreenMessage.Items.Add(TranslateHelper.Translate("Bottom Right"));
            cmbScreenMessage.Items.Add(TranslateHelper.Translate("Middle Right"));
            cmbScreenMessage.Items.Add(TranslateHelper.Translate("Top Right"));

            cmbScreenMessage.Items.Add(TranslateHelper.Translate("Bottom Left"));
            cmbScreenMessage.Items.Add(TranslateHelper.Translate("Middle Left"));
            cmbScreenMessage.Items.Add(TranslateHelper.Translate("Top Left"));

            cmbScreenMessage.Items.Add(TranslateHelper.Translate("Bottom Center"));
            cmbScreenMessage.Items.Add(TranslateHelper.Translate("Middle Center"));
            cmbScreenMessage.Items.Add(TranslateHelper.Translate("Top Center"));
                        
            cmbOnSound.Items.Clear();

            string[] ons = Properties.Settings.Default.RecentOnFilepath.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);

            cmbOnSound.Items.Add("");

            for (int k = 0; k < ons.Length; k++)
            {
                //cmbOnSound.Items.Add(ons[k]);
            }

            cmbOffSound.Items.Clear();

            string[] offs = Properties.Settings.Default.RecentOffFilepath.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);

            cmbOffSound.Items.Add("");

            for (int k = 0; k < offs.Length; k++)
            {
                //cmbOffSound.Items.Add(offs[k]);
            }            

            List<string> lst = GetWindowsSounds();

            for (int k = 0; k < lst.Count; k++)
            {
                cmbOnSound.Items.Add(lst[k]);
                cmbOffSound.Items.Add(lst[k]);

                cmbNumOffS.Items.Add(lst[k]);
                cmbNumOnS.Items.Add(lst[k]);

                cmbCapsOffS.Items.Add(lst[k]);
                cmbCapsOnS.Items.Add(lst[k]);

                cmbInsOffS.Items.Add(lst[k]);
                cmbInsOn.Items.Add(lst[k]);

                cmbScrollOffS.Items.Add(lst[k]);
                cmbScrollOnS.Items.Add(lst[k]);                
            }            

            RegistryKey key = Registry.CurrentUser;

            try
            {
                key = key.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false);

                if (key != null)
                {
                    if (key.GetValue(Module.ApplicationName) != null)
                    {
                        Properties.Settings.Default.RunStartupCU = true;

                        chkRunCurrentUser.Checked = true;
                    }

                    key.Close();
                }
            }
            catch (Exception ex)
            {
                Module.ShowError("Error could not read Registry !", ex.Message);
            }

            key = Registry.LocalMachine;

            try
            {
                key = key.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false);

                if (key != null)
                {
                    if (key.GetValue(Module.ApplicationName) != null)
                    {
                        Properties.Settings.Default.RunStartupLM = true;

                        chkRunLocalMachine.Checked = true;
                    }

                    key.Close();
                }
            }
            catch (Exception ex)
            {
                Module.ShowError("Error could not read Registry !", ex.Message);
            }

            LoadSettings();

            checkForNewVersionEachWeekToolStripMenuItem.Checked = Properties.Settings.Default.CheckWeek;
        }

        private void AdjustSizeLocation()
        {
            //3
            /*
            if (Properties.Settings.Default.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {

                if (Properties.Settings.Default.Width == -1)
                {
                    this.CenterToScreen();
                    return;
                }
                else
                {
                    this.Width = Properties.Settings.Default.Width;
                }
                if (Properties.Settings.Default.Height != -1)
                {
                    this.Height = Properties.Settings.Default.Height;
                }

                if (Properties.Settings.Default.Left != -1)
                {
                    this.Left = Properties.Settings.Default.Left;
                }

                if (Properties.Settings.Default.Top != -1)
                {
                    this.Top = Properties.Settings.Default.Top;
                }

                if (this.Width < 300)
                {
                    this.Width = 300;
                }

                if (this.Height < 300)
                {
                    this.Height = 300;
                }

                if (this.Left < 0)
                {
                    this.Left = 0;
                }

                if (this.Top < 0)
                {
                    this.Top = 0;
                }
            }
            */
        }

        private void SaveSizeLocation()
        {
            //3
            /*
            Properties.Settings.Default.Maximized = (this.WindowState == FormWindowState.Maximized);
            Properties.Settings.Default.Left = this.Left;
            Properties.Settings.Default.Top = this.Top;
            Properties.Settings.Default.Width = this.Width;
            Properties.Settings.Default.Height = this.Height;
            */

            Properties.Settings.Default.Save();

        }

        #region Localization

        private void AddLanguageMenuItems()
        {
            for (int k = 0; k < frmLanguage.LangCodes.Count; k++)
            {
                ToolStripMenuItem ti = new ToolStripMenuItem();
                ti.Text = frmLanguage.LangDesc[k];
                ti.Tag = frmLanguage.LangCodes[k];
                ti.Image = frmLanguage.LangImg[k];

                if (Properties.Settings.Default.Language == frmLanguage.LangCodes[k])
                {
                    ti.Checked = true;
                }

                ti.Click += new EventHandler(tiLang_Click);

                if (k < 25)
                {
                    languages1ToolStripMenuItem.DropDownItems.Add(ti);
                }
                else
                {
                    languages2ToolStripMenuItem.DropDownItems.Add(ti);
                }

                //languageToolStripMenuItem.DropDownItems.Add(ti);
            }
        }

        void tiLang_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ti = (ToolStripMenuItem)sender;
            string langcode = ti.Tag.ToString();
            ChangeLanguage(langcode);

            //for (int k = 0; k < languageToolStripMenuItem.DropDownItems.Count; k++)
            for (int k = 0; k < languages1ToolStripMenuItem.DropDownItems.Count; k++)
            {
                ToolStripMenuItem til = (ToolStripMenuItem)languages1ToolStripMenuItem.DropDownItems[k];
                if (til == ti)
                {
                    til.Checked = true;
                }
                else
                {
                    til.Checked = false;
                }
            }

            for (int k = 0; k < languages2ToolStripMenuItem.DropDownItems.Count; k++)
            {
                ToolStripMenuItem til = (ToolStripMenuItem)languages2ToolStripMenuItem.DropDownItems[k];
                if (til == ti)
                {
                    til.Checked = true;
                }
                else
                {
                    til.Checked = false;
                }
            }
        }

        private bool InChangeLanguage = false;

        private void ChangeLanguage(string language_code)
        {
            try
            {
                InChangeLanguage = true;

                Properties.Settings.Default.Language = language_code;
                frmLanguage.SetLanguage();

                Properties.Settings.Default.Save();
                Module.ShowMessage("Please restart the application !");
                Application.Exit();
                return;

                bool maximized = (this.WindowState == FormWindowState.Maximized);
                this.WindowState = FormWindowState.Normal;

                /*
                RegistryKey key = Registry.CurrentUser;
                RegistryKey key2 = Registry.CurrentUser;

                try
                {
                    key = key.OpenSubKey("Software\\4dots Software", true);

                    if (key == null)
                    {
                        key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\4dots Software");
                    }

                    key2 = key.OpenSubKey(frmLanguage.RegKeyName, true);

                    if (key2 == null)
                    {
                        key2 = key.CreateSubKey(frmLanguage.RegKeyName);
                    }

                    key = key2;

                    //key.SetValue("Language", language_code);
                    key.SetValue("Menu Item Caption", TranslateHelper.Translate("Change PDF Properties"));
                }
                catch (Exception ex)
                {
                    Module.ShowError(ex);
                    return;
                }
                finally
                {
                    key.Close();
                    key2.Close();
                }
                */
                //1SaveSizeLocation();

                //3SavePositionSize();

                this.Controls.Clear();

                InitializeComponent();

                SetupOnLoad();

                if (maximized)
                {
                    this.WindowState = FormWindowState.Maximized;
                }

                this.ResumeLayout(true);
            }
            finally
            {
                InChangeLanguage = false;
            }
        }

        #endregion

        private void buyApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Module.BuyURL);
        }

        private void enterLicenseKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private List<string> GetWindowsSounds()
        {            
            string syspath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(
                Environment.GetFolderPath(Environment.SpecialFolder.System)), "media");

            string[] filez = System.IO.Directory.GetFiles(syspath, "*.wav", SearchOption.TopDirectoryOnly);

            return new List<string>(filez);
        }

        private bool LoadedOnce = false;

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (LoadedOnce) return;

            LoadedOnce = true;

            //Module.ShowMessage(Color.LightSteelBlue.R.ToString() + " " + Color.LightSteelBlue.G.ToString() + " " + Color.LightSteelBlue.B.ToString());
            /*
            timer1.Enabled = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            */

            SetupOnLoad();

            if (!Module.HideMode && Properties.Settings.Default.CheckWeek)
            {
                UpdateHelper.InitializeCheckVersionWeek();
            }
            else
            {
                LoadComplete = false;

                btnOK_Click(null, null);

                LoadComplete = true;
            }
            
            /*
            for (int k = 1; k <= 5; k++)
            {
                //Console.Write("\a");
                Console.Beep();
                //Beep(800, 300);

                //Beep(800, 1000);
            }*/

            ResizeControls();
        }

        #region Help

        private void helpGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start(Application.StartupPath + "\\Video Cutter Joiner Expert - User's Manual.chm");
            System.Diagnostics.Process.Start(Module.HelpURL);
        }

        private void pleaseDonateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/donate.php");
        }

        private void dotsSoftwarePRODUCTCATALOGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com/downloads/4dots-Software-PRODUCT-CATALOG.pdf");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout f = new frmAbout();
            f.ShowDialog();
        }

        private void tiHelpFeedback_Click(object sender, EventArgs e)
        {
            /*
            frmUninstallQuestionnaire f = new frmUninstallQuestionnaire(false);
            f.ShowDialog();
            */

            System.Diagnostics.Process.Start("https://www.4dots-software.com/support/bugfeature.php?app=" + System.Web.HttpUtility.UrlEncode(Module.ShortApplicationTitle));
        }

        private void followUsOnTwitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.twitter.com/4dotsSoftware");
        }

        private void visit4dotsSoftwareWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.4dots-software.com");
        }

        private void checkForNewVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateHelper.CheckVersion(false);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch { }
        }

        #endregion

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();

            Process[] procs = System.Diagnostics.Process.GetProcessesByName("KeyPressedCaps");

            for (int k = 0; k < procs.Length; k++)
            {
                try
                {
                    procs[k].Kill();
                }
                catch { }
            }

            procs = System.Diagnostics.Process.GetProcessesByName("KeyPressedNum");

            for (int k = 0; k < procs.Length; k++)
            {
                try
                {
                    procs[k].Kill();
                }
                catch { }
            }

            procs = System.Diagnostics.Process.GetProcessesByName("KeyPressedScroll");

            for (int k = 0; k < procs.Length; k++)
            {
                try
                {
                    procs[k].Kill();
                }
                catch { }
            }

            procs = System.Diagnostics.Process.GetProcessesByName("KeyPressedIns");

            for (int k = 0; k < procs.Length; k++)
            {
                try
                {
                    procs[k].Kill();
                }
                catch { }
            }


            
        }

        private void SaveSettings()
        {
            string ons = "";

            for (int k = 1; k < cmbOnSound.Items.Count && k <= 13; k++)
            {
                if (cmbOnSound.Items[k].ToString() != string.Empty)
                {
                    ons += cmbOnSound.Items[k].ToString() + "|||";
                }
            }

            Properties.Settings.Default.RecentOnFilepath = ons;

            Properties.Settings.Default.OnFilepath = cmbOnSound.Text;

            string offs = "";

            for (int k = 1; k < cmbOffSound.Items.Count && k <= 13; k++)
            {
                if (cmbOffSound.Items[k].ToString() != string.Empty)
                {
                    offs += cmbOffSound.Items[k].ToString() + "|||";
                }
            }

            Properties.Settings.Default.RecentOffFilepath = offs;

            Properties.Settings.Default.OffFilepath = cmbOffSound.Text;

            Properties.Settings.Default.ScreenMsgFontSize = (int)nudFontSize.Value;

            Properties.Settings.Default.Beep = chkBeep.Checked;

            Properties.Settings.Default.CapsLock = chkCapsLock.Checked;

            Properties.Settings.Default.NumLock = chkNumLock.Checked;

            Properties.Settings.Default.ScrollLock = chkScrollLock.Checked;

            Properties.Settings.Default.Insert = chkInsert.Checked;

            Properties.Settings.Default.OffsetX = (int)nudOffsetX.Value;

            Properties.Settings.Default.OffsetY = (int)nudOffsetY.Value;

            Properties.Settings.Default.ScreenMsgFontSize = (int)nudFontSize.Value;

            Properties.Settings.Default.ScreenMsgLocation = cmbScreenMessage.SelectedIndex;

            Properties.Settings.Default.Icon = chkIcon.Checked;

            Properties.Settings.Default.SystemTrayIcon = chkSystemTray.Checked;

            Properties.Settings.Default.ScreenMessage = chkScreenMessage.Checked;

            Properties.Settings.Default.OnSound = chkOnSound.Checked;

            Properties.Settings.Default.OffSound = chkOffSound.Checked;

            //====

            Properties.Settings.Default.MinimizeSystemTray = minimizeToSystemTrayToolStripMenuItem.Checked;

            Properties.Settings.Default.ShowTrayIcon = showApplicationTrayIconToolStripMenuItem.Checked;

            Properties.Settings.Default.CapsLockOffSound = cmbCapsOffS.Text;
            Properties.Settings.Default.CapsLockOnSound=cmbCapsOnS.Text;

            Properties.Settings.Default.InsertOffSound=cmbInsOffS.Text;
            Properties.Settings.Default.InsertOnSound=cmbInsOn.Text;

            Properties.Settings.Default.NumLockOffSound=cmbNumOffS.Text;
            Properties.Settings.Default.NumLockOnSound=cmbNumOnS.Text;

            Properties.Settings.Default.ScrollLockOffSound=cmbScrollOffS.Text;
            Properties.Settings.Default.ScrollLockOnSound=cmbScrollOnS.Text;

            Properties.Settings.Default.SpecialNum=chkNumSpecific.Checked;
            Properties.Settings.Default.SpecialCaps=chkCapsSpecific.Checked;
            Properties.Settings.Default.SpecialScroll=chkScrollSpecific.Checked;
            Properties.Settings.Default.SpecialInsert=chkInsSpecific.Checked;

            Properties.Settings.Default.NumLockOffColor=btnNumColOff.BackColor;
            Properties.Settings.Default.NumLockOnColor=btnNumColOn.BackColor;

            Properties.Settings.Default.CapsLockOffColor=btnCapsColOff.BackColor;
            Properties.Settings.Default.CapsLockOnColor=btnCapsColOn.BackColor;

            Properties.Settings.Default.ScrollLockOffColor=btnScrollColOff.BackColor;
            Properties.Settings.Default.ScrollLockOnColor=btnScrollColOn.BackColor;

            Properties.Settings.Default.InsertOffColor=btnInsColOff.BackColor;
            Properties.Settings.Default.InsertOnColor=btnInsColOn.BackColor;

            Properties.Settings.Default.BeepNum=chkNumBeep.Checked;

            Properties.Settings.Default.BeepCaps=chkCapsBeep.Checked;

            Properties.Settings.Default.BeepScroll=chkScrollBeep.Checked;

            Properties.Settings.Default.BeepInsert=chkInsBeep.Checked;

            Properties.Settings.Default.SpecialNum = chkNumSpecific.Checked;

            Properties.Settings.Default.SpecialCaps = chkCapsSpecific.Checked;

            Properties.Settings.Default.SpecialScroll = chkScrollSpecific.Checked;

            Properties.Settings.Default.SpecialInsert = chkInsSpecific.Checked;

            Properties.Settings.Default.CheckWeek = checkForNewVersionEachWeekToolStripMenuItem.Checked;

            Properties.Settings.Default.Save();

            if (Properties.Settings.Default.RunStartupCU != chkRunCurrentUser.Checked)
            {
                if (!Module.RunAdminAction("-runstartupcu \"" + chkRunCurrentUser.Checked.ToString() + "\" \"" + Module.ApplicationName + "\" \"" + Application.ExecutablePath + "\" \"/hide\""))
                {
                    Module.ShowError("Error could not run Action that requires Admin Rights !");
                    return;
                }
                else
                {
                    Properties.Settings.Default.RunStartupCU = chkRunCurrentUser.Checked;
                }
            }

            if (Properties.Settings.Default.RunStartupLM != chkRunLocalMachine.Checked)
            {
                if (!Module.RunAdminAction("-runstartuplm \"" + chkRunLocalMachine.Checked.ToString() + "\" \"" + Module.ApplicationName + "\" \"" + Application.ExecutablePath + "\" \"/hide\""))
                {
                    Module.ShowError("Error could not run Action that requires Admin Rights !");
                    return;
                }
                else
                {
                    Properties.Settings.Default.RunStartupLM = chkRunLocalMachine.Checked;
                }
            }

            Properties.Settings.Default.OffsetX = (int)nudOffsetX.Value;

            Properties.Settings.Default.OffsetY = (int)nudOffsetY.Value;

            Properties.Settings.Default.chkCapsOffS = chkCapsOffS.Checked;

            Properties.Settings.Default.chkCapsOnS = chkCapsOnS.Checked;

            Properties.Settings.Default.chkInsOffS = chkInsOffS.Checked;

            Properties.Settings.Default.chkInsOnS = chkInsOns.Checked;

            Properties.Settings.Default.chkNumOnS = chkNumOnS.Checked;

            Properties.Settings.Default.chkNumOffS = chkNumOffS.Checked;

            Properties.Settings.Default.chkScrollOffS = chkScrollOffS.Checked;

            Properties.Settings.Default.chkScrollOnS = chkScrollOnS.Checked;

            Properties.Settings.Default.MinimizeOnOK = chkMinimizeOnOK.Checked;

            Properties.Settings.Default.Save();
        }

        private void btnOnColor_Click(object sender, EventArgs e)
        {
            ColorDialog cld = new ColorDialog();

            if (sender == btnOnColor)
            {
                cld.Color = Properties.Settings.Default.OnColor;
            }
            else if (sender == btnOffColor)
            {
                cld.Color = Properties.Settings.Default.OffColor;
            }
            else if (sender == btnNumColOff)
            {
                cld.Color = Properties.Settings.Default.NumLockOffColor;
            }
            else if (sender == btnNumColOn)
            {
                cld.Color = Properties.Settings.Default.NumLockOnColor;
            }
            else if (sender == btnCapsColOff)
            {
                cld.Color = Properties.Settings.Default.CapsLockOffColor;
            }
            else if (sender == btnCapsColOn)
            {
                cld.Color = Properties.Settings.Default.CapsLockOnColor;
            }
            else if (sender == btnScrollColOff)
            {
                cld.Color = Properties.Settings.Default.ScrollLockOffColor;
            }
            else if (sender == btnScrollColOn)
            {
                cld.Color = Properties.Settings.Default.ScrollLockOnColor;
            }
            else if (sender == btnInsColOff)
            {
                cld.Color = Properties.Settings.Default.InsertOffColor;
            }
            else if (sender == btnInsColOn)
            {
                cld.Color = Properties.Settings.Default.InsertOnColor;
            }

            cld.FullOpen = true;

            if (cld.ShowDialog() == DialogResult.OK)
            {
                Button btn = (Button)sender;
                btn.BackColor = cld.Color;

                if (sender == btnOnColor)
                {
                    Properties.Settings.Default.OnColor=cld.Color;
                }
                else if (sender == btnOffColor)
                {
                    Properties.Settings.Default.OffColor=cld.Color;
                }
                else if (sender == btnNumColOff)
                {
                    Properties.Settings.Default.NumLockOffColor=cld.Color;
                }
                else if (sender == btnNumColOn)
                {
                    Properties.Settings.Default.NumLockOnColor=cld.Color;
                }
                else if (sender == btnCapsColOff)
                {
                    Properties.Settings.Default.CapsLockOffColor=cld.Color;
                }
                else if (sender == btnCapsColOn)
                {
                    Properties.Settings.Default.CapsLockOnColor=cld.Color;
                }
                else if (sender == btnScrollColOff)
                {
                    Properties.Settings.Default.ScrollLockOffColor=cld.Color;
                }
                else if (sender == btnScrollColOn)
                {
                    Properties.Settings.Default.ScrollLockOnColor=cld.Color;
                }
                else if (sender == btnInsColOff)
                {
                    Properties.Settings.Default.InsertOffColor=cld.Color;
                }
                else if (sender == btnInsColOn)
                {
                    Properties.Settings.Default.InsertOnColor=cld.Color;
                }
            }
        }

        private void btnOffColor_Click(object sender, EventArgs e)
        {
            ColorDialog cld = new ColorDialog();

            if (sender == btnOnColor)
            {
                cld.Color = Properties.Settings.Default.OnColor;
            }
            else if (sender == btnOffColor)
            {
                cld.Color = Properties.Settings.Default.OffColor;
            }
            else if (sender == btnNumColOff)
            {
                cld.Color = Properties.Settings.Default.NumLockOffColor;
            }
            else if (sender == btnNumColOn)
            {
                cld.Color = Properties.Settings.Default.NumLockOnColor;
            }
            else if (sender == btnCapsColOff)
            {
                cld.Color = Properties.Settings.Default.CapsLockOffColor;
            }
            else if (sender == btnCapsColOn)
            {
                cld.Color = Properties.Settings.Default.CapsLockOnColor;
            }
            else if (sender == btnScrollColOff)
            {
                cld.Color = Properties.Settings.Default.ScrollLockOffColor;
            }
            else if (sender == btnScrollColOn)
            {
                cld.Color = Properties.Settings.Default.ScrollLockOnColor;
            }
            else if (sender == btnInsColOff)
            {
                cld.Color = Properties.Settings.Default.InsertOffColor;
            }
            else if (sender == btnInsColOn)
            {
                cld.Color = Properties.Settings.Default.InsertOnColor;
            }

            cld.FullOpen = true;

            if (cld.ShowDialog() == DialogResult.OK)
            {
                Button btn = (Button)sender;
                btn.BackColor = cld.Color;

                if (sender == btnOnColor)
                {
                    Properties.Settings.Default.OnColor = cld.Color;
                }
                else if (sender == btnOffColor)
                {
                    Properties.Settings.Default.OffColor = cld.Color;
                }
                else if (sender == btnNumColOff)
                {
                    Properties.Settings.Default.NumLockOffColor = cld.Color;
                }
                else if (sender == btnNumColOn)
                {
                    Properties.Settings.Default.NumLockOnColor = cld.Color;
                }
                else if (sender == btnCapsColOff)
                {
                    Properties.Settings.Default.CapsLockOffColor = cld.Color;
                }
                else if (sender == btnCapsColOn)
                {
                    Properties.Settings.Default.CapsLockOnColor = cld.Color;
                }
                else if (sender == btnScrollColOff)
                {
                    Properties.Settings.Default.ScrollLockOffColor = cld.Color;
                }
                else if (sender == btnScrollColOn)
                {
                    Properties.Settings.Default.ScrollLockOnColor = cld.Color;
                }
                else if (sender == btnInsColOff)
                {
                    Properties.Settings.Default.InsertOffColor = cld.Color;
                }
                else if (sender == btnInsColOn)
                {
                    Properties.Settings.Default.InsertOnColor = cld.Color;
                }
            }
        }

        private bool NumEnabled = false;
        private bool Init = false;

        private void InitializeIcons()
        {
            for (int k = 0; k <= 3; k++)
            {
                PreviousValues[k] = -1;
                IndicateKey[k] = false;
                BeepKey[k] = false;
            }

            Process[] procs = System.Diagnostics.Process.GetProcessesByName("KeyPressedCaps");

            for (int k = 0; k < procs.Length; k++)
            {
                try
                {
                    procs[k].Kill();
                }
                catch { }
            }

            procs = System.Diagnostics.Process.GetProcessesByName("KeyPressedNum");

            for (int k = 0; k < procs.Length; k++)
            {
                try
                {
                    procs[k].Kill();
                }
                catch { }
            }

            procs = System.Diagnostics.Process.GetProcessesByName("KeyPressedScroll");

            for (int k = 0; k < procs.Length; k++)
            {
                try
                {
                    procs[k].Kill();
                }
                catch { }
            }

            procs = System.Diagnostics.Process.GetProcessesByName("KeyPressedIns");

            for (int k = 0; k < procs.Length; k++)
            {
                try
                {
                    procs[k].Kill();
                }
                catch { }
            }

            string onsound = chkOnSound.Checked ? cmbOnSound.Text : "";

            string offsound = chkOffSound.Checked ? cmbOffSound.Text : "";

            if (onsound == string.Empty) onsound = "None";

            if (offsound == string.Empty) offsound = "None";

            if (chkIcon.Checked)
            {
                Process p = null;

                if (chkCapsLock.Checked)
                {
                    p = new Process();
                    p.StartInfo.FileName = "KeyPressedCaps.exe";
                    p.StartInfo.WorkingDirectory = Application.StartupPath;
                    p.StartInfo.Arguments = "\"CAP\" \"" + ColorToString((chkCapsSpecific.Checked ? Properties.Settings.Default.CapsLockOnColor : Properties.Settings.Default.OnColor)) + "\" \""
                        + ColorToString((chkCapsSpecific.Checked ? Properties.Settings.Default.CapsLockOffColor : Properties.Settings.Default.OffColor)) + "\""
                        + " \"" + ((chkCapsSpecific.Checked ? Properties.Settings.Default.BeepCaps.ToString() : Properties.Settings.Default.Beep.ToString())) + "\""
                        + " \"" + ((chkCapsSpecific.Checked ? Properties.Settings.Default.CapsLockOnSound : onsound)) + "\""
                        + " \"" + ((chkCapsSpecific.Checked ? Properties.Settings.Default.CapsLockOffSound : offsound)) + "\""
                        ;
                    p.Start();
                }

                if (chkInsert.Checked)
                {
                    p = new Process();
                    p.StartInfo.FileName = "KeyPressedIns.exe";
                    p.StartInfo.WorkingDirectory = Application.StartupPath;
                    p.StartInfo.Arguments = "\"INS\" \"" + ColorToString((chkInsSpecific.Checked ? Properties.Settings.Default.InsertOnColor : Properties.Settings.Default.OnColor)) + "\" \""
                        + ColorToString((chkInsSpecific.Checked ? Properties.Settings.Default.InsertOffColor : Properties.Settings.Default.OffColor)) + "\""
                        + " \"" + ((chkInsSpecific.Checked ? Properties.Settings.Default.BeepInsert.ToString() : Properties.Settings.Default.Beep.ToString())) + "\""
                        + " \"" + ((chkInsSpecific.Checked ? Properties.Settings.Default.InsertOnSound : onsound)) + "\""
                        + " \"" + ((chkInsSpecific.Checked ? Properties.Settings.Default.InsertOffSound : offsound)) + "\""
                        ;
                    p.Start();
                }

                if (chkScrollLock.Checked)
                {
                    p = new Process();
                    p.StartInfo.FileName = "KeyPressedScroll.exe";
                    p.StartInfo.WorkingDirectory = Application.StartupPath;
                    p.StartInfo.Arguments = "\"SCR\" \"" + ColorToString((chkScrollSpecific.Checked ? Properties.Settings.Default.ScrollLockOnColor : Properties.Settings.Default.OnColor)) + "\" \""
                        + ColorToString((chkScrollSpecific.Checked ? Properties.Settings.Default.ScrollLockOffColor : Properties.Settings.Default.OffColor)) + "\""
                        + " \"" + ((chkScrollSpecific.Checked ? Properties.Settings.Default.BeepScroll.ToString() : Properties.Settings.Default.Beep.ToString())) + "\""
                        + " \"" + ((chkScrollSpecific.Checked ? Properties.Settings.Default.ScrollLockOnSound : onsound)) + "\""
                        + " \"" + ((chkScrollSpecific.Checked ? Properties.Settings.Default.ScrollLockOffSound : offsound)) + "\""
                        ;
                    p.Start();
                }

                if (chkNumLock.Checked)
                {
                    p = new Process();
                    p.StartInfo.FileName = "KeyPressedNum.exe";
                    p.StartInfo.WorkingDirectory = Application.StartupPath;
                    p.StartInfo.Arguments = "\"NUM\" \"" + ColorToString((chkNumSpecific.Checked ? Properties.Settings.Default.NumLockOnColor : Properties.Settings.Default.OnColor)) + "\" \""
                        + ColorToString((chkNumSpecific.Checked ? Properties.Settings.Default.NumLockOffColor : Properties.Settings.Default.OffColor)) + "\""
                        + " \"" + ((chkNumSpecific.Checked ? Properties.Settings.Default.BeepNum.ToString() : Properties.Settings.Default.Beep.ToString())) + "\""
                        + " \"" + ((chkNumSpecific.Checked ? Properties.Settings.Default.NumLockOnSound : onsound)) + "\""
                        + " \"" + ((chkNumSpecific.Checked ? Properties.Settings.Default.NumLockOffSound : offsound)) + "\""
                        ;
                    p.Start();
                }
            }

            if (chkScreenMessage.Checked)
            {
                frmBalloonCaps = new frmBalloonTip((frmBalloonTip.LocationEnum)cmbScreenMessage.SelectedIndex,
                    "Caps Lock", (int)nudFontSize.Value, (int)nudOffsetX.Value, (int)nudOffsetY.Value,
                    (chkCapsSpecific.Checked ? Properties.Settings.Default.CapsLockOnColor : Properties.Settings.Default.OnColor),
                    (chkCapsSpecific.Checked ? Properties.Settings.Default.CapsLockOffColor : Properties.Settings.Default.OffColor));

                frmBalloonInsert = new frmBalloonTip((frmBalloonTip.LocationEnum)cmbScreenMessage.SelectedIndex,
                    "Insert", (int)nudFontSize.Value, (int)nudOffsetX.Value, (int)nudOffsetY.Value,
                    (chkInsSpecific.Checked ? Properties.Settings.Default.InsertOnColor : Properties.Settings.Default.OnColor),
                    (chkInsSpecific.Checked ? Properties.Settings.Default.InsertOffColor : Properties.Settings.Default.OffColor));

                frmBalloonNum = new frmBalloonTip((frmBalloonTip.LocationEnum)cmbScreenMessage.SelectedIndex,
                    "Num Lock", (int)nudFontSize.Value, (int)nudOffsetX.Value, (int)nudOffsetY.Value,
                    (chkNumSpecific.Checked ? Properties.Settings.Default.NumLockOnColor : Properties.Settings.Default.OnColor),
                    (chkNumSpecific.Checked ? Properties.Settings.Default.NumLockOffColor : Properties.Settings.Default.OffColor));

                frmBalloonScroll = new frmBalloonTip((frmBalloonTip.LocationEnum)cmbScreenMessage.SelectedIndex,
                    "Scroll Lock", (int)nudFontSize.Value, (int)nudOffsetX.Value, (int)nudOffsetY.Value,
                    (chkScrollSpecific.Checked ? Properties.Settings.Default.ScrollLockOnColor : Properties.Settings.Default.OnColor),
                    (chkScrollSpecific.Checked ? Properties.Settings.Default.ScrollLockOffColor : Properties.Settings.Default.OffColor));

                lstfrmBaloonTip.Clear();

                lstfrmBaloonTip.Add(frmBalloonNum);
                lstfrmBaloonTip.Add(frmBalloonScroll);
                lstfrmBaloonTip.Add(frmBalloonInsert);
                lstfrmBaloonTip.Add(frmBalloonCaps);
                
            }

            IndicateKey[0] = chkNumLock.Checked;
            IndicateKey[1] = chkScrollLock.Checked;
            IndicateKey[2] = chkInsert.Checked;
            IndicateKey[3] = chkCapsLock.Checked;

            BeepKey[0]=chkNumSpecific.Checked?chkNumBeep.Checked:chkBeep.Checked;
            BeepKey[1]=chkScrollSpecific.Checked?chkScrollBeep.Checked:chkBeep.Checked;
            BeepKey[2]=chkInsSpecific.Checked?chkInsBeep.Checked:chkBeep.Checked;
            BeepKey[3]=chkCapsSpecific.Checked?chkCapsBeep.Checked:chkBeep.Checked;

            for (int k = 0; k < lstKeyNotifyIcon.Count; k++)
            {
                lstKeyNotifyIcon[k].notIcon.Visible = false;
                lstKeyNotifyIcon[k].notIcon.Dispose();
                lstKeyNotifyIcon[k].notIcon = null;
            }

            lstKeyNotifyIcon.Clear();

            if (chkSystemTray.Checked)
            {                                
                KeyNotifyIcon kn = new KeyNotifyIcon("N", "Num Lock",(chkNumSpecific.Checked ? Properties.Settings.Default.NumLockOnColor : Properties.Settings.Default.OnColor),
                    (chkNumSpecific.Checked ? Properties.Settings.Default.NumLockOffColor : Properties.Settings.Default.OffColor));

                lstKeyNotifyIcon.Add(kn);

                kn = new KeyNotifyIcon("S", "Scroll Lock",(chkScrollSpecific.Checked ? Properties.Settings.Default.ScrollLockOnColor : Properties.Settings.Default.OnColor),
                    (chkScrollSpecific.Checked ? Properties.Settings.Default.ScrollLockOffColor : Properties.Settings.Default.OffColor));

                lstKeyNotifyIcon.Add(kn);

                kn = new KeyNotifyIcon("I", "Insert", (chkInsSpecific.Checked ? Properties.Settings.Default.InsertOnColor : Properties.Settings.Default.OnColor),
                    (chkInsSpecific.Checked ? Properties.Settings.Default.InsertOffColor : Properties.Settings.Default.OffColor));

                lstKeyNotifyIcon.Add(kn);

                kn = new KeyNotifyIcon("C", "Caps Lock",(chkCapsSpecific.Checked ? Properties.Settings.Default.CapsLockOnColor : Properties.Settings.Default.OnColor),
                    (chkCapsSpecific.Checked ? Properties.Settings.Default.CapsLockOffColor : Properties.Settings.Default.OffColor));

                lstKeyNotifyIcon.Add(kn);                
                
            }

            // =================

            lstMCIOn.Clear();
            lstMCIOff.Clear();

            string keyons=((chkNumSpecific.Checked?Properties.Settings.Default.NumLockOnSound: onsound));
            string keyoffs = ((chkNumSpecific.Checked ? Properties.Settings.Default.NumLockOffSound : offsound));

            MciAudio mci = new MciAudio();
            mci.PlayMediaFile = keyons;

            lstMCIOn.Add(mci);

            mci = new MciAudio();
            mci.PlayMediaFile = keyoffs;

            lstMCIOff.Add(mci);            

            // =============

            keyons=((chkScrollSpecific.Checked ? Properties.Settings.Default.ScrollLockOnSound : onsound));
            keyoffs = ((chkScrollSpecific.Checked ? Properties.Settings.Default.ScrollLockOffSound : offsound));            

            mci = new MciAudio();
            mci.PlayMediaFile = keyons;

            lstMCIOn.Add(mci);

            mci = new MciAudio();
            mci.PlayMediaFile = keyoffs;

            lstMCIOff.Add(mci);

            //================
            
            keyons=((chkInsSpecific.Checked ? Properties.Settings.Default.InsertOnSound : onsound));
            keyoffs = ((chkInsSpecific.Checked ? Properties.Settings.Default.InsertOffSound : offsound));            

            mci = new MciAudio();
            mci.PlayMediaFile = keyons;

            lstMCIOn.Add(mci);

            mci = new MciAudio();
            mci.PlayMediaFile = keyoffs;

            lstMCIOff.Add(mci);

            //===================

            keyons=((chkCapsSpecific.Checked ? Properties.Settings.Default.CapsLockOnSound : onsound));
            keyoffs = ((chkCapsSpecific.Checked ? Properties.Settings.Default.CapsLockOffSound : offsound));

            mci = new MciAudio();
            mci.PlayMediaFile = keyons;

            lstMCIOn.Add(mci);

            mci = new MciAudio();
            mci.PlayMediaFile = keyoffs;

            lstMCIOff.Add(mci);

            timKey.Enabled = true;
        }

        private string ColorToString(System.Drawing.Color col)
        {
            return col.R.ToString("D3") + "-" + col.G.ToString("D3") + "-" + col.B.ToString("D3");
        }

        private System.Drawing.Color StringToColor(string color)
        {
            System.Drawing.Color col = System.Drawing.Color.FromArgb
                (
                int.Parse(color.Substring(0, 3)),
                int.Parse(color.Substring(4, 3)),
                int.Parse(color.Substring(8, 3)));

            return col;
        }

        private bool Init2 = false;
        
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ShowIcons()
        {
            if (bmpNumLockOn==null)
            {
                CreateIcons();
            }

            if (chkCapsLock.Checked && frmCapsLock == null)
            {
                frmCapsLock = new frmKeyPressed();
                frmCapsLock.Icon=IconCapsLockOff;
                frmCapsLock.Show();
            }

            if (chkScrollLock.Checked && frmScrollLock == null)
            {
                frmScrollLock = new frmKeyPressed();
                frmScrollLock.Icon=IconScrollLockOff;
                frmScrollLock.Show();
            }

            if (chkInsert.Checked && frmInsert == null)
            {
                frmInsert = new frmKeyPressed();
                frmInsert.Icon=IconInsertOff;
                frmInsert.Show();
            }

            if (chkNumLock.Checked && frmNumLock == null)
            {
                frmNumLock = new frmKeyPressed();
                frmNumLock.Icon=IconNumLockOff;
                frmNumLock.Show();
            }                      
        }

        private void CreateIcons()
        {
            bmpNumLockOn = new Bitmap(48, 48);

            using (Graphics g = Graphics.FromImage(bmpNumLockOn))
            {
                try
                {
                    g.Clear(Color.White);
                        
                    Font fIcon = new Font(Font.FontFamily, 16, FontStyle.Bold);                        

                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;                        

                    SolidBrush sb=new SolidBrush(Properties.Settings.Default.OnColor);

                    g.DrawString("NUM", fIcon, sb, new PointF(3, 8));                        

                    sb.Dispose();

                    fIcon.Dispose();

                }
                catch { }

            }               

            IconNumLockOn  = Icon.FromHandle(bmpNumLockOn.GetHicon());

            //===========

            bmpNumLockOff = new Bitmap(48, 48);

            using (Graphics g = Graphics.FromImage(bmpNumLockOff))
            {
                try
                {
                    g.Clear(Color.White);
                        
                    Font fIcon = new Font(Font.FontFamily, 16, FontStyle.Bold);                        

                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;                        

                    SolidBrush sb=new SolidBrush(Properties.Settings.Default.OffColor);

                    g.DrawString("NUM", fIcon, sb, new PointF(3, 8));                        

                    sb.Dispose();

                    fIcon.Dispose();

                }
                catch { }

            }               

            IconNumLockOff  = Icon.FromHandle(bmpNumLockOff.GetHicon());

            //==============

            bmpScrollLockOn = new Bitmap(48, 48);

            using (Graphics g = Graphics.FromImage(bmpScrollLockOn))
            {
                try
                {
                    g.Clear(Color.White);
                        
                    Font fIcon = new Font(Font.FontFamily, 16, FontStyle.Bold);                        

                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;                        

                    SolidBrush sb=new SolidBrush(Properties.Settings.Default.OnColor);

                    g.DrawString("SCROLL", fIcon, sb, new PointF(3, 8));                        

                    sb.Dispose();

                    fIcon.Dispose();

                }
                catch { }

            }               

            IconScrollLockOn  = Icon.FromHandle(bmpScrollLockOn.GetHicon());        

        //===============

            bmpScrollLockOff = new Bitmap(48, 48);

            using (Graphics g = Graphics.FromImage(bmpScrollLockOff))
            {
                try
                {
                    g.Clear(Color.White);
                        
                    Font fIcon = new Font(Font.FontFamily, 16, FontStyle.Bold);                        

                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;                        

                    SolidBrush sb=new SolidBrush(Properties.Settings.Default.OffColor);

                    g.DrawString("SCROLL", fIcon, sb, new PointF(3, 8));                        

                    sb.Dispose();

                    fIcon.Dispose();

                }
                catch { }

            }               

            IconScrollLockOff  = Icon.FromHandle(bmpScrollLockOff.GetHicon());

            //==============

            bmpCapsLockOn = new Bitmap(48, 48);

            using (Graphics g = Graphics.FromImage(bmpCapsLockOn))
            {
                try
                {
                    g.Clear(Color.White);
                        
                    Font fIcon = new Font(Font.FontFamily, 16, FontStyle.Bold);                        

                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;                        

                    SolidBrush sb=new SolidBrush(Properties.Settings.Default.OnColor);

                    g.DrawString("CAPS", fIcon, sb, new PointF(3, 8));                        

                    sb.Dispose();

                    fIcon.Dispose();

                }
                catch { }

            }               

            IconCapsLockOn  = Icon.FromHandle(bmpCapsLockOn.GetHicon());        

            //==============

            bmpCapsLockOff = new Bitmap(48, 48);

            using (Graphics g = Graphics.FromImage(bmpCapsLockOff))
            {
                try
                {
                    g.Clear(Color.White);
                        
                    Font fIcon = new Font(Font.FontFamily, 16, FontStyle.Bold);                        

                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;                        

                    SolidBrush sb=new SolidBrush(Properties.Settings.Default.OffColor);

                    g.DrawString("CAPS", fIcon, sb, new PointF(3, 8));                        

                    sb.Dispose();

                    fIcon.Dispose();

                }
                catch { }

            }               

            IconCapsLockOff  = Icon.FromHandle(bmpCapsLockOff.GetHicon());      
  
            //==============

            bmpInsertOn = new Bitmap(48, 48);

            using (Graphics g = Graphics.FromImage(bmpInsertOn))
            {
                try
                {
                    g.Clear(Color.White);
                        
                    Font fIcon = new Font(Font.FontFamily, 16, FontStyle.Bold);                        

                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;                        

                    SolidBrush sb=new SolidBrush(Properties.Settings.Default.OnColor);

                    g.DrawString("INS", fIcon, sb, new PointF(3, 8));                        

                    sb.Dispose();

                    fIcon.Dispose();

                }
                catch { }

            }               

            IconInsertOn  = Icon.FromHandle(bmpInsertOn.GetHicon());        

            //==============

            bmpInsertOff = new Bitmap(48, 48);

            using (Graphics g = Graphics.FromImage(bmpInsertOff))
            {
                try
                {
                    g.Clear(Color.White);
                        
                    Font fIcon = new Font(Font.FontFamily, 16, FontStyle.Bold);                        

                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;                        

                    SolidBrush sb=new SolidBrush(Properties.Settings.Default.OffColor);

                    g.DrawString("INS", fIcon, sb, new PointF(3, 8));                        

                    sb.Dispose();

                    fIcon.Dispose();

                }
                catch { }

            }               

            IconInsertOff  = Icon.FromHandle(bmpInsertOff.GetHicon());        
        }        

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;

            this.WindowState = FormWindowState.Normal;

            this.BringToFront();

            notMain.Visible = false;            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveSettings();

            PreviousValues[0] = -1;
            PreviousValues[1] = -1;
            PreviousValues[2] = -1;
            PreviousValues[3] = -1;

            InitializeIcons();

            if (chkMinimizeOnOK.Checked)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private int[] PreviousValues = new int[4];

        private bool InTimer = false;

        private void timKey_Tick(object sender, EventArgs e)
        {
            if (InTimer) return;

            try
            {
                InTimer = true;

                for (int k = 0; k <= 3; k++)
                {
                    if (!IndicateKey[k]) continue;

                    int x = 0;

                    if (k == 0)
                    {
                        //x = GetAsyncKeyState(Keys.NumLock);

                        x = Control.IsKeyLocked(Keys.NumLock) ? 1 : 0;
                    }
                    else if (k == 1)
                    {
                        //x = GetAsyncKeyState(Keys.Scroll);

                        x = Control.IsKeyLocked(Keys.Scroll) ? 1 : 0;
                    }
                    else if (k == 2)
                    {
                        //x = GetAsyncKeyState(Keys.Insert);

                        x = Control.IsKeyLocked(Keys.Insert) ? 1 : 0;
                    }
                    else if (k == 3)
                    {
                        //x = GetAsyncKeyState(Keys.CapsLock);

                        x = Control.IsKeyLocked(Keys.CapsLock) ? 1 : 0;
                    }

                    if ((x == 1) || (x == Int16.MinValue)) //Use constants (0x8000 and -32768 is Int16.MaxValue)
                    {
                        if (PreviousValues[k] == 0 && Properties.Settings.Default.ScreenMessage)
                        {
                            //this.Icon = IconOn;
                            //Module.ShowMessage("true");

                            lstfrmBaloonTip[k].ShowBalloon(true);
                        }

                        if (PreviousValues[k] == 0 && BeepKey[k])
                        {
                            Console.Beep();
                        }

                        if (PreviousValues[k] == 0 && lstMCIOn[k].PlayMediaFile != string.Empty)
                        {
                            lstMCIOn[k].StopPlay();
                            lstMCIOn[k].Close();

                            lstMCIOn[k].Initialize();
                            lstMCIOn[k].OpenPlay();
                            lstMCIOn[k].Play();
                        }

                        if ((PreviousValues[k] == -1 || PreviousValues[k] == 0) && Properties.Settings.Default.SystemTrayIcon)
                        {
                            //this.Icon = IconOn;

                            lstKeyNotifyIcon[k].ShowOn();
                        }

                        PreviousValues[k] = 1;
                    }
                    else if (x == 0)
                    {
                        if (PreviousValues[k] == 1 && Properties.Settings.Default.ScreenMessage)
                        {
                            //this.Icon = IconOff;                    

                            //Module.ShowMessage("false");

                            lstfrmBaloonTip[k].ShowBalloon(false);
                        }

                        if (PreviousValues[k] == 1 && BeepKey[k])
                        {
                            Console.Beep();
                        }

                        if (PreviousValues[k] == 1 && lstMCIOff[k].PlayMediaFile != string.Empty)
                        {
                            lstMCIOff[k].StopPlay();
                            lstMCIOff[k].Close();

                            lstMCIOff[k].Initialize();
                            lstMCIOff[k].OpenPlay();
                            lstMCIOff[k].Play();
                        }

                        if ((PreviousValues[k] == -1 || PreviousValues[k] == 1) && Properties.Settings.Default.SystemTrayIcon)
                        {
                            lstKeyNotifyIcon[k].ShowOff();
                        }

                        PreviousValues[k] = 0;
                    }
                }
            }
            finally
            {
                InTimer = false;
            }
        }

        private void chkRunCurrentUser_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRunCurrentUser.Checked)
            {
                chkRunLocalMachine.Checked = false;
            }            
        }

        private void chkRunLocalMachine_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRunLocalMachine.Checked)
            {
                chkRunCurrentUser.Checked = false;
            }
        }

        private void showApplicationTrayIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LoadComplete)
            {
                showApplicationTrayIconToolStripMenuItem.Checked = !showApplicationTrayIconToolStripMenuItem.Checked;

                Properties.Settings.Default.ShowTrayIcon = showApplicationTrayIconToolStripMenuItem.Checked;

                notMain.Visible = Properties.Settings.Default.ShowTrayIcon;
            }
        }

        private void minimizeToSystemTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LoadComplete)
            {            
                minimizeToSystemTrayToolStripMenuItem.Checked = !minimizeToSystemTrayToolStripMenuItem.Checked;

                Properties.Settings.Default.MinimizeSystemTray = minimizeToSystemTrayToolStripMenuItem.Checked;
            }
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.MinimizeSystemTray)
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    this.Visible = false;

                    notMain.Visible = true;
                }
            }
        }

        private void btnBrowseOn_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Audio Files (*.wav;*.mp3)|*.wav;*.mp3";

            if (opf.ShowDialog() == DialogResult.OK)
            {                
                if (sender == btnBrowseNumOffS)
                {
                    cmbNumOffS.Items.Add(opf.FileName); cmbNumOffS.SelectedIndex = cmbNumOffS.Items.Count - 1;
                    chkNumOffS.Checked = true;
                }
                if (sender == btnBrowseNumOnS)
                {
                    cmbNumOnS.Items.Add(opf.FileName); cmbNumOnS.SelectedIndex = cmbNumOnS.Items.Count - 1;
                    chkNumOnS.Checked = true;
                }
                else if (sender == btnBrowseOn)
                {
                    cmbOnSound.Items.Add(opf.FileName); cmbOnSound.SelectedIndex = cmbOnSound.Items.Count - 1;
                    chkOnSound.Checked = true;
                }                
                else if (sender == btnBrowseOff)
                {
                    cmbOffSound.Items.Add(opf.FileName); cmbOffSound.SelectedIndex = cmbOffSound.Items.Count - 1;
                    chkOffSound.Checked = true;
                }
                
                else if (sender == btnBrowseCapsOffS)
                {
                    cmbCapsOffS.Items.Add(opf.FileName); cmbCapsOffS.SelectedIndex = cmbCapsOffS.Items.Count - 1;
                    chkCapsOffS.Checked = true;
                }
                else if (sender == btnBrowseCapsOnS)
                {
                    cmbCapsOnS.Items.Add(opf.FileName); cmbCapsOnS.SelectedIndex = cmbCapsOnS.Items.Count - 1;
                    chkCapsOnS.Checked = true;
                }
                else if (sender == btnBrowseInsOffS)
                {
                    cmbInsOffS.Items.Add(opf.FileName); cmbInsOffS.SelectedIndex = cmbInsOffS.Items.Count - 1;
                    chkInsOffS.Checked = true;
                }
                else if (sender == btnBrowseInsOnS)
                {
                    cmbInsOn.Items.Add(opf.FileName); cmbInsOn.SelectedIndex = cmbInsOn.Items.Count - 1;
                    chkInsOns.Checked = true;
                }
                else if (sender == btnScrollOffS)
                {
                    cmbScrollOffS.Items.Add(opf.FileName); cmbScrollOffS.SelectedIndex = cmbScrollOffS.Items.Count - 1;
                    chkScrollOffS.Checked = true;
                }
                else if (sender == btnScrollOnS)
                {
                    cmbScrollOnS.Items.Add(opf.FileName); cmbScrollOnS.SelectedIndex = cmbScrollOnS.Items.Count - 1;
                    chkScrollOnS.Checked = true;
                }               
            }
        }

        private void saveSettingsFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSettings();

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Key Status Indicator 4dots Settings (*.set)|*.set";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new System.IO.StreamWriter(sfd.FileName))
                {
                    sw.WriteLine("# Key Status Indicator 4dots Settings File - Be careful if you edit it !");

                    sw.WriteLine(ColorToString(Properties.Settings.Default.OnColor));

                    sw.WriteLine(ColorToString(Properties.Settings.Default.OffColor));

                    sw.WriteLine(Properties.Settings.Default.OnFilepath);

                    sw.WriteLine(Properties.Settings.Default.OffFilepath);

                    sw.WriteLine(Properties.Settings.Default.ScreenMsgFontSize);

                    sw.WriteLine(Properties.Settings.Default.Beep);

                    sw.WriteLine(Properties.Settings.Default.CapsLock);
                    
                    sw.WriteLine(Properties.Settings.Default.NumLock);
                    
                    sw.WriteLine(Properties.Settings.Default.ScrollLock);
                    
                    sw.WriteLine(Properties.Settings.Default.Insert);

                    sw.WriteLine(Properties.Settings.Default.OffsetX);

                    sw.WriteLine(Properties.Settings.Default.OffsetY);

                    sw.WriteLine(Properties.Settings.Default.ScreenMsgFontSize);

                    sw.WriteLine(Properties.Settings.Default.ScreenMsgLocation);

                    sw.WriteLine(Properties.Settings.Default.Icon);

                    sw.WriteLine(Properties.Settings.Default.SystemTrayIcon);

                    sw.WriteLine(Properties.Settings.Default.ScreenMessage);

                    sw.WriteLine(Properties.Settings.Default.CapsLockOffSound);

                    sw.WriteLine(Properties.Settings.Default.CapsLockOnSound);

                    sw.WriteLine(Properties.Settings.Default.InsertOffSound);

                    sw.WriteLine(Properties.Settings.Default.InsertOnSound);

                    sw.WriteLine(Properties.Settings.Default.NumLockOffSound);

                    sw.WriteLine(Properties.Settings.Default.NumLockOnSound);

                    sw.WriteLine(Properties.Settings.Default.ScrollLockOffSound);

                    sw.WriteLine(Properties.Settings.Default.ScrollLockOnSound);

                    sw.WriteLine(Properties.Settings.Default.SpecialNum);

                    sw.WriteLine(Properties.Settings.Default.SpecialCaps);

                    sw.WriteLine(Properties.Settings.Default.SpecialScroll);

                    sw.WriteLine(Properties.Settings.Default.SpecialInsert);

                    sw.WriteLine(ColorToString(Properties.Settings.Default.NumLockOffColor));

                    sw.WriteLine(ColorToString(Properties.Settings.Default.NumLockOnColor));

                    sw.WriteLine(ColorToString(Properties.Settings.Default.CapsLockOffColor));

                    sw.WriteLine(ColorToString(Properties.Settings.Default.CapsLockOnColor));

                    sw.WriteLine(ColorToString(Properties.Settings.Default.ScrollLockOffColor));

                    sw.WriteLine(ColorToString(Properties.Settings.Default.ScrollLockOnColor));

                    sw.WriteLine(ColorToString(Properties.Settings.Default.InsertOffColor));

                    sw.WriteLine(ColorToString(Properties.Settings.Default.InsertOnColor));

                    sw.WriteLine(Properties.Settings.Default.BeepNum);

                    sw.WriteLine(Properties.Settings.Default.BeepCaps);

                    sw.WriteLine(Properties.Settings.Default.BeepScroll);

                    sw.WriteLine(Properties.Settings.Default.BeepInsert);

                    sw.WriteLine(Properties.Settings.Default.OnSound);

                    sw.WriteLine(Properties.Settings.Default.OffSound);                    

                    sw.WriteLine(Properties.Settings.Default.MinimizeSystemTray);

                    sw.WriteLine(Properties.Settings.Default.ShowTrayIcon);

                    sw.WriteLine(Properties.Settings.Default.chkCapsOffS);

                    sw.WriteLine(Properties.Settings.Default.chkCapsOnS);

                    sw.WriteLine(Properties.Settings.Default.chkInsOffS);

                    sw.WriteLine(Properties.Settings.Default.chkInsOnS);

                    sw.WriteLine(Properties.Settings.Default.chkNumOffS);

                    sw.WriteLine(Properties.Settings.Default.chkNumOnS);

                    sw.WriteLine(Properties.Settings.Default.chkScrollOffS);

                    sw.WriteLine(Properties.Settings.Default.chkScrollOnS);
                }
            }
        }

        private void openSettingsFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Key Status Indicator 4dots Settings (*.set)|*.set";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new System.IO.StreamReader(ofd.FileName))
                {
                    sr.ReadLine();

                    Properties.Settings.Default.OnColor = StringToColor(sr.ReadLine());

                    Properties.Settings.Default.OffColor = StringToColor(sr.ReadLine());

                    Properties.Settings.Default.OnFilepath = sr.ReadLine();

                    Properties.Settings.Default.OffFilepath = sr.ReadLine();

                    Properties.Settings.Default.ScreenMsgFontSize = int.Parse(sr.ReadLine());

                    Properties.Settings.Default.Beep = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.CapsLock = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.NumLock = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.ScrollLock = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.Insert = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.OffsetX = int.Parse(sr.ReadLine());

                    Properties.Settings.Default.OffsetY = int.Parse(sr.ReadLine());

                    Properties.Settings.Default.ScreenMsgFontSize = int.Parse(sr.ReadLine());

                    Properties.Settings.Default.ScreenMsgLocation = int.Parse(sr.ReadLine());

                    Properties.Settings.Default.Icon = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.SystemTrayIcon = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.ScreenMessage = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.CapsLockOffSound = sr.ReadLine();

                    Properties.Settings.Default.CapsLockOnSound = sr.ReadLine();

                    Properties.Settings.Default.InsertOffSound = sr.ReadLine();

                    Properties.Settings.Default.InsertOnSound = sr.ReadLine();

                    Properties.Settings.Default.NumLockOffSound = sr.ReadLine();

                    Properties.Settings.Default.NumLockOnSound = sr.ReadLine();

                    Properties.Settings.Default.ScrollLockOffSound = sr.ReadLine();

                    Properties.Settings.Default.ScrollLockOnSound = sr.ReadLine();

                    Properties.Settings.Default.SpecialNum = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.SpecialCaps = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.SpecialScroll = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.SpecialInsert = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.NumLockOffColor = StringToColor(sr.ReadLine());

                    Properties.Settings.Default.NumLockOnColor = StringToColor(sr.ReadLine());

                    Properties.Settings.Default.CapsLockOffColor = StringToColor(sr.ReadLine());

                    Properties.Settings.Default.CapsLockOnColor = StringToColor(sr.ReadLine());

                    Properties.Settings.Default.ScrollLockOffColor = StringToColor(sr.ReadLine());

                    Properties.Settings.Default.ScrollLockOnColor = StringToColor(sr.ReadLine());

                    Properties.Settings.Default.InsertOffColor = StringToColor(sr.ReadLine());

                    Properties.Settings.Default.InsertOnColor = StringToColor(sr.ReadLine());

                    Properties.Settings.Default.BeepNum = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.BeepCaps = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.BeepScroll = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.BeepInsert = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.OnSound = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.OffSound = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.MinimizeSystemTray = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.ShowTrayIcon = StringToBool(sr.ReadLine());

                    Properties.Settings.Default.chkCapsOffS= StringToBool(sr.ReadLine());

                    Properties.Settings.Default.chkCapsOnS= StringToBool(sr.ReadLine());

                    Properties.Settings.Default.chkInsOffS= StringToBool(sr.ReadLine());

                    Properties.Settings.Default.chkInsOnS= StringToBool(sr.ReadLine());

                    Properties.Settings.Default.chkNumOffS= StringToBool(sr.ReadLine());

                    Properties.Settings.Default.chkNumOnS= StringToBool(sr.ReadLine());

                    Properties.Settings.Default.chkScrollOffS= StringToBool(sr.ReadLine());

                    Properties.Settings.Default.chkScrollOnS = StringToBool(sr.ReadLine());

                    LoadSettings();
                }
            }
        }

        private bool StringToBool(string str)
        {
            return str == bool.TrueString;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == MessageHelper.WM_COPYDATA)
            {
                MessageHelper.COPYDATASTRUCT mystr = new MessageHelper.COPYDATASTRUCT();
                Type mytype = mystr.GetType();
                mystr = (MessageHelper.COPYDATASTRUCT)m.GetLParam(mytype);

                string arg = mystr.lpData;

                if (arg == "SHOW")
                {
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    this.Show();
                    this.BringToFront();
                }
            }
            else if (m.Msg == MessageHelper.WM_ACTIVATEAPP)
            {
                this.Show();

                this.WindowState = FormWindowState.Normal;
                this.Show();
                this.BringToFront();
            }
            else
            {
                base.WndProc(ref m);
            }

        }

        private void notMain_Click(object sender, EventArgs e)
        {
            showToolStripMenuItem_Click(null, null);
        }
    }
}
