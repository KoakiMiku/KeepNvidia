using System;
using System.Windows.Forms;

namespace NVIDIAKeeper
{
    static class Program
    {
        private static readonly string name = "NVIDIA-Keeper";

        private static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    switch (MessageBox.Show($"{I18N.GetString("Setup")}", name,
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information))
                    {
                        case DialogResult.Yes:
                            Autorun.Add();
                            KeepRunning.Start();
                            break;
                        case DialogResult.No:
                            Autorun.Remove();
                            KeepRunning.Stop();
                            break;
                    }
                }
                else if (args[0] == "-start")
                {
                    KeepRunning.Start();
                }
                else if (args[0] == "-stop")
                {
                    KeepRunning.Stop();
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
