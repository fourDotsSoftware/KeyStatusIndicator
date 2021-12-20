using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyPressed
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length < 3)
            {
                Environment.Exit(0);
                return;
            }

            string caption = args[0];

            string coloron = args[1];

            string coloroff = args[2];

            bool beep = (args[3] == bool.TrueString);

            string onsound = args[4];

            string offsound = args[5];

            //string onsound = args[4].Substring(1, args[4].Length - 2);

            //string offsound = args[5].Substring(1, args[5].Length - 2);

            System.Drawing.Color colOn = System.Drawing.Color.FromArgb
                (
                int.Parse(coloron.Substring(0, 3)),
                int.Parse(coloron.Substring(4, 3)),
                int.Parse(coloron.Substring(8, 3)));

            System.Drawing.Color colOff = System.Drawing.Color.FromArgb
                (
                int.Parse(coloroff.Substring(0, 3)),
                int.Parse(coloroff.Substring(4, 3)),
                int.Parse(coloroff.Substring(8, 3)));


            Application.Run(new frmKeyPressed(caption,colOn,colOff,beep,onsound,offsound));
        }
    }
}
