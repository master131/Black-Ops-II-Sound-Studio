using BlackOps2SoundStudio.Decoders;
using BlackOps2SoundStudio.Encoders;
using System;
using System.Windows.Forms;

namespace BlackOps2SoundStudio
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
