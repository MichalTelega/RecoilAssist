using RecoilAssist.Controllers;
using RecoilAssist.DatabaseModule;
using RecoilAssist.UI_Console;
using RecoilAssist.UI_Graphical;
using System;
using System.Windows.Forms;

namespace RecoilAssist
{
    class Program
    {
        static void Main(string[] args)
        {
            // Konsola
            if (args.Length > 0 && args[0] == "-nogui")
            {
                bool inMemory = args.Length > 1 && args[1] == "-memory";
                (new ConsoleUI()
                {
                    In = Console.In,
                    Out = Console.Out,
                    DatabaseProvider = inMemory ? ((IDatabaseProvider)new MemoryDatabaseProvider()) : new DatabaseProvider(),
                }
                ).Run();
            }
            // GUI
            else
            {
                ConsoleWindowController.HideConsole();
                OpenGUI();
            }
        }

        [STAThread]
        static void OpenGUI()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RecoilAssistForm());
        }
    }
}
