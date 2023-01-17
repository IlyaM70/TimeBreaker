using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TimeBreaker
{
    class CountDown
    {
        Timer timer = new Timer(1000);
        public TimeOnly time;

        public CountDown(TimeOnly time)
        {
            this.time = time;
        }

        public void StartTimer()
        {
            timer.Elapsed += OnTimedEvent;
            timer.Start();
                        
        }
        private  void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
            {
                time.Add(new TimeSpan(-1));
                
            }
    }
        
}
