using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RecoilAssist.Controllers
{
    /// <summary>
    /// "Pracownik" odpowiadaj¹cy za wykonanie serii wzorców (MovementPattern) w tle
    /// </summary>
    public class MouseWorker
    {
        private Thread thread;

        /// <summary>
        /// Wektor ró¿nicowy który pracownik ostatnio obs³u¿y³
        /// </summary>
        private Vector lastTargetPoint;

        // Wzorce które maja siê wykonaæ
        public List<Patterns.MovementPattern> Patterns { get; private set; }

        public MouseWorker(IEnumerable<Patterns.MovementPattern> movementPatterns)
        {
            Patterns = new List<Patterns.MovementPattern>();
            Patterns.AddRange(movementPatterns);
        }

        /// <summary>
        /// Uruchamia MouseWorker'a w celu wykonania wzorców w tle
        /// </summary>
        public void Fire()
        {
            if (Patterns.Count == 0)
                return;

            thread = new Thread(() =>
            {
                var MaxTime = Patterns.Max(p => p.StopTime);

                if (MaxTime > new TimeSpan(0, 1, 0))
                    throw new Exception("Time of MouseWorker is too long! Set of patterns contains StopTime that is over 1 minute.");

                lastTargetPoint = Vector.Zero;
                var startDateTime = DateTime.Now;
                var stopDateTime = startDateTime.Add(MaxTime);

                try
                {
                    while (DateTime.Now <= stopDateTime)
                    {
                        Move(startDateTime, DateTime.Now);
                    }
                    Move(startDateTime, stopDateTime);
                }
                catch (Exception e)
                {
                    Move(startDateTime, DateTime.Now);
                }
            });
            thread.Start();
        }

        /// <summary>
        /// G³ówna funkcja przemieszczaj¹ca kursor myszki wzglêdem dt (dt = aktualny_czas - start_czas)
        /// 
        /// Wektor ró¿nicowy  - wektor reprezentuj¹cy drogê jak¹ pokona³a, lub ma pokonaæ myszka, 
        ///                     wektor nie zale¿y od aktualnego po³o¿enia myszki, dziêki czemu
        ///                     u¿ytkownik ma kontrolê nad myszk¹ podczas dzia³ania MouseWorker'a
        /// </summary>
        /// <param name="startTime">czas uruchomienia MouseWorker'a</param>
        /// <param name="currentTime">aktualny czas</param>
        private void Move(DateTime startTime, DateTime currentTime)
        {
            // zresetuj wektor ró¿nicowy
            Vector targetPoint = Vector.Zero;

            // oblicz dt = aktualny_czas - start_czas
            TimeSpan deltaTime = currentTime.Subtract(startTime);

            // oblicz docelowy wektor ró¿nicowy
            foreach (var pattern in Patterns)
                targetPoint += pattern.GetDelta(deltaTime);

            // oblicz deltê => wektor jaki musi pokonaæ kursor aby wektor ró¿nicowy siê zgadza³ z docelowym
            Vector deltaPoint = targetPoint - lastTargetPoint;
            
            // Przesuñ kursor o wektor delta
            MouseController.Move(deltaPoint.x, deltaPoint.y);

            lastTargetPoint = targetPoint;
            if (deltaPoint != Vector.Zero)
                Console.WriteLine(deltaPoint);
        }

        /// <summary>
        /// Przerywa dzia³anie workera
        /// </summary>
        public void Kill()
        {
            if (thread != null && thread.IsAlive)
                thread.Abort();
        }
    }
}
