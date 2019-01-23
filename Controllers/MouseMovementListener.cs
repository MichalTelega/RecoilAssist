using System;
using System.Threading;

namespace RecoilAssist.Controllers
{
    /// <summary>
    /// "S³uchacz" który nas³uchuje zdarzenia podnoszone przez dynamikê myszki.
    /// Po wykryciu zdarzenia uruchamia odpowiedni Event Handler
    /// </summary>
    public class MouseMovementListener
    {
        // Punkt reprezentuj¹cy NULL (brak punktu), jako ¿e punkty odnosz¹ siê do wspó³rzêdnych ekranowych mo¿na przyj¹æ ¿e punkt (-1,-1) nie istnieje.
        private static readonly Vector NullPoint = new Vector(-1, -1);

        // Ostatni punkt który listener obs³u¿y³
        private Vector lastPoint = NullPoint;

        // true - jeœli do tej pory LPP by³ wciœniêty, false - w.p.p.
        private bool pressed;

        private Thread thread;

        public bool IsRunning => thread != null && thread.IsAlive;

        /// <summary>
        /// Uruchamia siê gdy MouseMovementListener wykry³ pierwsz¹ pozycjê myszki (inicjalizacja)
        /// </summary>
        public event EventHandler<Vector> OnInit;

        /// <summary>
        /// Uruchamia siê gdy myszka zmienia³a swoje wspó³rzêdne
        /// </summary>
        public event EventHandler<Vector> OnMove;

        /// <summary>
        /// Uruchamia siê w momencie wciœniêcia LPP (a wczeœniej nie by³)
        /// </summary>
        public event EventHandler<Vector> OnMousePressed;

        /// <summary>
        /// Uruchamia siê w momencie uwolnienia LPP (gdy by³ wczeœniej wciœniêty)
        /// </summary>
        public event EventHandler<Vector> OnMouseReleased;

        /// <summary>
        /// Uruchamia nas³uch
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
        /// Wstrzymuje nas³uch
        /// </summary>
        public void Stop()
        {
            if (IsRunning)
                thread.Abort();
        }

        /// <summary>
        /// Pêtla nas³uchuj¹ca dynamikê myszki
        /// </summary>
        private void ListenLoop()
        {
            bool exit = false;
            while (!exit)
            {
                try
                {
                    // punkt w którym znajduje siê myszka
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

                    // sprawdzenie wspó³rzêdnych
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
