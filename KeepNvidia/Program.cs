using System;
using System.Windows.Forms;

namespace KeepNvidia
{
    static class Program
    {
        private static readonly string name = "KeepNvidia";

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
                            return;
                        default:
                            return;
                    }
                }
                else if (args[0] == "-start")
                {
                    KeepRunning.Start();
                }
                else
                {
                    throw new ArgumentException();
                }

                Application.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
