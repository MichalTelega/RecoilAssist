using System;
using System.Runtime.InteropServices;

namespace RecoilAssist.Controllers
{
    /// <summary>
    /// Kontroler do obs³ugi myszki na poziomie systemowym
    /// 
    /// Kontroler komunikuje siê g³ównie za pomoc¹ komponentu USER
    /// <see>https://en.wikipedia.org/wiki/Windows_USER</see>
    /// </summary>
    public class MouseController
    {
        #region Praca z user32.dll
        /// <summary>
        /// Struktura reprezentuj¹ca punkt. S³u¿y do komunikacji z komponentem USER
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Vector(POINT point)
            {
                return new Vector(point.X, point.Y);
            }
        }

        // sta³e https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-mouse_event
        private const int MOUSEEVENTF_MOVE = 0x0001; /* mouse move */
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
        private const int MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008; /* right button down */
        private const int MOUSEEVENTF_RIGHTUP = 0x0010; /* right button up */
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020; /* middle button down */
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040; /* middle button up */
        private const int MOUSEEVENTF_XDOWN = 0x0080; /* x button down */
        private const int MOUSEEVENTF_XUP = 0x0100; /* x button down */
        private const int MOUSEEVENTF_WHEEL = 0x0800; /* wheel button rolled */
        private const int MOUSEEVENTF_VIRTUALDESK = 0x4000; /* map to entire virtual desktop */
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000; /* absolute move */

        private const ushort VK_LBUTTON = 0x01;
        private const ushort VK_RBUTTON = 0x02;

        private const ushort BUTTON_DOWN_CON = 0x8000;

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// <see>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-getcursorpos</see>
        /// </summary>
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        /// <summary>
        /// <see>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-mouse_event</see>
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        /// <summary>
        /// <see>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-setcursorpos</see>
        /// </summary>
        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);

        /// <summary>
        /// <see>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-getkeystate</see>
        /// </summary>
        [DllImport("user32.dll")]
        public static extern short GetKeyState(ushort virtualKeyCode);
        #endregion

        /// <summary>
        ///  Sprawdza aktualne wspó³rzêdne myszki
        /// </summary>
        /// <returns>aktualne wspó³rzêdne myszki</returns>
        public static Vector GetMousePosition()
        {
            POINT pointBuffer;
            if (!GetCursorPos(out pointBuffer))
                throw new Exception("Could not get mouse position");
            return pointBuffer;
        }

        /// <summary>
        /// Przemieszcza myszkê w pionie
        /// </summary>
        /// <param name="y">iloœæ pikseli która myszka pokona po wykonaniu tej metody</param>
        public static void MoveVertically(int y)
        {
            Vector current = GetMousePosition();
            SetCursorPos(current.x, current.y + y);
        }

        /// <summary>
        /// Przemieszcza myszkê w poziomie
        /// </summary>
        /// <param name="y">iloœæ pikseli która myszka pokona po wykonaniu tej metody</param>
        public static void MoveHorizontally(int x)
        {
            Vector current = GetMousePosition();
            SetCursorPos(current.x + x, current.y);
        }

        /// <summary>
        /// Przemieszcza myszkê o dany wektor
        /// </summary>
        /// <param name="x">iloœæ pikseli która myszka pokona w poziomie</param>
        /// <param name="y">iloœæ pikseli która myszka pokona w pionie</param>
        public static void Move(int x, int y)
        {
            Vector current = GetMousePosition();
            SetCursorPos(current.x + x, current.y + y);
        }

        /// <summary>
        /// Zwraca true - jeœli LPP jest wciœniêty, false - w przeciwnym wypadku
        /// </summary>
        public static bool IsMousePressed()
        {
            if ((BUTTON_DOWN_CON & GetKeyState(VK_LBUTTON)) != 0)
                return true;
            else
                return false;
        }
    }
}
