using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RecoilAssist.Controllers
{
    /// <summary>
    /// Kontroler do obsługi okna konsoli na poziomie systemowym
    /// </summary>
    public static class ConsoleWindowController
    {
        /// <summary>
        /// <see href="https://docs.microsoft.com/en-us/windows/console/getconsolewindow">źródło</see>
        /// </summary>
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        /// <summary>
        /// <see>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-showwindow</see>
        /// </summary>
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        /// <summary>
        /// Wyświetla okno konsoli
        /// </summary>
        public static void ShowConsole()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_SHOW);
        }

        /// <summary>
        /// Ukrywa okno konsoli
        /// </summary>
        public static void HideConsole()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
        }
    }
}
