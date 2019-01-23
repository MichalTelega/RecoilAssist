using RecoilAssist.Controllers;
using RecoilAssist.Patterns;
using System;
using System.Collections.Generic;

namespace RecoilAssist
{
    /// <summary>
    /// Kontroler do korygowania ruchu myszki, tak aby niwelować efekt odrzutu broni w grach
    /// </summary>
    public class RecoilAssistController : IDisposable
    {
        /// <summary>
        /// "Słuchacz" który generuje zdarzenia, obsługiwane przez kontroler
        /// </summary>
        public MouseMovementListener Listener { get; private set; }

        /// <summary>
        /// Aktywny "pracownik" - wątek zmieniający współrzędne kursora myszy w zależności od wzorców
        /// </summary>
        private MouseWorker currentWorker;

        /// <summary>
        /// Przypięte wzorce, które będą przekazywane do podległych MouseWorker'ów
        /// </summary>
        public List<MovementPattern> AttachedPatterns { get; set; }

        /// <summary>
        /// true - jeśli kontroler nasłuchuje zdarzenia i koryguje ruchy myszki, false - w.p.p.
        /// </summary>
        public bool IsActive => Listener != null ? Listener.IsRunning : false;

        public RecoilAssistController()
        {
            AttachedPatterns = new List<MovementPattern>();

            Listener = new MouseMovementListener();
            Listener.OnMousePressed += Listener_OnMousePressed;
            Listener.OnMouseReleased += Listener_OnMouseReleased;
        }

        /// <summary>
        /// Włącz lub wyłącz kontroler
        /// </summary>
        /// <param name="flag">true - jeśli kontroler ma być ostatecznie włączony, false - jeśli kontroler ma być wyłączony</param>
        public void SetActive(bool flag)
        {
            if (flag)
                Listener.Listen();
            else
                Listener.Stop();
        }

        // Obsługa zdarzenia wciśnięcia myszki
        private void Listener_OnMouseReleased(object sender, Vector e)
        {
            if (currentWorker != null)
                currentWorker.Kill();
        }

        // Obsługa zdarzenia zwolnienia myszki
        private void Listener_OnMousePressed(object sender, Vector e)
        {
            if (currentWorker != null)
                currentWorker.Kill();
            currentWorker = new MouseWorker(AttachedPatterns);
            currentWorker.Fire();
        }

        // Zabija wątki uruchomione w tle
        public void Dispose()
        {
            if (currentWorker != null)
                currentWorker.Kill();
            Listener.Stop();
        }
    }
}
