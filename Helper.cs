using System.Diagnostics;

namespace testOne {
    public class Helper {

        Stopwatch timer;

        public Helper()
        {
            this.timer = new Stopwatch();
            // this.timer.Start();
        }

        public void startTimer()
        {
            this.timer.Reset();
            this.timer.Start();
        }

        public void stopTimer()
        {
            this.timer.Stop();
            TimeSpan ts = this.timer.Elapsed;

            string elapsedTime = String.Format("{0:00}",
            ts.Milliseconds);

            Console.WriteLine(elapsedTime);
        }
    }
}