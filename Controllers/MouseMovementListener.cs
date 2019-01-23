using System;
using System.Threading;

namespace RecoilAssist.Controllers
{
    /// <summary>
    /// "S�uchacz" kt�ry nas�uchuje zdarzenia podnoszone przez dynamik� myszki.
    /// Po wykryciu zdarzenia uruchamia odpowiedni Event Handler
    /// </summary>
    public class MouseMovementListener
    {
        // Punkt reprezentuj�cy NULL (brak punktu), jako �e punkty odnosz� si� do wsp�rz�dnych ekranowych mo�na przyj�� �e punkt (-1,-1) nie istnieje.
        private static readonly Vector NullPoint = new Vector(-1, -1);

        // Ostatni punkt kt�ry listener obs�u�y�
        private Vector lastPoint = NullPoint;

        // true - je�li do tej pory LPP by� wci�ni�ty, false - w.p.p.
        private bool pressed;

        private Thread thread;

        public bool IsRunning => thread != null && thread.IsAlive;

        /// <summary>
        /// Uruchamia si� gdy MouseMovementListener wykry� pierwsz� pozycj� myszki (inicjalizacja)
        /// </summary>
        public event EventHandler<Vector> OnInit;

        /// <summary>
        /// Uruchamia si� gdy myszka zmienia�a swoje wsp�rz�dne
        /// </summary>
        public event EventHandler<Vector> OnMove;

        /// <summary>
        /// Uruchamia si� w momencie wci�ni�cia LPP (a wcze�niej nie by�)
        /// </summary>
        public event EventHandler<Vector> OnMousePressed;

        /// <summary>
        /// Uruchamia si� w momencie uwolnienia LPP (gdy by� wcze�niej wci�ni�ty)
        /// </summary>
        public event EventHandler<Vector> OnMouseReleased;

        /// <summary>
        /// Uruchamia nas�uch
        /// </summary>
        public void Listen()
        {
            if (IsRunning)
                thread.Abort();
            thread = new Thread(new ThreadStart(ListenLoop));
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }

        /// <summary>
        /// Wstrzymuje nas�uch
        /// </summary>
        public void Stop()
        {
            if (IsRunning)
                thread.Abort();
        }

        /// <summary>
        /// P�tla nas�uchuj�ca dynamik� myszki
        /// </summary>
        private void ListenLoop()
        {
            bool exit = false;
            while (!exit)
            {
                try
                {
                    // punkt w kt�rym znajduje si� myszka
                    var currentPoint = MouseController.GetMousePosition();
                    
                    // sprawdzenie LPP
                    if (!pressed && MouseController.IsMousePressed())
                    {
                        pressed = true;
                        OnMousePressed?.Invoke(this, currentPoint);
                    }
                    else if (pressed && !MouseController.IsMousePressed())
                    {
                        pressed = false;
                        OnMouseReleased?.Invoke(this, currentPoint);
                    }

                    // sprawdzenie wsp�rz�dnych
                    if (lastPoint == NullPoint)
                    {
                        lastPoint = currentPoint;
                        OnInit?.Invoke(this, currentPoint);
                    }
                    else if (lastPoint != currentPoint)
                    {
                        lastPoint = currentPoint;
                        OnMove?.Invoke(this, currentPoint);
                    }
                }
                // przerwij
                catch (ThreadAbortException e)
                {
                    exit = true;
                }
                // kontynuuj
                catch (Exception e) { }
            }
        }
    }
}
