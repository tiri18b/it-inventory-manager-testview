using System;
using System.Windows.Forms;
using ITInventoryManager.Forms;

namespace ITInventoryManager;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
