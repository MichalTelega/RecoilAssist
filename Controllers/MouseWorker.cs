using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RecoilAssist.Controllers
{
    /// <summary>
    /// "Pracownik" odpowiadaj�cy za wykonanie serii wzorc�w (MovementPattern) w tle
    /// </summary>
    public class MouseWorker
    {
        private Thread thread;

        /// <summary>
        /// Wektor r�nicowy kt�ry pracownik ostatnio obs�u�y�
        /// </summary>
        private Vector lastTargetPoint;

        // Wzorce kt�re maja si� wykona�
        public List<Patterns.MovementPattern> Patterns { get; private set; }

        public MouseWorker(IEnumerable<Patterns.MovementPattern> movementPatterns)
        {
            Patterns = new List<Patterns.MovementPattern>();
            Patterns.AddRange(movementPatterns);
        }

        /// <summary>
        /// Uruchamia MouseWorker'a w celu wykonania wzorc�w w tle
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
        /// G��wna funkcja przemieszczaj�ca kursor myszki wzgl�dem dt (dt = aktualny_czas - start_czas)
        /// 
        /// Wektor r�nicowy  - wektor reprezentuj�cy drog� jak� pokona�a, lub ma pokona� myszka, 
        ///                     wektor nie zale�y od aktualnego po�o�enia myszki, dzi�ki czemu
        ///                     u�ytkownik ma kontrol� nad myszk� podczas dzia�ania MouseWorker'a
        /// </summary>
        /// <param name="startTime">czas uruchomienia MouseWorker'a</param>
        /// <param name="currentTime">aktualny czas</param>
        private void Move(DateTime startTime, DateTime currentTime)
        {
            // zresetuj wektor r�nicowy
            Vector targetPoint = Vector.Zero;

            // oblicz dt = aktualny_czas - start_czas
            TimeSpan deltaTime = currentTime.Subtract(startTime);

            // oblicz docelowy wektor r�nicowy
            foreach (var pattern in Patterns)
                targetPoint += pattern.GetDelta(deltaTime);

            // oblicz delt� => wektor jaki musi pokona� kursor aby wektor r�nicowy si� zgadza� z docelowym
            Vector deltaPoint = targetPoint - lastTargetPoint;
            
            // Przesu� kursor o wektor delta
            MouseController.Move(deltaPoint.x, deltaPoint.y);

            lastTargetPoint = targetPoint;
            if (deltaPoint != Vector.Zero)
                Console.WriteLine(deltaPoint);
        }

        /// <summary>
        /// Przerywa dzia�anie workera
        /// </summary>
        public void Kill()
        {
            if (thread != null && thread.IsAlive)
                thread.Abort();
        }
    }
}
